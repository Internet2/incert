using System.Net;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.InCommon
{
    class SynchronousLogRequest : AbstractLoggingContract
    {
        public SynchronousLogRequest(IEndpointManager endpointManager)
            : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Function", "LogMessage");
            request.AddParameter("Message", Message);
            return request;
        }

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
                return true;

            if (result.StatusCode != HttpStatusCode.NoContent)
                return true;

            return false;
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.LogWait; }
        }
    }
}
