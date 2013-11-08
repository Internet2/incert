using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class ImportCertificateFromSettings : AbstractCertificateTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string CredentialsKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string FriendlyName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ImportCertificateFromSettings(IEngine engine):base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var bytes = GetCertificateBytesFromWrapper();
                
                var result = CertificateUtilities.ImportUserCertificateFromBuffer(bytes,
                    SettingsManager.GetSecureTemporarySettingString(CredentialsKey), FriendlyName);

                if (!result.Result)
                    return new CouldNotImportContent { Issue = result.Reason };

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
            return "Install certificate from settings store";
        }
    }
}
