using System;
using System.IO;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class ImportUserCertificateFromFile : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string CredentialsKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Target
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ImportUserCertificateFromFile(IEngine engine):base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Target))
                    throw new InvalidOperationException("The target path cannot be empty");

                if (!File.Exists(Target))
                    Target = Path.Combine(PathUtilities.DownloadFolder, Target);

                if (!File.Exists(Target))
                    return new FileNotFound { Target = Target };

                var result = CertificateUtilities.ImportUserCertificateFromFile(
                    Target,
                    SettingsManager.GetSecureTemporarySettingString(CredentialsKey));

                if (!result.Result)
                    return new CouldNotImportContent{Issue = result.Reason};

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }
        
        public override string GetFriendlyName()
        {
            return "Import certificate from file";
        }
    }
}
