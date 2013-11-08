using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractUserCampusContract:AbstractContract
    {
        public string User { get; set; }
        
        protected AbstractUserCampusContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }
        
        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.GetUserCampus; }
        }

        public override IResult GetErrorResult()
        {
            return new ExceptionOccurred(GetError());
        }
    }
}
