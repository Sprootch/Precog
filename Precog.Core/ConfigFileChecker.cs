using System.Collections.Generic;

namespace Precog.Core
{
    public enum ConfigStatus
    {
        Undefined,
        Success,
        Error
    }

    public class ConfigFileResult
    {
        public string ConfigFile { get; set; }
        public string Result { get; set; }
        public ConfigStatus Status { get; set; }
    }

    public class ConfigFileChecker
    {
        DirectoryParser dirParser = new DirectoryParser(new FileSystemEnumerator());

        public void AddExcludes(params string[] excludes)
        {
            dirParser.Excludes.AddRange(excludes);
        }

        public List<ConfigFileResult> Check(string path)
        {
            var files = dirParser.Parse(path, "*.config");

            List<ConfigFileResult> results = new List<ConfigFileResult>();
            foreach (var file in files)
            {
                var configAnalyzer = new ConfigFileParser(file);

                var result = configAnalyzer.Analyze();
                if (result.IsFailure)
                {
                    results.Add(new ConfigFileResult { ConfigFile = file, Result = result.Error, Status = ConfigStatus.Error });
                }
                else
                {
                    results.Add(new ConfigFileResult { ConfigFile = file, Result = result.Value, Status = ConfigStatus.Success });
                }
            }

            return results;
        }
    }
}
