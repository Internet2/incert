using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Control
{
    class PreviousResultNotTypeOf:PreviousResultTypeOf
    {
        public PreviousResultNotTypeOf(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
