using System;
using System.IO;
using System.Windows.Media.Imaging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public class LoadImageFromArchive : AbstractTask
    {
        public LoadImageFromArchive(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Archive
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Image
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ImageKey
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

                if (string.IsNullOrWhiteSpace(Image))
                    throw new Exception("The image parameter cannot be null or empty");

                if (string.IsNullOrWhiteSpace(ImageKey))
                    throw new Exception("The image key parameter cannot be null or empty");

                var trustedResult = CertificateUtilities.IsFileTrusted(Archive);
                if (!trustedResult.Result)
                    return new CouldNotImportContent { Issue = trustedResult.Reason };

                var bytes = CabArchiveUtilities.ExtractFile(Archive, Image);
                if (bytes == null)
                    throw new Exception(string.Format("Could not extract {0} from archive", Image));

                using (var stream = new MemoryStream(bytes))
                {
                    SettingsManager.SetTemporaryObject(
                        ImageKey, 
                        BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad));
                }
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Load image ({0}) from archive ({1}); key = {2}", Image, Archive, ImageKey);
        }
    }
}
