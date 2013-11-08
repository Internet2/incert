using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class ComputerNameRequest:AbstractComputerNameContract
    {
        public ComputerNameRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override string GetEndpointUrl()
        {
            var uri = UriUtilities.ResolveUri(PreferredName, EndpointManager.GetEndpointForFunction(SupportedFunction));
            return uri == null ? "" : uri.AbsoluteUri;
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("name", PreferredName);
            return request;
        }
    }
}
