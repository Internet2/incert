using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class IsWindowsXp : AbstractCondition
    {
        public IsWindowsXp(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !SystemUtilities.IsWindowsXp() 
                ? new BooleanReason(false, "This computer's operating system ({0}) is not Windows Xp", Environment.OSVersion.VersionString) 
                : new BooleanReason(true, "This computer's operating system is Windows XP");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
