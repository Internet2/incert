using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractAuthenticationContract : AbstractContract
    {
        protected AbstractAuthenticationContract(IEndpointManager endpointManager)
            : base(endpointManager)
        {
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.AuthenticateUser; }
        }

        public string LoginId { get; set; }
        public string Credential1 { get; set; }
        public string Credential2 { get; set; }
        public string Credential3 { get; set; }
        public string Credential4 { get; set; }
        public string Provider { get; set; }
    }

}
