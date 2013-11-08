using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class IsWindows7:AbstractCondition
    {
        public IsWindows7(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !SystemUtilities.IsWindows7()
                ? new BooleanReason(false, "This computer's operating system ({0}) is not Windows 7", Environment.OSVersion.VersionString)
                : new BooleanReason(true, "This computer's operating system is Windows 7");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
