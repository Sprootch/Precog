using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Precog.UnitTests
{
    internal class PrecogEngine
    {
        private IEnumerable<IConfigPlugin> plugins;

        public PrecogEngine(IEnumerable<IConfigPlugin> plugins)
        {
            this.plugins = plugins;
        }

        public async Task<List<string>> AnalyzeAsync(string configFilePath, IProgress<string> progress)
        {
            var p = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFilePath
            }, ConfigurationUserLevel.None);

            List<string> s = null;
            foreach (var plugin in plugins)
            {
                progress.Report("START " + plugin.GetType().Name);
                s = await plugin.CheckAsync(p);
                progress.Report("END");
            }

            return s;
        }
    }
}