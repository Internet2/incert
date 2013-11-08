using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Control
{
    class IsExceptionResult:AbstractCondition
    {
        public string ResultKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public IsExceptionResult(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            var result = SettingsManager.GetTemporaryObject(ResultKey);
            if (result == null)
                return new BooleanReason(false, "No object found for key {0}", ResultKey);

            if (!(result is ExceptionOccurred))
                return new BooleanReason(false, "The result is not an exception result");

            return new BooleanReason(true, "The result is an exception result");
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ResultKey");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ResultKey = XmlUtilities.GetTextFromAttribute(node, "resultKey");
        }
    }
}
