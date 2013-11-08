using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Engine
{
    class ModeEquals : AbstractCondition
    {
        private readonly IEngine _engine;

        public ModeEquals(IEngine engine)
            : base(engine)
        {
            _engine = engine;
        }

        public EngineModes Value {get; set;}

        public override BooleanReason Evaluate()
        {
            try
            {
                if (_engine == null)
                    return new BooleanReason(false, "could not retrieve active engine instance");

                return _engine.Mode != Value 
                    ? new BooleanReason(false, "engine mode ({0}) is not equal to {1}", _engine.Mode, Value) 
                    : new BooleanReason(true, "engine mode ({0}) equals {1}", _engine.Mode, Value);
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("Value");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Value = XmlUtilities.GetEnumValueFromAttribute(node, "value", EngineModes.All);
        }
    }
}
