using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Domain
{
    class IsInAnyDomain:AbstractCondition
    {
        public IsInAnyDomain(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return SystemUtilities.IsComputerInAnyDomain();
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
