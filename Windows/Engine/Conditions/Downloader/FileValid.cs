using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Downloader
{
    class FileValid : AbstractCondition
    {
        private static readonly ILog Log = Logger.Create();

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public FileValid(IEngine engine):base(engine)
        {
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var info = SettingsManager.GetTemporaryObject(Key) as FileInfoWrapper;
                if (info == null)
                    return new BooleanReason(false, "No wrapper exists for the key {0}", Key);

                return info.VerifyFile(PathUtilities.DownloadFolder);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new BooleanReason(false, "an issue occurred while attempting to validate the file: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return (!string.IsNullOrWhiteSpace(Key));
        }
    }
}
