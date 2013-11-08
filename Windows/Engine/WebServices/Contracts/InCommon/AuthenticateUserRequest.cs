using System.Net;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.Authentication;
using Org.InCommon.InCert.Engine.WebServices.Exceptions;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.InCommon
{
    class AuthenticateUserRequest:AbstractAuthenticationContract
    {
        public AuthenticateUserRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Function", "TestAuthentication");
            request.AddParameter("LoginId", LoginId);
            request.AddParameter("LoginPwd", Credential1);
            request.AddParameter("LoginCred2", Credential2);
            request.AddParameter("LoginCred3", Credential3);
            request.AddParameter("LoginCred4", Credential4);
            request.AddParameter("CaName", Provider);
            return request;
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

        protected override System.Exception GetExceptionFromRestResponse(IRestResponse response)
        {
            var result = base.GetExceptionFromRestResponse(response);
            return result as WebServiceException == null ? result : 
                new AuthenticateUserRequestException(result);
        }

        public override IResult GetErrorResult()
        {
            return new CouldNotAuthenticateUser {Issue = GetError().Message};
        }

    }
}
