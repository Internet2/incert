using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.UserInterface
{
    class ValidColorString : AbstractCondition
    {
        public string Value
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ValidColorString(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return new BooleanReason(false, "The color value is empty");

            var result = AppearanceManager.GetBrushForColor(Value, null);
            return result == null ? 
                new BooleanReason(false, "the value {0} does not convert to a valid color", Value) : 
                new BooleanReason(true, "the value {0} translates to a valid color", Value);
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("Value");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Value = XmlUtilities.GetTextFromAttribute(node, "value");
        }
    }
}
