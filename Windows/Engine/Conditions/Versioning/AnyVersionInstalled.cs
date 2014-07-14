using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    public class AnyVersionInstalled:AbstractMsiVersionCondition
    {
        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public AnyVersionInstalled(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var info = SettingsManager.GetTemporaryObject(ObjectKey) as FileInfoWrapper;
                if (info == null)
                    return new BooleanReason(false, "No wrapper exists for the key {0}", ObjectKey);

                if (IsProductInstalledByUpgradeCode(info.MsiUpgradeCode))
                    return new BooleanReason(true, "A version is installed for upgrade code {0}", info.MsiUpgradeCode);

                return IsProductInstalled(info.MsiProductCode) ?
                    new BooleanReason(true, "A version is installed for product code {0}", info.MsiProductCode) :
                    new BooleanReason(false, "No versions are installed for either the upgrade code {0} or the product code {1}", info.MsiUpgradeCode, info.MsiProductCode);
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
