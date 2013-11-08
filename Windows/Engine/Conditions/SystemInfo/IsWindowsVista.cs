using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class IsWindowsVista:AbstractCondition
    {
        public IsWindowsVista(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !SystemUtilities.IsWindowsVista()
                ? new BooleanReason(false, "This computer's operating system ({0}) is not Windows Vista", Environment.OSVersion.VersionString)
                : new BooleanReason(true, "This computer's operating system is Windows Vista");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
