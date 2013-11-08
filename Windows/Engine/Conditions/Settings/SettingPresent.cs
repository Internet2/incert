using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Settings
{
    public class SettingPresent:AbstractCondition
    {

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public SettingPresent(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !SettingsManager.IsTemporarySettingStringPresent(Key) ? 
                new BooleanReason(false, "The setting '" + Key + "' is not set") : 
                new BooleanReason(true, "The setting '" + Key + "' is set");
        }

        public override bool IsInitialized()
        {
            return !string.IsNullOrWhiteSpace(Key);
        }

        public override void ConfigureFromNode(XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
        }
    }
}
