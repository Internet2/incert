using System;
using System.Diagnostics;
using System.IO;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    class FileVersionAtLeast : AbstractCondition
    {
        public FileVersionAtLeast(IEngine engine)
            : base (engine)
        {
        }

        public string FilePath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string InfoWrapperKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InfoWrapperKey))
                    return new BooleanReason(false, "no info wrapper key specified");

                var wrapper = SettingsManager.GetTemporaryObject(InfoWrapperKey) as FileInfoWrapper;
                if (wrapper == null)
                    return new BooleanReason(false, "no valid info wrapper present");

                Version version;
                if (!Version.TryParse(wrapper.Version, out version))
                    return new BooleanReason(false, "info wrapper does not contain valid version value");
                
                if (!File.Exists(FilePath))
                    return new BooleanReason(false, "The file {0} does not exist");

                var info = FileVersionInfo.GetVersionInfo(FilePath);
                var fileVersion = new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart,
                                              info.FilePrivatePart);

                return fileVersion < version
                    ? new BooleanReason(false, "File version {0} is less than expected version {1}", fileVersion, version) 
                    : new BooleanReason(true, "File version {0} is greater than or equal to expected version {1}", fileVersion, version);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            if (!IsPropertySet("InfoWrapperKey"))
                return false;

            if (!IsPropertySet("FilePath"))
                return false;

            return true;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            InfoWrapperKey = XmlUtilities.GetTextFromAttribute(node, "key");
            FilePath = XmlUtilities.GetTextFromAttribute(node, "target");
        }
    }
}
