using System;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class RetrieveUserCertificate : AbstractCertificateTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string UsernameKey { get; set; }

        [PropertyAllowedFromXml]
        public string PassphraseKey { get; set; }

        [PropertyAllowedFromXml]
        public string Credential2Key { get; set; }

        [PropertyAllowedFromXml]
        public string Credential3Key { get; set; }

        [PropertyAllowedFromXml]
        public string Credential4Key { get; set; }
        
        [PropertyAllowedFromXml]
        public string CertificateProvider
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public int Timeout { get; set; }

        [PropertyAllowedFromXml]
        public bool EncryptCertificate { get; set; }

        public RetrieveUserCertificate(IEngine engine)
            : base(engine)
        {
            Timeout = 90000;
            EncryptCertificate = true;
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var request =
                    EndpointManager.GetContract<AbstractGetCertificateContract>(
                        EndPointFunctions.GetCertificate);

                request.LoginId = SettingsManager.GetTemporarySettingString(UsernameKey);
                request.Credential1 = SettingsManager.GetTemporarySettingString(PassphraseKey);
                request.Credential2 = SettingsManager.GetTemporarySettingString(Credential2Key);
                request.Credential3 = SettingsManager.GetTemporarySettingString(Credential3Key);
                request.Credential4 = SettingsManager.GetTemporarySettingString(Credential4Key);
                request.Provider = CertificateProvider;
                request.EncryptCertificate = EncryptCertificate ? "Yes" : "No";
                Timeout = Timeout;
                

                var result = request.MakeRequest<CertificateWrapper>();
                if (result == null)
                    return request.GetErrorResult();

                var bytes = Convert.FromBase64String(result.Pkcs12);

                var password = SettingsManager.GetSecureTemporarySettingString(PassphraseKey);
                X509Certificate2 certificate;
                if (password == null)
                {
                    Log.Info("creating certificate without password");
                    certificate = new X509Certificate2(bytes);
                }
                else
                {
                    Log.Info("creating certificate with password");
                    certificate = new X509Certificate2(bytes, password);
                }

                // save the text and the the actual cert
                var wrapper = new CertificateDataWrapper
                    {
                        Certificate = certificate,
                        Bytes = bytes,
                        Text = result.Pkcs12
                    };
                
                SettingsManager.SetTemporaryObject(CertificateWrapperKey, wrapper);
                
                return new NextResult();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            return "Retrieve user certificate";
        }
    }
}
