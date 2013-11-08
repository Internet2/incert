using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class LocationQueryRequest:AbstractLocationQueryContract
    {
        public LocationQueryRequest(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override IResult GetErrorResult()
        {
            return new ExceptionOccurred(GetError());
        }


    }
}
