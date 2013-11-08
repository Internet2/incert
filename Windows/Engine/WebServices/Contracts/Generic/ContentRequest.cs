using System;
using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Deserializers;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.Generic
{
    class ContentRequest : AbstractUrlContract
    {
        public ContentRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            return new RestRequest(Method.GET);
        }

        public override string GetEndpointUrl()
        {
            var uri = UriUtilities.ResolveUri(Url, EndpointManager.GetEndpointForFunction(SupportedFunction));
            return uri == null ? "" : uri.AbsoluteUri;
        }

        protected override T TryRequest<T>()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                SetError(new Exception("No content url is specified"));
                return null;
            }
            
            return base.TryRequest<T>();
        }
        
        public override Results.IResult GetErrorResult()
        {
            return new CouldNotRetrieveContent {Target = Url, Issue = GetError().Message};
        }

        protected override RestClient GetClient()
        {
            var client = base.GetClient();
            client.AddHandler("text/xml", new ImportableDeserializer());
            client.AddHandler("application/xml", new ImportableDeserializer());
            return client;
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.GetContent; }
        }
        
    }
}
