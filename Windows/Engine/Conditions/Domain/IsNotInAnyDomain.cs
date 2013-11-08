using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Domain
{
    class IsNotInAnyDomain : IsInAnyDomain
    {
        public IsNotInAnyDomain(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return base.Evaluate().Invert();
        }
    }
}
