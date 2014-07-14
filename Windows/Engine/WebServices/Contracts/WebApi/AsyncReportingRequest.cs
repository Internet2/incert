using System.Net;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;
using DataFormat = RestSharp.DataFormat;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class AsyncReportingRequest:AbstractReportingContract
    {
        public AsyncReportingRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            var wrapper = new ReportingEntry
            {
                Machine = new Machine { MachineId = EndpointManager.GetClientIdentifier() },
                Name = Name,
                Value = Value,
                Session = new Session { SessionGuid = EndpointManager.GetSessionId() }
            };

            request.AddBody(wrapper);
            return request;
        }

        public override T MakeRequest<T>()
        {
            var client = GetClient();
            var request = GetRequestObject();

            client.ExecuteAsync(request, ResponseHandler);

            return new T();
        }

        private static void ResponseHandler(IRestResponse restResponse, RestRequestAsyncHandle restRequestAsyncHandle)
        {
            // ignore for now;
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
            get { return EndPointFunctions.UploadAsyncReport; }
        }

    }
}
