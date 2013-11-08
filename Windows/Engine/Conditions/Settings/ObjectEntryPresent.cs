using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Settings
{
    class ObjectEntryPresent:AbstractCondition
    {
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public ObjectEntryPresent(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            if (SettingsManager.GetTemporaryObject(SettingKey) == null)
                return new BooleanReason(false, "no object exists for the key {0}", SettingKey);

            return new BooleanReason(true, "object found for the key {0}", SettingKey);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            SettingKey = XmlUtilities.GetTextFromAttribute(node, "objectKey");
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("SettingKey");
        }
    }
}
