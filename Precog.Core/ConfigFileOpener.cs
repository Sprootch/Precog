using System;
using System.Configuration;
using CSharpFunctionalExtensions;

namespace Precog.Core
{
    internal static class ConfigFileOpener
    {
        public static Result<Configuration> Open(string configFilePath)
        {
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFilePath
            };

            try
            {
                var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                return Result.Ok(config);
            }
            catch (Exception e)
            {
                return Result.Fail<Configuration>(e.Message);
            }
        }
    }
}
