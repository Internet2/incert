using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class RemoveSimiliarCertificatesFromStore : AbstractCertificateTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public StoreName StoreName { get; set; }

        public RemoveSimiliarCertificatesFromStore(IEngine engine)
            : base(engine)
        {
            StoreName = StoreName.My;
        }

        public override IResult Execute(IResult previousResults)
        {
            X509Store store = null;
            try
            {
                var certificate = GetCertificateFromWrapper();
                if (certificate == null)
                {
                    Log.Warn("Certificate does not exist in settings store; cannot remove similiar certificates");
                    return new NextResult();
                }

                var authorityKey = CertificateUtilities.GetAuthorityKeyFromCertificate(certificate);
                if (string.IsNullOrWhiteSpace(authorityKey))
                {
                    Log.WarnFormat("Cannot retrieve authority key from certificate; cannot remove similiar certificates");
                    return new NextResult();
                }

                var subjectKey = CertificateUtilities.GetSubjectKeyFromCertificate(certificate);
                if (string.IsNullOrWhiteSpace(subjectKey))
                {
                    Log.WarnFormat("Cannot retrieve subject key from certificate; cannot remove similiar certificates");
                    return new NextResult();
                }

                store = new X509Store(StoreName, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                var instances = new X509Certificate2Collection();
                foreach (var instance in store.Certificates)
                {
                    // shouldn't remove new cert 
                    if (instance.Equals(certificate))
                        continue;
                    
                    if (!authorityKey.Equals(
                        CertificateUtilities.GetAuthorityKeyFromCertificate(instance),
                        StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    if (!subjectKey.Equals(
                        CertificateUtilities.GetSubjectKeyFromCertificate(instance), 
                        StringComparison.InvariantCultureIgnoreCase))
                    
                    Log.InfoFormat("Similar certificate found: serial number {0}; subject name {1}", instance.SerialNumber, instance.SubjectName.Name);
                    instances.Add(instance);
                }

                if (instances.Count == 0)
                    return new NextResult();

                Log.InfoFormat("Removing {0} similar certificates", instances.Count);
                store.RemoveRange(instances);

                var notRemoved = new X509Certificate2Collection();
                foreach (var instance in instances.Cast<X509Certificate2>().Where(instance => store.Certificates.Contains(instance)))
                {
                    notRemoved.Add(instance);
                }

                if (notRemoved.Count == 0)
                    Log.InfoFormat("{0} similiar certificates removed", instances.Count);
                else
                {
                    foreach (var instance in notRemoved)
                        Log.WarnFormat("Certificate with serial number {0} not removed", instance.SerialNumber);
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
                    store.Close();
            }
        }

        public override string GetFriendlyName()
        {
            return "Remove similar certificates from store";
        }
    }
}
