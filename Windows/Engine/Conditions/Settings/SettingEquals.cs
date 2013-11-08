using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Settings
{
    public class SettingEquals:AbstractCondition
    {
        
        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string Value
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public bool IgnoreCase { get; set; }

        public SettingEquals(IEngine engine):base(engine)
        {
        }
        
        public override BooleanReason Evaluate()
        {
            var result = SettingsManager.GetTemporarySettingString(Key);
            if (string.IsNullOrWhiteSpace(result))
                return new BooleanReason(false, "The settings value does not currently exist");
            
            var comparison = StringComparison.InvariantCulture;
            if (IgnoreCase)
                comparison = StringComparison.InvariantCultureIgnoreCase;
            
            if (!result.Equals(Value, comparison))
               return new BooleanReason(false, "The settings value is not equal to " + Value);
            
            return new BooleanReason(true, "The settings value is equal to " + Value);
        }

        public override bool IsInitialized()
        {
            if (!IsPropertySet("Key"))
                return false;

            return !IsPropertySet("Value");
        }

        public override void ConfigureFromNode(XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
            Value = XmlUtilities.GetTextFromAttribute(node, "value");
            IgnoreCase = XmlUtilities.GetBooleanFromAttribute(node, "ignoreCase", false);
        }
    }
}
