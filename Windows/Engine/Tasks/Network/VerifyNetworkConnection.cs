using System.Net.NetworkInformation;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Network;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Network
{
    class VerifyNetworkConnection: AbstractTask
    {
        public VerifyNetworkConnection(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return new NetworkNoConnection();
            

            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Verify network connection";
        }
    }
}
