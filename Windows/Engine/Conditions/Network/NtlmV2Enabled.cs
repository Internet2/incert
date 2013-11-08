using System.IO;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Network
{
    class NtlmV2Enabled:AbstractCondition
    {
        public NtlmV2Enabled(IEngine engine):base(engine)
        {
        }
        
        public override BooleanReason Evaluate()
        {
            var path = Path.Combine(new[] { "System", "CurrentControlSet", "Control", "Lsa" });
            using (
                var key =
                    Registry.LocalMachine.OpenSubKey(path, true))
            {
                if (key == null)
                    return new BooleanReason(false, "Registry key {0} could not be opened", path);

                var level = key.GetValue("lmcompatibilitylevel").ToIntOrDefault(0);
                return level !=5 
                    ? new BooleanReason(false, "Level ({0}) is not equal to 5", level) 
                    : new BooleanReason(true, "Level equals 5");
            }
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
