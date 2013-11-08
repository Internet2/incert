using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class ImportCertificatesFromArchive : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        private readonly List<string> _certificates = new List<string>();

        public ImportCertificatesFromArchive(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Archive
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string Certificate
        {
            set
            {
                _certificates.Add(value);
            }
        }

        [PropertyAllowedFromXml]
        public StoreName StoreName { get; set; }

        [PropertyAllowedFromXml]
        public StoreLocation StoreLocation { get; set; }

        [PropertyAllowedFromXml]
        public string FriendlyName { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            X509Store store = null;
            try
            {
                if (string.IsNullOrWhiteSpace(Archive))
                    throw new Exception("The archive parameter cannot be null or empty");
                
                if (!File.Exists(Archive))
                    return new FileNotFound {Target = Archive};

                var trustedResult = CertificateUtilities.IsFileTrusted(Archive);
                if (!trustedResult.Result)
                    return new CouldNotImportContent { Issue = trustedResult.Reason };
                
                if (!_certificates.Any())
                    throw  new Exception("No certificates specified to import");
                
                store = new X509Store(StoreName, StoreLocation);
                store.Open(OpenFlags.ReadWrite);

                foreach (var certName in _certificates)
                {
                    var bytes = CabArchiveUtilities.ExtractFile(Archive, certName);
                    if (bytes == null)
                        throw new Exception(string.Format("Could not extract {0} from archive", certName));

                    if (bytes.Length == 0)
                        throw new Exception(string.Format("Could not extract {0} from archive; invalid content returned", certName));

                    var certificate = new X509Certificate2(bytes);
                    if (store.Certificates.Contains(certificate))
                    {
                        Log.InfoFormat("Certificate {0} already exists in {1}.{2}", certName, StoreLocation, StoreName);
                        continue;
                    }

                    CertificateUtilities.ImportCertificateFromBuffer(certificate.Handle, store.StoreHandle, FriendlyName);
                    if (store.Certificates.Contains(certificate))
                        Log.InfoFormat("Certificate ({0}) added to {1}.{2}!", certName, StoreLocation, StoreName);
                    else
                        Log.WarnFormat("Certificate ({0}) may not have been added successfully", certName);
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
            return "Import root certificates";
        }
    }
}
