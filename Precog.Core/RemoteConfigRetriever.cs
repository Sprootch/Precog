using System.Configuration;
using System.Diagnostics;

namespace Precog.Core
{
    public class RemoteConfigRetriever
    {
        public string GetRemoteConfiguration(string serviceAddress)
        {
            var pInfo = new ProcessStartInfo(@"svcutil.exe", serviceAddress)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(pInfo);

            return @"output.config";
        }
    }
}
