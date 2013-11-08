using System;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Settings
{
    class PersistedValuePresent : AbstractCondition
    {
        public PersistedValuePresent(IEngine engine)
            : base (engine)
        {
        }

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var settingsValue = Properties.Settings.Default.GetKeyedProperty(Key);
                if (string.IsNullOrWhiteSpace(settingsValue))
                    return new BooleanReason(false, "No settings value is present for the persisted key {0}", Key);

                return new BooleanReason(true, "Settings value exists for the persisted key {0}", Key);
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("Key");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Key = XmlUtilities.GetTextFromAttribute(node, "persistedKey");
        }
    }
}
