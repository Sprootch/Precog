using System;
using System.IO;
using Precog.Core;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var d = new DirectoryParser(new FileSystemEnumerator());
            d.Excludes.Add("NLog.config");
            d.Excludes.Add("transform");
            d.Excludes.Add("artifact_bkp");

            var configs = d.Parse(@"C:\Projects\Censy\sources\Deploy\", "*.config");

            foreach (var config in configs)
            {
                ProcessConfig(config);
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static void ProcessConfig(string config)
        {
            try
            {
                Console.WriteLine(config);
                var services = new ConfigFileParser(config).GetServices();
                foreach (var service in services)
                {
                    Console.WriteLine(service.Address);
                    Console.WriteLine(service.Identity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
