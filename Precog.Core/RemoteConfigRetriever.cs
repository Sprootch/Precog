using System.Configuration;
using System.Diagnostics;

namespace Precog.Core
{
    public class RemoteConfigRetriever
    {
        public Configuration GetRemoteConfiguration(string serviceAddress)
        {
            var pInfo = new ProcessStartInfo(@"svcutil.exe", serviceAddress)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(pInfo);

            return ConfigFileOpener.Open(@"output.config");
        }
    }
}
