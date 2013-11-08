using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.WindowsRegistry
{
    class ValueExists:AbstractRegistryCondition
    {
        public ValueExists(IEngine engine):base(engine)
        {
        }
        
        public override BooleanReason Evaluate()
        {
            try
            {
                using (var key = RegistryRoot.OpenRegistryKey(RegistryKey, false))
                {
                    if (key == null)
                        return new BooleanReason(false, "Could not open {0}\\{1}", RegistryRoot, RegistryKey);


                    var value = key.GetValue(ValueName);
                    return value == null ? new BooleanReason(false, "The registry value {0} does not exist for {1}\\{2}", ValueName, RegistryRoot, RegistryKey) : new BooleanReason(true, "The registry value {0} exists for {1}\\{2}", ValueName, RegistryRoot, RegistryKey);
                }
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while evaluating the condition: {0}", e.Message);
            }
        }
        
      

      
    }
}
