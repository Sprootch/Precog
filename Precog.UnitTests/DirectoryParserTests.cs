using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precog.Core;

namespace Precog.UnitTests
{
    [TestClass]
    public class DirectoryParserTests
    {
        [TestMethod]
        public void CanParseOneDirectory()
        {
            var d = new DirectoryParser();

            var configs = d.Parse(@"C:\Projects\Censy\sources\Deploy\ActionListItem\artifact\DEV", "*.config");

            Assert.AreEqual(2, configs.Count());
        }

        [TestMethod]
        public void CanExcludePatterns()
        {
            var d = new DirectoryParser();
            d.Excludes.Add("nlog.config");

            var configs = d.Parse(@"C:\Projects\Censy\sources\Deploy\ActionListItem\artifact\DEV", "*.config");

            Assert.AreEqual(1, configs.Count());
        }

        [TestMethod]
        public void ExcludePatternsIsCaseInsensitive()
        {
            var d = new DirectoryParser();
            d.Excludes.Add("nLoG.ConFig");

            var configs = d.Parse(@"C:\Projects\Censy\sources\Deploy\ActionListItem\artifact\DEV", "*.config");

            Assert.AreEqual(1, configs.Count());
        }
    }
}
