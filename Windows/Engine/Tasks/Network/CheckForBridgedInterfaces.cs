using System;
using System.Linq;
using System.Net.NetworkInformation;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Network;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Network
{
    class CheckForBridgedInterfaces:AbstractTask
    {
        public CheckForBridgedInterfaces(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces()
                    .Where(adapter => adapter.Description.ToLowerInvariant().Contains("mac bridge miniport")))
                {
                    return new BridgedAdapterPresent {InterfaceName = adapter.Name};
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Check for bridged interfaces";
        }
    }
}
