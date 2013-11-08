using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Misc
{
    class ValidUrlString:AbstractCondition
    {

        public string Value
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public ValidUrlString(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Value))
                    return new BooleanReason(false, "The url string is empty or null");
                
                return !Uri.IsWellFormedUriString(Value,UriKind.Absolute) ? 
                    new BooleanReason(false, "{0} is not a valid url", Value) :
                    new BooleanReason(true, "the url {0} is valid");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while attempting to validate a url '{0}': {1}", Value, e.Message);
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
