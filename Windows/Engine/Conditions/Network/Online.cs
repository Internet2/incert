using System.Net.NetworkInformation;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Network
{
    class Online : AbstractCondition
    {
        public Online(IEngine engine)
            : base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return new BooleanReason(false, "network is not available.");

            return new BooleanReason(true, "Network is available.");
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
