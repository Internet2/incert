using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractStatusInfoContract:AbstractContract
    {
        protected AbstractStatusInfoContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.GetStatusInfo; }
        }

        public override Results.IResult GetErrorResult()
        {
            return new CouldNotRetrieveStatusInfo { Issue = GetError().Message };
        }
    }
}
