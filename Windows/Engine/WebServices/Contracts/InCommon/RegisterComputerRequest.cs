using System.Net;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.Registration;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;
using System.Linq;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.InCommon
{
    class RegisterComputerRequest:AbstractRegistrationContract
    {
        public RegisterComputerRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override T ProcessResults<T>(T result)
        {
            return result ?? new T();
        }

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
                return true;

            if (result.StatusCode != HttpStatusCode.NoContent)
                return true;

            return false;
        }

        public override IResult GetErrorResult()
        {
            return new CouldNotRegisterComputer {Issue = GetError().Message};
        }
        
        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Function", "RegisterComputer");
            request.AddParameter("LoginId", LoginId);
            request.AddParameter("LoginPwd", Credential1);
            request.AddParameter("LoginCred2", Credential2);
            request.AddParameter("LoginCred3", Credential3);
            request.AddParameter("LoginCred4", Credential4);
            request.AddParameter("CaName", Provider);

            foreach (var entry in WirelessAddresses.Where(entry => entry !=null && !string.IsNullOrWhiteSpace(entry.Address)))
            {
                request.AddParameter("Wireless", entry);
            }

            foreach (var entry in WiredAddresses.Where(entry => entry != null && !string.IsNullOrWhiteSpace(entry.Address)))
            {
                request.AddParameter("Wired", entry);
            }

            return request;
        }
    }
}
