using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    public class LatestVersionInstalled:AbstractMsiVersionCondition
    {
        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public LatestVersionInstalled(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var info = SettingsManager.GetTemporaryObject(ObjectKey) as FileInfoWrapper;
                if (info == null)
                    return new BooleanReason(false, "No wrapper exists for the key {0}", ObjectKey);

                if (string.IsNullOrWhiteSpace(info.MsiUpgradeCode))
                    return new BooleanReason(false, "No upgrade code present in wrapper");

                Version targetVersion;
                if (!Version.TryParse(info.Version, out targetVersion))
                    return new BooleanReason(false, "Could not convert the string '{0}' to a valid version", info.Version);

                var installedVersion = GetInstalledVersionForUpgradeCode(info.MsiUpgradeCode);
                return targetVersion > installedVersion ?
                    new BooleanReason(false, "The installed version ({0}) is less than the target version ({1})", installedVersion,targetVersion) :
                    new BooleanReason(true, "The installed version ({0}) is greater than the target version ({1})", installedVersion, targetVersion);
                
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while attempting to determine whether latest version installed: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ObjectKey");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ObjectKey = XmlUtilities.GetTextFromAttribute(node, "objectKey");
        }
    }
}
