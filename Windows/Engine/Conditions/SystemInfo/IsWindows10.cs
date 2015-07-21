using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    public class IsWindows10:AbstractCondition
    {
        public IsWindows10(IEngine engine) : base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return SystemUtilities.IsWindows10()
                ? new BooleanReason(true, "This computer's operating system is Windows 10")
                : new BooleanReason(false, "This computer's operating system ({0}) is not Windows 10", Environment.OSVersion.VersionString);
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
