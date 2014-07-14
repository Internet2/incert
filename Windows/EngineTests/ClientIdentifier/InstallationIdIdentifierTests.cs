using System.Collections.ObjectModel;
using NSubstitute;
using NUnit.Framework;
using Org.InCommon.InCert.Engine.ClientIdentifier;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;

namespace EngineTests.ClientIdentifier
{
    [TestFixture]
    public class InstallationIdIdentifierTests
    {
        public static object[] CaseSourceTestData =
            {
                new object[] { "", "{499D6EE1-E87E-4950-92C7-37198BCC2A46}", "{499D6EE1-E87E-4950-92C7-37198BCC2A46}" },
                new object[] {"TestingId","TestingId", "TestingId".ToSha1Hash() }
    
            };

        [Test, TestCaseSource("CaseSourceTestData")]
        public void TestIdentifierExtension(string installationId, string backupId, string expected)
        {

            var mockCollection = Substitute.For<SoftwareLicensingProduct.SoftwareLicensingProductCollection>();
            mockCollection.GetEnumerator().Returns(new Collection<SoftwareLicensingProduct>
            {
                new MockSoftwareLicensingProduct(installationId)
            }.GetEnumerator());

            var mockProxy = Substitute.For<ISoftwareLicensingProductProxy>();
            mockProxy.GetInstances().Returns(mockCollection);

            var mockBackup = Substitute.For<IClientIdentifier>();
            mockBackup.GetIdentifier().Returns(backupId);

            var identifier = new InstallationIdClientIdentifier(mockProxy, mockBackup);
            Assert.That(identifier.GetIdentifier(), Is.EqualTo(expected));
        }


        protected class MockSoftwareLicensingProduct : SoftwareLicensingProduct
        {
            private readonly string _installationId;

            public MockSoftwareLicensingProduct(string installationId)
            {
                _installationId = installationId;
            }

            public override string PartialProductKey
            {
                get
                {
                    return "testing";
                }
            }

            public override string ApplicationID
            {
                get { return "55c92734-d682-4d71-983e-d6ec3f16059f"; }
            }

            public override string OfflineInstallationId
            {
                get { return _installationId; }
            }
        }


    }


}
