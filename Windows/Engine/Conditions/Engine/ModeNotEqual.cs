using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Engine
{
    class ModeNotEqual:ModeEquals
    {
        public ModeNotEqual(IEngine engine) : base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
