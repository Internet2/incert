using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Network;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Network
{
    class CheckForNatDevice : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public CheckForNatDevice(IEngine engine)
            : base (engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var request = EndpointManager.GetContract<AbstractStatusInfoContract>(EndPointFunctions.GetStatusInfo);
                request.IgnoreCertificateErrors = true;
                var result = request.MakeRequest<StatusInfo>();
                if (result == null)
                    return request.GetErrorResult();

                // if the address that the server sees is not assigned to any interface,
                // then we're behind a nat device
                foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (var adapterAddress in adapter.GetIPProperties().UnicastAddresses
                        .Where(address => address.Address.AddressFamily == AddressFamily.InterNetwork)
                        .Select(address => address.Address.ToString()).Where(adapterAddress => adapterAddress.Equals(result.ClientIPV4Address, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        Log.InfoFormat("Current ip address ({0}) is assigned to {1}", adapterAddress, adapter.Description);
                        return new NextResult();
                    }
                }
                
                Log.WarnFormat("Nat detected: Ip Address = {0}", result.ClientIPV4Address);
                return new NetworkAddressTranslation();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Check for network address translation";
        }
    }
}
