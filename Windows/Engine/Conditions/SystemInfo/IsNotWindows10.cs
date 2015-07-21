using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.Misc;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    public class IsNotWindows10:IsWindows10
    {
        public IsNotWindows10(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
