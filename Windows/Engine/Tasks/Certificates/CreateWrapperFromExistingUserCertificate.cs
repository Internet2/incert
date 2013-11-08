using System;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class CreateWrapperFromExistingUserCertificate:AbstractCertificateTask
    {
        public CreateWrapperFromExistingUserCertificate(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string AuthorityKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string EmailAddress
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            X509Store store = null;
            try
            {
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
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

                    var wrapper = new CertificateDataWrapper { Certificate = certificate };
                    SettingsManager.SetTemporaryObject(CertificateWrapperKey, wrapper);
                    break;
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
        }

        public override string GetFriendlyName()
        {
            return "Create wrapper for existing certificate";
        }
    }
}
