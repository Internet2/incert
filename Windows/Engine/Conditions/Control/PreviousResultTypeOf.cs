using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Control
{
    class PreviousResultTypeOf:AbstractCondition
    {
        private AbstractTaskResult _result;
        
        public string ResultType
        {
            set { _result = ReflectionUtilities.LoadFromAssembly<AbstractTaskResult>(value);}
        }
        
        public PreviousResultTypeOf(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (_result == null)
                    return new BooleanReason(false, "The 'compareTo' result is null");

                if (SettingsManager.PreviousTaskResult == null)
                    return new BooleanReason(false, "The previous result is null");

                if (SettingsManager.PreviousTaskResult.GetType() != _result.GetType())
                return new BooleanReason(false, "The stored result ({0}) is not equal to {1}", SettingsManager.PreviousTaskResult.GetType().Name, _result.GetType().Name);

                return new BooleanReason(true, "The stored result ({0}) is equal to {1}", SettingsManager.PreviousTaskResult.GetType().Name, _result.GetType().Name);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ResultType = XmlUtilities.GetTextFromAttribute(node, "resultType");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
