using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Precog.Core
{
    class ServiceParser
    {
        public static IReadOnlyCollection<Service> GetServices(Configuration configuration)
        {
            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(configuration);
            var endpoints = serviceModel.Client.Endpoints;

            List<Service> services = new List<Service>();
            for (int i = 0; i < endpoints.Count; i++)
            {
                var endpointElement = endpoints[i];

                var service = new Service(endpointElement.Address.OriginalString, 
                    endpointElement?.Identity?.UserPrincipalName?.Value ?? 
                    endpointElement?.Identity?.ServicePrincipalName?.Value, endpointElement.Binding);

                services.Add(service);
            }

            return services;
        }
    }
}
