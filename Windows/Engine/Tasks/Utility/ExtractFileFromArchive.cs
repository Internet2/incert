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

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class ExtractFileFromArchive:AbstractTask
    {
        public ExtractFileFromArchive(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Archive
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string TargetFile
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DestinationFile
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Archive))
                    throw new Exception("The archive parameter cannot be null or empty");

                if (string.IsNullOrWhiteSpace(TargetFile))
                    throw new Exception("The icon parameter cannot be null or empty");

                if (!File.Exists(Archive))
                    return new FileNotFound { Target = Archive };

                var trustedResult = CertificateUtilities.IsFileTrusted(Archive);
                if (!trustedResult.Result)
                    return new CouldNotImportContent { Issue = trustedResult.Reason };

                var bytes = CabArchiveUtilities.ExtractFile(Archive, TargetFile);
                if (bytes == null)
                    throw new Exception(string.Format("Could not extract {0} from archive", TargetFile));

                File.WriteAllBytes(DestinationFile, bytes);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Extract file from archive";
        }
    }
}
