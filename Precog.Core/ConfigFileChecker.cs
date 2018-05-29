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

                progress?.Report(ConfigMessage.Success("OK"));
            }
        }

        public List<ConfigFileResult> Check(string path)
        {
            var files = _directorParser.Parse(path, "*.config");

            List<ConfigFileResult> results = new List<ConfigFileResult>();
            foreach (var file in files)
            {
                var configParser = new ConfigFileParser(file);

                var result = configParser.Analyze();
                if (result.IsFailure)
                {
                    results.Add(new ConfigFileResult { ConfigFile = file, Result = result.Error, Status = Severity.Error });
                }
                else
                {
                    results.Add(new ConfigFileResult { ConfigFile = file, Result = result.Value, Status = Severity.Success });
                }
            }

            return results;
        }
    }
}
