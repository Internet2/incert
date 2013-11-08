using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.Authentication;
using Org.InCommon.InCert.Engine.WebServices.Exceptions;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class IdentityQueryRequest : AbstractIdentityQueryContract
    {
        public IdentityQueryRequest(IEndpointManager endpointManager)
            : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            var wrapper = new AuthenticatedIdentityQuery
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
                    IdentityQuery = new IdentityQuery
                        {
                            GroupPaths = GroupPaths,
                            Properties = Properties
                        }
                };
            request.AddBody(wrapper);
            return request;
        }

        protected override Exception GetExceptionFromRestResponse(IRestResponse response)
        {
            var result = base.GetExceptionFromRestResponse(response);
            return result as WebServiceException == null ? result :
                new AuthenticateUserRequestException(result);
        }

        public override IResult GetErrorResult()
        {
            return new CouldNotAuthenticateUser { Issue = GetError().Message };
        }
    }
}
