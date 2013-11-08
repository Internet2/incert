using System.Net;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class SynchronousLogRequest:AbstractLoggingContract
    {
        public SynchronousLogRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) {RequestFormat = DataFormat.Json};
            var wrapper = new EventEntry
                {
                    EventType = new EventType {Name = Event},
                    Machine = new Machine { MachineId = Machine },
                    Session = new Session {SessionGuid = Session},
                    User = new User {Username = User},
                    Text = Message
                };
            request.AddBody(wrapper);
            
            return request;
        }

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
                return true;

            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode !=HttpStatusCode.NoContent )
                return true;

            return false;
        }

        public override string GetEndpointUrl()
        {
            return EndpointManager.GetEndpointForFunction(SupportedFunction);
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.LogWait; }
        }
    }
}
