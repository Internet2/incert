using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Domain
{
    class IsCurrentUserDomainAccount:AbstractCondition
    {
        public IsCurrentUserDomainAccount(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !SystemUtilities.IsCurrentUserLocalAccount() ?
                new BooleanReason(false, "The current user ({0}) is a domain account.", SystemUtilities.GetCurrentUserUsernameAndDomain()) :
                new BooleanReason(false, "The current user ({0}) is a local account.", SystemUtilities.GetCurrentUserUsernameAndDomain());
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
