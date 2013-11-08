using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.Verification
{
    class VerifyServerVersion:AbstractTask
    {
        public VerifyServerVersion(IEngine engine) : base(engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var request = EndpointManager.GetContract<AbstractStatusInfoContract>(EndPointFunctions.GetStatusInfo);
                var result = request.MakeRequest<StatusInfo>();

                return result == null ? request.GetErrorResult() : new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            return "Verify server version";
        }
    }
}
