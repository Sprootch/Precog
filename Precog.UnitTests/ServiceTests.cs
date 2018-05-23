using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precog.Core;

namespace Precog.UnitTests
{
    [TestClass]
    public class ServiceTests
    {
        [TestMethod]
        public void ServiceEqualsMethod()
        {
            var svc1 = new Service("ABCDEF", "User1");
            var svc2 = new Service("ABCDEF", "User1");

            Assert.IsTrue(svc1.Equals(svc2));
        }

        [TestMethod]
        public void ObjectEqualsMethod()
        {
            object svc1 = new Service("ABCDEF", "User1");
            object svc2 = new Service("ABCDEF", "User1");

            Assert.IsTrue(svc1.Equals(svc2));
        }

        [TestMethod]
        public void NullEqualsMethod()
        {
            object svc1 = new Service("ABCDEF", "User1");
            object svc2 = null;

            Assert.IsFalse(svc1.Equals(svc2));
        }
    }
}
