using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    public abstract class AbstractCertificateTask : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        protected AbstractCertificateTask(IEngine engine)
            : base(engine)
        {

        }

        [PropertyAllowedFromXml]
        public string CertificateWrapperKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        internal string GetCertificateTextFromWrapper()
        {
            var wrapper = GetWrapperForKey();
            return wrapper == null ? "" : wrapper.Text;
        }

        internal X509Certificate2 GetCertificateFromWrapper()
        {
            var wrapper = GetWrapperForKey();
            return wrapper == null ? null : wrapper.Certificate;
        }

        internal byte[] GetCertificateBytesFromWrapper()
        {
            var wrapper = GetWrapperForKey();
            return wrapper == null ? null : wrapper.Bytes;
        }

        private CertificateDataWrapper GetWrapperForKey()
        {
            var wrapper = SettingsManager.GetTemporaryObject(CertificateWrapperKey) as CertificateDataWrapper;
            if (wrapper == null)
            {
                Log.WarnFormat("Cannot retrieve certificate data wrapper for key {0}", CertificateWrapperKey);
                return null;
            }

            return wrapper;
        }

        internal class CertificateDataWrapper
        {
            public X509Certificate2 Certificate { get; set; }
            public byte[] Bytes { get; set; }
            public string Text { get; set; }
        }

        [Serializable]
        public class CertificatePropertiesWrapper
        {
            public CertificatePropertiesWrapper()
            {

            }

            public CertificatePropertiesWrapper(X509Certificate2 certificate)
            {
                AuthorityKey = CertificateUtilities.GetAuthorityKeyFromCertificate(certificate);
                SubjectKey = CertificateUtilities.GetSubjectKeyFromCertificate(certificate);
            }

            public string AuthorityKey { get; set; }
            public string SubjectKey { get; set; }

            public void Serialize(string targetPath)
            {
                PathUtilities.EnsureDirectoryExists(targetPath);

                using (var stream = File.Open(targetPath, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(CertificatePropertiesWrapper));
                    serializer.Serialize(stream, this);
                }
            }

            public static CertificatePropertiesWrapper Deserialize(string targetPath)
            {
                using (var stream = new FileStream(targetPath, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(CertificatePropertiesWrapper));
                    return (CertificatePropertiesWrapper)serializer.Deserialize(stream);
                }
            }
        }
    }
}
