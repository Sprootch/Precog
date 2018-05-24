using System.Collections.Generic;

namespace Precog.Core
{
    public class ConfigFileChecker
    {
        DirectoryParser _directorParser = new DirectoryParser(new FileSystemEnumerator());

        public void AddExcludes(params string[] excludes)
        {
            _directorParser.Excludes.AddRange(excludes);
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
