using System.Net;
using System.Windows;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;
using Org.InCommon.InCert.DataContracts;
using DataFormat = RestSharp.DataFormat;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class ReportingRequest:AbstractReportingContract
    {
        public ReportingRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.UploadReport; }
        }


        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) {RequestFormat = DataFormat.Json};
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

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
                return true;

            if (result.StatusCode !=  HttpStatusCode.NoContent)
                return true;

            return false;
        }


    }
}
