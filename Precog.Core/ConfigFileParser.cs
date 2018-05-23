using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel.Configuration;

namespace Precog.Core
{
    public class ConfigFileParser
    {
        private readonly Configuration _configFile;

        public ConfigFileParser(string configFilePath)
        {
            if (configFilePath == null) throw new ArgumentNullException("configFilePath");
            if (!File.Exists(configFilePath)) throw new ArgumentException("Config file does not exists!");

            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFilePath
            };
            _configFile = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        public IReadOnlyCollection<Service> GetServices()
        {
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(_configFile);
            var endpoints = serviceModel.Client.Endpoints;

            var list = new List<Service>();
            for (int i = 0; i < endpoints.Count; i++)
            {
                var endpointElement = endpoints[i];

                var service = new Service(endpointElement.Address.OriginalString, endpointElement.Identity.UserPrincipalName.Value);

                list.Add(service);
            }

            return list;
        }
    }
}
