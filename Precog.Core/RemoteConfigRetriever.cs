using System.Diagnostics;
using System.IO;

namespace Precog.Core
{
    public class RemoteConfigRetriever
    {
        public string GetRemoteConfiguration(string serviceAddress)
        {
            var configFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".config");
            var pInfo = new ProcessStartInfo(@"svcutil.exe", $"{serviceAddress} /config:{configFilePath}")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(pInfo)
                .WaitForExit(500);
            
            return configFilePath;
        }
    }
}
