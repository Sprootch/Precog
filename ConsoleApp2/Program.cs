using System;
using NLog;
using Precog.Core;

namespace ConsoleApp2
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var checker = new ConfigFileChecker();
            checker.AddExcludes("NLog.config", "transform", "artifact_bkp");

            var results = checker.Check(@"C:\Projects\Censy\sources\Deploy\");

            foreach (var result in results)
            {
                _logger.Debug(result.ConfigFile);
                if (result.Status == ConfigStatus.Success)
                {
                    _logger.Info(result.Result);
                }
                else
                {
                    _logger.Error(result.Result);
                }
            }

            Console.ReadKey();
        }
    }
}
