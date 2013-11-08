using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Utilities;

namespace EngineTests.Utilities
{
    [TestClass]
    public class XmlUtilitiesTests
    {
        [TestMethod]
        public void TestGetEnumFlagsValueFromAttribute()
        {
            const string testString1 = "IgnoreNotTimeValid";
            const string testString2 = "IgnoreNotTimeValid IgnoreNotTimeNested";
            const string testString3 = "invalidValue";

            var oneFlagElement = new XElement("Root", new XAttribute("flags", testString1));
            var twoFlagElement = new XElement("Root", new XAttribute("flags", testString2));
            var invalidFlagElement = new XElement("Root", new XAttribute("flags", testString3));
            var validButWithInvalidElement = new XElement("Root",
                new XAttribute("flags", testString2 + " " + testString3));

            var result = XmlUtilities.GetEnumFlagsValueFromAttribute(oneFlagElement, "flags",
                X509VerificationFlags.NoFlag);
            
            Assert.AreEqual(result, X509VerificationFlags.IgnoreNotTimeValid, "Attribute with one value set should equal value");

            result = XmlUtilities.GetEnumFlagsValueFromAttribute(twoFlagElement, "flags", X509VerificationFlags.NoFlag);
            Assert.AreEqual(result, (X509VerificationFlags.IgnoreNotTimeValid | X509VerificationFlags.IgnoreNotTimeNested), "Attribute with multiple values set should equal value");

            result = XmlUtilities.GetEnumFlagsValueFromAttribute(invalidFlagElement, "flags",
                X509VerificationFlags.NoFlag);
            Assert.AreEqual(result, X509VerificationFlags.NoFlag, "A single invalid value should produce the default value");

            result = XmlUtilities.GetEnumFlagsValueFromAttribute(validButWithInvalidElement, "flags",
                                                                 X509VerificationFlags.NoFlag);
            Assert.AreEqual(result, (X509VerificationFlags.IgnoreNotTimeValid | X509VerificationFlags.IgnoreNotTimeNested), "Attribute with multiple values set should equal value of valid elements, ignoring invalid elements");

        }
    }
}
