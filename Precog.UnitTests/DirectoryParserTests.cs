using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Precog.Core;

namespace Precog.UnitTests
{
    [TestClass]
    public class DirectoryParserTests
    {
        Mock<IFileEnumerator> _fakeFileSystem = new Mock<IFileEnumerator>();

        [TestInitialize]
        public void BeforeEachTest()
        {
            _fakeFileSystem.Setup(s => s.EnumerateFiles(It.IsAny<string>(), "*.config"))
                .Returns(new[] { @"C:\\Temp\web.config", @"C:\Temp\nlog.config" });
        }

        private DirectoryParser CreateDirectoryParser()
        {
            return new DirectoryParser(_fakeFileSystem.Object);
        }

        [TestMethod]
        public void CanParseOneDirectory()
        {
            var parser = CreateDirectoryParser();
            var configs = parser.Parse("NotRelevant", "*.config");

            Assert.AreEqual(2, configs.Count());
        }

        [TestMethod]
        public void CanExcludePatterns()
        {
            var parser = CreateDirectoryParser();
            parser.Excludes.Add("nlog.config");

            var configs = parser.Parse(@"NotRelevant", "*.config");

            Assert.AreEqual(1, configs.Count());
        }

        [TestMethod]
        public void ExcludePatternsIsCaseInsensitive()
        {
            var parser = CreateDirectoryParser();
            parser.Excludes.Add("nLoG.ConFig");

            var configs = parser.Parse(@"NotRelevant", "*.config");

            Assert.AreEqual(1, configs.Count());
        }

        [TestMethod]
        public void CanExcludeDirectory()
        {
            var parser = CreateDirectoryParser();
            parser.Excludes.Add("Temp");

            var configs = parser.Parse(@"NotRelevant", "*.config");

            Assert.AreEqual(0, configs.Count());
        }
    }
}
