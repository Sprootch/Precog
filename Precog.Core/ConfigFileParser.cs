using System;
using System.Configuration;
using System.IO;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Precog.Core
{
    public class ConfigFileParser
    {
        private Configuration _configFile;
        private readonly string configFilePath;

        public ConfigFileParser(string configFilePath)
        {
            if (configFilePath == null) throw new ArgumentNullException("configFilePath");
            if (!File.Exists(configFilePath)) throw new ArgumentException("Config file does not exists!");

            this.configFilePath = configFilePath;
        }

        public async Task<Result<string>> AnalyzeAsync()
        {
            return await Task.Run(() => Analyze());
        }

        public Result<string> Analyze()
        {
            ChannelEndpointElementCollection endpoints;
            try
            {
                ExeConfigurationFileMap configMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configFilePath
                };
                _configFile = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                endpoints = ReadClientServices();
            }
            catch (Exception e)
            {
                return Result.Fail<string>(e.Message);
            }

            //for (int i = 0; i < endpoints.Count; i++)
            //{
            //    var endpointElement = endpoints[i];

            //    var service = new Service(endpointElement.Address.OriginalString, endpointElement.Identity.UserPrincipalName.Value);
            //}

            return Result.Ok("OK");
        }

        private ChannelEndpointElementCollection ReadClientServices()
        {
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(_configFile);
            var endpoints = serviceModel.Client.Endpoints;
            return endpoints;
        }
    }
}
