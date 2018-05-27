using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Precog.UnitTests
{
    [TestClass]
    public class PrecogEngineTests
    {
        [TestMethod]
        public async Task MyTestMethodAsync()
        {
            IEnumerable<IConfigPlugin> plugins = new List<IConfigPlugin>
            {
                new DummyPlugin()
            };
            var engine = new PrecogEngine(plugins);
            IProgress<string> p = new Progress<string>(i => Trace.WriteLine(i));

            var t = await engine.AnalyzeAsync("configFilePath", p);

            Assert.IsNotNull(t);
        }
    }

    public interface IConfigPlugin
    {
        Task<List<string>> CheckAsync(Configuration configurationFile);
    }

    class DummyPlugin : IConfigPlugin
    {
        public async Task<List<string>> CheckAsync(Configuration configurationFile)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            return await Task.FromResult(new List<string> { "A", "N", });

            //return await Task.Run(() =>
            //{
            //    Thread.Sleep(TimeSpan.FromSeconds(10));

            //    return new List<string> { "A", "N", };
            //});
        }
    }
}
