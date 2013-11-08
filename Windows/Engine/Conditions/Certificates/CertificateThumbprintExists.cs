using System;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Certificates
{
    class CertificateThumbprintExists : AbstractCondition
    {
        public string Thumbprint
        {
            get { return GetDynamicValue(); }
            private set { SetDynamicValue(value); }
        }

        public StoreName Store { get; set; }
        public StoreLocation Location { get; set; }

        public CertificateThumbprintExists(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            X509Store store = null;
            try
            {
                if (string.IsNullOrWhiteSpace(Thumbprint))
                    return new BooleanReason(false, "No thumbprint is specified");
                
                store = new X509Store(Store, Location);
                store.Open(OpenFlags.ReadOnly);

                foreach (var certificate in store.Certificates)
                {
                    if (string.IsNullOrWhiteSpace(certificate.Thumbprint))
                        continue;
                    
                    if (certificate.Thumbprint.Equals(Thumbprint, StringComparison.InvariantCultureIgnoreCase))
                        return new BooleanReason(true, "Certificate matching thumbprint {0} found!");

                }

                return new BooleanReason(false, "No certificate matching thumbprint {0} found!");
            }
            catch (Exception e)
            {
                return new BooleanReason(false,
                                         "An issue occurred while attempting to locate certificate for thumbprint {0}: {1}",
                                         Thumbprint, e.Message);
            }
            finally
            {
                if (store !=null)
                    store.Close();
            }
        }

        public override bool IsInitialized()
        {
            if (!IsPropertySet("Thumbprint"))
                return false;

            if (Store == 0)
                return false;

            if (Location == 0)
                return false;

            return true;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Thumbprint = XmlUtilities.GetTextFromAttribute(node, "thumbprint");
            Store = XmlUtilities.GetEnumValueFromAttribute(node, "store", StoreName.My);
            Location = XmlUtilities.GetEnumValueFromAttribute(node, "location", StoreLocation.CurrentUser);
        }
    }
}
