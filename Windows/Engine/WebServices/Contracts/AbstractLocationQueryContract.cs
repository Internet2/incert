using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractLocationQueryContract:AbstractContract
    {
        protected AbstractLocationQueryContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            return new RestRequest(Method.GET);
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.LocationQuery; }
        }
    }
}
