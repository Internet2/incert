using System.Net;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.InCommon
{
    public class AsynchronousLogRequest:AbstractLoggingContract
    {
        public AsynchronousLogRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }



        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Function", "LogMessage");
            request.AddParameter("Message", Message);
            return request;
        }

        public override T MakeRequest<T>()
        {
            var client = GetClient();
            var request = GetRequestObject();

            client.ExecuteAsync(request, ResponseHandler);

            return new T();
        }

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
                return true;

            if (result.StatusCode != HttpStatusCode.NoContent)
                return true;

            return false;
        }
        
        private static void ResponseHandler(IRestResponse restResponse, RestRequestAsyncHandle restRequestAsyncHandle)
        {
            // ignore for now;
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.LogAsync; }
        }

    }

   
}
