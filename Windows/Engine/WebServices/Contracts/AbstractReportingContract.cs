using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractReportingContract:AbstractContract
    {
        protected AbstractReportingContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public string Name { get; set; }
        public string Value { get; set; }

       public override IResult GetErrorResult()
        {
            return new CouldNotUploadReport {Issue = GetError().Message};
        }

        public override string GetEndpointUrl()
        {
            return EndpointManager.GetEndpointForFunction(SupportedFunction);
        }

        protected override T ProcessResults<T>(T result)
        {
            return result ?? new T();
        }
    }
}
