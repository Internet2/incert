using System.Net;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.Registration;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;
using log4net;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class RegistrationQueryStringRequest:AbstractRegistrationContract
    {
        private static readonly ILog Log = Logger.Create();
        
        public RegistrationQueryStringRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override EndPointFunctions SupportedFunction
        {
            get
            {
                return EndPointFunctions.GetRegistrationQueryString;
            }
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) { RequestFormat = RestSharp.DataFormat.Json };
            var wrapper = new RegistrationRequest
            {
                AuthenticationQuery = new BaseAuthenticationQuery
                {
                    UserId = LoginId,
                    Credential1 = Credential1,
                    Credential2 = Credential2,
                    Credential3 = Credential3,
                    Credential4 = Credential4,
                    Provider = Provider
                },
                Machine = new Machine { MachineId = EndpointManager.GetClientIdentifier() },
                WiredAddresses = WiredAddresses,
                WirelessAddresses = WirelessAddresses,
                Session = EndpointManager.GetSessionId().ToString()
            };

            request.AddBody(wrapper);
            return request;
        }

        public override IResult GetErrorResult()
        {
            return new CouldNotRegisterComputer { Issue = GetError().Message };
        }

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
            {
                Log.WarnFormat("issue occurred while attempting to complete web-service call: response status ({0}) is not 'completed'", result.ResponseStatus);
                return true;
            }

            if (result.StatusCode != HttpStatusCode.OK)
            {
                Log.WarnFormat("issue occurred while attempting to complete web-service call: {0}", result.StatusCode);
                return true;
            }

            return false;
        }
    }
}
