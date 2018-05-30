using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Precog.Core
{
    public class ConfigFileChecker
    {
        DirectoryParser _directorParser = new DirectoryParser(new FileSystemEnumerator());

        public void AddExcludes(params string[] excludes)
        {
            _directorParser.Excludes.AddRange(excludes);
        }

        public async Task CheckAsync(string path, IProgress<ConfigMessage> progress = null)
        {
            var files = _directorParser.Parse(path, "*.config");
            await Task.Run(() => ParseConfigFiles(files, progress));
        }

        private static void ParseConfigFiles(IEnumerable<string> files, IProgress<ConfigMessage> progress = null)
        {
            foreach (var file in files)
            {
                progress.Report(ConfigMessage.Info(file));
                var config = ConfigFileOpener.Open(file);
                if (config.IsFailure)
                {
                    progress?.Report(ConfigMessage.Error("ERROR"));
                    progress?.Report(ConfigMessage.Error(config.Error));
                    continue;
                }

                var result = new ConfigFileParser(file).Analyze();
                if (result.IsFailure)
                {
                    progress?.Report(ConfigMessage.Error("ERROR"));
                    progress?.Report(ConfigMessage.Error(result.Error));
                    continue;
                }

                var clientServices = ServiceParser.GetServices(config.Value);
                foreach (var clientService in clientServices)
                {
                    progress?.Report(ConfigMessage.Info(clientService.ToString()));
                    var generatedConfigFile = new RemoteConfigRetriever().GetRemoteConfiguration(clientService.Address);
                    Thread.Sleep(1000);
                    var serverConfig = ConfigFileOpener.Open(generatedConfigFile).Value;
                    var serverServices = ServiceParser.GetServices(serverConfig);

                    var correspondingService = serverServices.First(s => s.Binding == clientService.Binding);

                    if (!correspondingService.Equals(clientService))
                    {
                        progress?.Report(ConfigMessage.Error("Client configuration does not match server configuration!"));
                        progress?.Report(ConfigMessage.Error(correspondingService.ToString()));
                    }
                    else
                    {
                        progress?.Report(ConfigMessage.Success("OK"));
                    }
                }
            }
        }
    }
}
