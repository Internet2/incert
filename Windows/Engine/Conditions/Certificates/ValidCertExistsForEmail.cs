using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Certificates
{
    class ValidCertExistsForEmail:AbstractCertExistsCondition
    {
        public string EmailAddress
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ValidCertExistsForEmail(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            X509Store store = null;
            try
            {
                store = new X509Store(Store, Location);
                store.Open(OpenFlags.ReadOnly);

                foreach (var certificate in store.Certificates)
                {
                    var authorityKey = CertificateUtilities.GetAuthorityKeyFromCertificate(certificate);
                    if (!AuthorityKey.Equals(authorityKey, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var email = certificate.GetNameInfo(X509NameType.EmailName, false);
                    if (string.IsNullOrWhiteSpace(email))
                        continue;

                    if (!email.Equals(EmailAddress, StringComparison.InvariantCulture))
                        continue;

                    return CertificateUtilities.VerifyCertificate(certificate, Validate,
                                                                  Mode, new TimeSpan(0, 0, 0, Timeout),
                                                                  Flags);
                }

                return new BooleanReason(false, "no matching certificates found");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, e.Message);
            }
            finally
            {
                if (store != null)
                    store.Close();
            }
        }

        public override bool IsInitialized()
        {
            if (Store == 0)
                return false;

            if (Location == 0)
                return false;

            if (!IsPropertySet("AuthorityKey"))
                return false;

            if (!IsPropertySet("EmailAddress"))
                return false;

            return true;
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            EmailAddress = XmlUtilities.GetTextFromAttribute(node, "address");
        }
    }
}
