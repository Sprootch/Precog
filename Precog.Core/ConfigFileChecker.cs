using System;
using System.Collections.Generic;
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

                var services = ServiceParser.GetServices(config.Value);
                foreach (var service in services)
                {
                    progress.Report(ConfigMessage.Info(service.ToString()));
                    var remoteConfig = new RemoteConfigRetriever().GetRemoteConfiguration(service.Address);

                    //var x = ServiceParser.GetServices(remoteConfig);
                }


                progress?.Report(ConfigMessage.Success("OK"));
            }
        }
    }
}
