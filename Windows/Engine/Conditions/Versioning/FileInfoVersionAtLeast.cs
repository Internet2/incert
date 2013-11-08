using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    class FileInfoVersionAtLeast:AbstractCondition
    {
         public string CompareVersion
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public FileInfoVersionAtLeast(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var info = SettingsManager.GetTemporaryObject(Key) as FileInfoWrapper;
                if (info == null)
                    return new BooleanReason(false, "No wrapper exists for the key {0}", Key);

                if (string.IsNullOrWhiteSpace(info.Version))
                    return new BooleanReason(false, "No version is set in the file info structure");

                Version version;
                if (!Version.TryParse(info.Version, out version))
                    return new BooleanReason(false, "Could not convert the string '{0}' to a valid version", info.Version);

                Version compareVersion;
                if (!Version.TryParse(CompareVersion, out compareVersion))
                    return new BooleanReason(false, "Cannot cast value '{0}' to version", CompareVersion);

                return version < compareVersion ?
                    new BooleanReason(false, "The stored version ({0}) is less than the target version ({1})", version, compareVersion) :
                    new BooleanReason(true, "The stored version ({0}) is greater than the target version ({1})", version, compareVersion);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while attempting to evaluate condition: {0}", e);
            }
        }

        public override bool IsInitialized()
        {
            if (string.IsNullOrWhiteSpace(Key))
                return false;

            return true;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
            CompareVersion = XmlUtilities.GetTextFromAttribute(node, "version");
        }
    }
}
