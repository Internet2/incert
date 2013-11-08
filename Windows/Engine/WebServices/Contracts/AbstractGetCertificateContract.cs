using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractGetCertificateContract:AbstractAuthenticationContract
    {
        public string EncryptCertificate { get; set; }
        
        protected AbstractGetCertificateContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.RegisterComputer; }
        }
    }
}
