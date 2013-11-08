using System.Net;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class AsynchronousLogRequest:AbstractLoggingContract
    {
        public AsynchronousLogRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            var wrapper = new EventEntry
            {
                EventType = new EventType { Name = Event },
                Machine = new Machine { MachineId = Machine },
                Session = new Session { SessionGuid = Session },
                User = new User { Username = User },
                Text = Message
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

            if (result.StatusCode != HttpStatusCode.OK)
                return true;

            return false;
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.LogAsync; }
        }
    }
}
