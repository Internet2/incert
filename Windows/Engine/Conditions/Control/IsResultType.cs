using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Control
{
    class IsResultType:AbstractCondition
    {
        private AbstractTaskResult _result;
        
        public string ResultKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string ResultType
        {
            set { _result = ReflectionUtilities.LoadFromAssembly<AbstractTaskResult>(value);}
        }
        
        public IsResultType(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            if (_result == null)
                return new BooleanReason(false, "The 'CompareTo' result is null");

            var result = SettingsManager.GetTemporaryObject(ResultKey) as AbstractTaskResult;
            if (result == null)
                return new BooleanReason(false, "The stored result is null");

            if (result.GetType() != _result.GetType())
                return new BooleanReason(false, "The stored result ({0}) is not equal to {1}", result.GetType().Name, _result.GetType().Name);

            return new BooleanReason(true, "The stored result ({0}) is equal to {1}", result.GetType().Name, _result.GetType().Name);
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ResultKey");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ResultKey = XmlUtilities.GetTextFromAttribute(node, "resultKey");
            ResultType = XmlUtilities.GetTextFromAttribute(node, "resultType");
        }
    }
}
