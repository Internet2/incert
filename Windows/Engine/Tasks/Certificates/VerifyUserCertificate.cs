using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Certificates;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class VerifyUserCertificate:AbstractCertificateTask
    {
        private static readonly ILog Log = Logger.Create();
        
        private TimeSpan? _warnTimespan;

        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public X509RevocationMode Mode { get; set; }

        [PropertyAllowedFromXml]
        public X509RevocationFlag Validate { get; set; }

        [PropertyAllowedFromXml]
        public X509VerificationFlags Flags { get; set; }

        [PropertyAllowedFromXml]
        public int Timeout { get; set; }

        [PropertyAllowedFromXml]
        public int DaysToWarnBeforeExpires
        {
            set
            {
                _warnTimespan =  new TimeSpan(value,0,0,0).Duration();
            }
        }

        public VerifyUserCertificate(IEngine engine) : base(engine)
        {
            TargetPath = Path.Combine(PathUtilities.UtilityAppDataFolder, "CertInfo.xml");
            Mode = X509RevocationMode.Online;
            Validate = X509RevocationFlag.EntireChain;
            Flags = X509VerificationFlags.NoFlag;
            Timeout = 90;
            _warnTimespan = new TimeSpan(30,0,0,0);
        }

        public override IResult Execute(IResult previousResults)
        {
            X509Store store = null;
            try
            {   
                if (!File.Exists(TargetPath))
                {
                    Log.WarnFormat("The file {0} does not exist. Cannot evaluate certificate status", TargetPath);
                    return new FileNotFound{Issue = TargetPath};
                }

                var wrapper = CertificatePropertiesWrapper.Deserialize(TargetPath);
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

                var validCerts = GetMatchingValidCerts(wrapper);
                if (!validCerts.Any())
                    return new UserCertNotPresent();

                validCerts = GetUpToDateCertificates(validCerts);
                if (!validCerts.Any())
                    return new UserCertWillExpire(_warnTimespan);

                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (store !=null)
                    store.Close();
            }
        }

        private List<X509Certificate2> GetMatchingValidCerts(CertificatePropertiesWrapper wrapper)
        {
            X509Store store = null;
            try
            {
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open( OpenFlags.ReadOnly);
                var result = new List<X509Certificate2>();
                foreach (var certificate in store.Certificates)
                {
                    if (!wrapper.AuthorityKey.Equals(
                        CertificateUtilities.GetAuthorityKeyFromCertificate(certificate), 
                        StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    if (!wrapper.SubjectKey.Equals(
                        CertificateUtilities.GetSubjectKeyFromCertificate(certificate),
                        StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    if (!CertificateUtilities.VerifyCertificate(
                        certificate,
                        Validate,
                        Mode,
                        new TimeSpan(0,0,0,Timeout), 
                        Flags).Result)
                        continue;
                    
                    result.Add(certificate);
                }

                return result;
            }
            finally
            {
                if (store !=null)
                    store.Close();
            }
        } 

        private List<X509Certificate2> GetUpToDateCertificates(List<X509Certificate2> certificates)
        {
            if (!certificates.Any())
                return certificates;

            if (!_warnTimespan.HasValue)
                return certificates;

            var testDate = DateTime.UtcNow.Add(_warnTimespan.Value);

            return certificates
                .Where(certificate => testDate >= certificate.NotBefore)
                .Where(certificate => testDate <= certificate.NotAfter).ToList();
        }

        public override string GetFriendlyName()
        {
            return "verify certificate from info file";
        }
    }
}
