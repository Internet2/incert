using System;
using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.Generic
{
    class FileInfoUrlRequest : AbstractUrlContract
    {
        public FileInfoUrlRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override T TryRequest<T>()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                SetError(new Exception("No filename is specified"));
                return null;
            }

            return base.TryRequest<T>();
        }

        public override string GetEndpointUrl()
        {
            var uri = UriUtilities.ResolveUri(Url, EndpointManager.GetEndpointForFunction(SupportedFunction));
            return uri == null ? "" : uri.AbsoluteUri;
        }

        public override Results.IResult GetErrorResult()
        {
            return new CouldNotRetrieveFileInfo { Issue = GetError().Message };
        }

        protected override IRestRequest GetRequestObject()
        {
            return new RestRequest(Method.GET);
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.GetFileInfo; }
        }
    }
}
