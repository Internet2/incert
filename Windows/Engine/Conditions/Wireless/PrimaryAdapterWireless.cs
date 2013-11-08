using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Wireless
{
    class PrimaryAdapterWireless:AbstractCondition
    {
        public PrimaryAdapterWireless(IEngine engine) : base (engine)
        {
            
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var adapter = NetworkUtilities.GetPrimaryAdapter(EndpointManager);
                if (adapter == null)
                    return new BooleanReason(false, "could not determine primary adapter");

                return !NetworkUtilities.IsAdapterWireless(adapter) 
                    ? new BooleanReason(false, "{0} is not wireless", adapter.Description) 
                    : new BooleanReason(true, "{0} is wireless", adapter.Description);
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
