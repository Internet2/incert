using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractUrlContract:AbstractContract
    {
        protected AbstractUrlContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public string Url { get; set; }

       
    }
}
