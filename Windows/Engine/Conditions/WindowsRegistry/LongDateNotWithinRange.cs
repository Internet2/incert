using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.WindowsRegistry
{
    class LongDateNotWithinRange:LongDateWithinRange
    {
        public LongDateNotWithinRange(IEngine engine):base(engine)
        {
        }

        public override Results.Misc.BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
