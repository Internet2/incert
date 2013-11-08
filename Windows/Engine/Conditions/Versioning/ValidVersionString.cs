using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    class ValidVersionString : AbstractCondition
    {
        public string Value
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ValidVersionString(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return new BooleanReason(false, "The value string is empty");

                Version testVersion;
                return !Version.TryParse(Value, out testVersion) ? 
                    new BooleanReason(false, "The version string '{0}' is not valid", Value) : 
                    new BooleanReason(true, "The version string '{0}' is valid",Value);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while attempting to validate the version string '{0}': {1}", Value, e.Message);
            }
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
