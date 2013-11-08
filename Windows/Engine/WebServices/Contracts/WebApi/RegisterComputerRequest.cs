using System.Windows;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.Registration;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class RegisterComputerRequest : AbstractRegistrationContract
    {
        public RegisterComputerRequest(IEndpointManager endpointManager)
            : base(endpointManager)
        {
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
                    Machine = new Machine { MachineId = Application.Current.GetIdentifier() },
                    WiredAddresses = WiredAddresses,
                    WirelessAddresses = WirelessAddresses,
                    Session = Application.Current.GetSessionId().ToString()
                };

            request.AddBody(wrapper);
            return request;
        }

        public override IResult GetErrorResult()
        {
            return new CouldNotRegisterComputer { Issue = GetError().Message };
        }
    }
}
