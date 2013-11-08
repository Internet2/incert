using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractComputerNameContract:AbstractContract
    {
        public string PreferredName { get; set; }
        
        protected AbstractComputerNameContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.GetComputername; }
        }

        public override IResult GetErrorResult()
        {
            return new ExceptionOccurred(GetError());
        }
    }
}
