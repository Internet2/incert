using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Utilities;

namespace EngineTests.Utilities
{
    [TestClass]
    public class UriUtilitiesTest
    {
        [TestMethod]
        public void TestGetfileFromUriMethod()
        {
            var testUri = new Uri("http://test.test.org/blah/blah/blah.txt", UriKind.RelativeOrAbsolute);
            Assert.IsTrue("blah.txt".Equals(UriUtilities.GetTargetFromUri(testUri)));

            testUri = new Uri("http://test.test.org/blah/blah/blah.txt?blah=12", UriKind.RelativeOrAbsolute);
            Assert.IsTrue("blah.txt".Equals(UriUtilities.GetTargetFromUri(testUri)));

            testUri = new Uri("blah.txt", UriKind.RelativeOrAbsolute);
            Assert.IsTrue("blah.txt".Equals(UriUtilities.GetTargetFromUri(testUri)));

            testUri = new Uri("blah.txt?blah=12", UriKind.RelativeOrAbsolute);
            Assert.IsTrue("blah.txt".Equals(UriUtilities.GetTargetFromUri(testUri)));
        }
    }
}
