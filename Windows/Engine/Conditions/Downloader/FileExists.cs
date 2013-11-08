using System.IO;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;

namespace Org.InCommon.InCert.Engine.Conditions.Downloader
{
    class FileExists : AbstractCondition
    {
        
        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
        }

        public FileExists(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            var info = SettingsManager.GetTemporaryObject(Key) as FileInfoWrapper;
            if (info == null)
                return new BooleanReason(false, "No wrapper exists for the key {0}", Key);

            var target = UriUtilities.GetTargetFromUri(info.FileName);
            target = Path.Combine(PathUtilities.DownloadFolder, target);

            return !System.IO.File.Exists(target) ?
                new BooleanReason(false, "The file {0} does not exist", target) :
                new BooleanReason(true, "The file {0} exists");
        }

        public override bool IsInitialized()
        {
            return (!string.IsNullOrWhiteSpace(Key));
        }
    }
}
