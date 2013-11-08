using System;
using System.IO;
using System.Windows.Media.Imaging;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class SetApplicationIconFromArchive : AbstractTask
    {
        public SetApplicationIconFromArchive(IEngine engine)
            : base (engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Archive
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Icon
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

                if (string.IsNullOrWhiteSpace(Icon))
                    throw new Exception("The icon parameter cannot be null or empty");
                
                if (!File.Exists(Archive))
                    return new FileNotFound { Target = Archive };

                var trustedResult = CertificateUtilities.IsFileTrusted(Archive);
                if (!trustedResult.Result)
                    return new CouldNotImportContent { Issue = trustedResult.Reason };

                var bytes = CabArchiveUtilities.ExtractFile(Archive, Icon);
                if (bytes == null)
                    throw new Exception(string.Format("Could not extract {0} from archive", Icon));

                using (var stream = new MemoryStream(bytes))
                    AppearanceManager.ApplicationIcon = BitmapFrame.Create(stream);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set application icon from archive ({0}:{1})", Archive, Icon);
        }
    }
}
