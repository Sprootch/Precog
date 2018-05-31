using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Precog.Core
{
    public class RemoteConfigRetriever : IDisposable
    {
        private readonly string _configFilePath;

        public RemoteConfigRetriever()
        {
            _configFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".config");
        }

        public string GetRemoteConfiguration(string serviceAddress)
        {
            var pInfo = new ProcessStartInfo(@"svcutil.exe", $"{serviceAddress} /config:{_configFilePath}")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(pInfo)
                .WaitForExit(500);

            return File.Exists(_configFilePath)
                ? _configFilePath
                : null;
        }

        private static void TryDeleteFile(string configFilePath)
        {
            try
            {
                File.Delete(configFilePath);
            }
            catch (Exception)
            {
            }
        }

        #region IDisposable

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                TryDeleteFile(_configFilePath);
            }

            disposed = true;
        }
        #endregion
    }
}
