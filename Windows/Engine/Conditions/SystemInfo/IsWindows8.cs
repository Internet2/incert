using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class IsWindows8:AbstractCondition
    {
        public IsWindows8(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return (!SystemUtilities.IsWindows8() && !SystemUtilities.IsWindows81())
                ? new BooleanReason(false, "This computer's operating system ({0}) is not Windows 8", Environment.OSVersion.VersionString)
                : new BooleanReason(true, "This computer's operating system is Windows 8");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
