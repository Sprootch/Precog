using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Precog.Core
{
    public class ConfigFileChecker
    {
        DirectoryParser _directorParser = new DirectoryParser(new FileSystemEnumerator());
        public event EventHandler<int> ProgressChanged;

        protected virtual void OnProgressChanged(int i)
        {
            ProgressChanged?.Invoke(this, i);
        }

        public void AddExcludes(params string[] excludes)
        {
            _directorParser.Excludes.AddRange(excludes);
        }

        public async Task CheckAsync(string path, IProgress<ConfigMessage> progress = null)
        {
            var files = _directorParser.Parse(path, "*.config");
            await Task.Run(() => ParseConfigFiles(files, progress));
        }

        private void ParseConfigFiles(IEnumerable<string> files, IProgress<ConfigMessage> progress = null)
        {
            double i = 0;

            foreach (var file in files)
            {
                i++;
                //var percentage = (int)((i / files.Count()) * 100.0);
                double dProgress = (i / files.Count()) * 100.0;
                OnProgressChanged((int)dProgress);

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
                    if (clientService.IsLocalHost)
                    {
                        progress?.Report(ConfigMessage.Warning("WARNING"));
                        progress?.Report(ConfigMessage.Warning("Service is localhost...Skipping..."));
                        continue;
                    }

                    IReadOnlyCollection<Service> serverServices;
                    using (var remoteConfigRetriever = new RemoteConfigRetriever())
                    {
                        var generatedConfigFile = remoteConfigRetriever.GetRemoteConfiguration(clientService.Address);
                        if (generatedConfigFile == null)
                        {
                            progress?.Report(ConfigMessage.Warning("Could not generate server config file!"));
                            continue;
                        }
                        var serverConfig = ConfigFileOpener.Open(generatedConfigFile).Value;
                        serverServices = ServiceParser.GetServices(serverConfig);
                    }

                    var correspondingService = serverServices.FirstOrDefault(s => s.Binding == clientService.Binding);
                    if (correspondingService == null)
                    {
                        progress?.Report(ConfigMessage.Error("No matching service found!"));
                        continue;
                    }

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
