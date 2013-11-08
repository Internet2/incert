using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class Is64Bit:AbstractCondition
    {
        public Is64Bit(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            return !Environment.Is64BitOperatingSystem ? 
                new BooleanReason(false, "Computer is not running 64-bit operating system") : 
                new BooleanReason(true, "Computer is running 64-bit operating system");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
