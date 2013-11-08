using System;
using System.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Wireless
{
    class WirelessAdapterPresent:AbstractCondition
    {
        public WirelessAdapterPresent(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                return !NetworkUtilities.GetWirelessAdapters().Any() 
                    ? new BooleanReason(false, "No wireless adapters are present") 
                    : new BooleanReason(true, "At least one wireless adapter present");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while attempting to determine whether any wireless adapters are present: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
