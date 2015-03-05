using System;
using NUnit.Framework;
using Org.InCommon.InCert.Engine.Utilities;

namespace EngineTests.Utilities
{
    [TestFixture]
    public class UriUtilitiesTest
    {
        [Test]
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
