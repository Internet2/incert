using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class IsAdministrator:AbstractCondition
    {
        public IsAdministrator(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !SystemUtilities.IsRunningInAdminContext() ?
                new BooleanReason(false, "utility not running in admin context") : new BooleanReason(true, "utility is running in admin context");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
