using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Certificates
{
    abstract class AbstractCertExistsCondition:AbstractCondition
    {
        public string AuthorityKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public int Timeout { get; set; }

        public X509RevocationFlag Validate { get; set; }

        public X509VerificationFlags Flags { get; set; }

        public X509RevocationMode Mode { get; set; }

        public StoreName Store { get; set; }

        public StoreLocation Location { get; set; }

        protected AbstractCertExistsCondition(IEngine engine):base(engine)
        {

        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);

            AuthorityKey = XmlUtilities.GetTextFromAttribute(node, "authorityKey");
            Store = XmlUtilities.GetEnumValueFromAttribute(node, "store", StoreName.My);
            Validate = XmlUtilities.GetEnumValueFromAttribute(node, "validate", X509RevocationFlag.EntireChain);
            Mode = XmlUtilities.GetEnumValueFromAttribute(node, "mode", X509RevocationMode.Online);
            Flags = XmlUtilities.GetEnumFlagsValueFromAttribute(node, "flags", X509VerificationFlags.NoFlag);
            Location =
                XmlUtilities.GetEnumValueFromAttribute(node, "location", StoreLocation.CurrentUser);
            Timeout = XmlUtilities.GetIntegerFromAttribute(node, "timeout", 30);
        }
    }
}
