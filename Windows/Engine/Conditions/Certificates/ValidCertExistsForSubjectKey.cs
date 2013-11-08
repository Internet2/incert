using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Certificates
{
    class ValidCertExistsForSubjectKey : AbstractCertExistsCondition
    {
        
        public string SubjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ValidCertExistsForSubjectKey(IEngine engine)
            : base(engine)
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

                    var subjectKey = CertificateUtilities.GetSubjectKeyFromCertificate(certificate);
                    if (!SubjectKey.Equals(subjectKey, StringComparison.InvariantCultureIgnoreCase))
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

            if (!IsPropertySet("SubjectKey"))
                return false;

            return true;
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            SubjectKey = XmlUtilities.GetTextFromAttribute(node, "subjectKey");
        }
    }
}
