using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.WebServices
{
    class ConfigureUserCampusContract:AbstractContractConfigTask
    {
        public ConfigureUserCampusContract(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(EndpointUrl))
                    EndpointManager.SetEndpointForFunction(EndPointFunctions.GetUserCampus, EndpointUrl);

                if (!string.IsNullOrWhiteSpace(Contract))
                    EndpointManager.SetContractForFunction(EndPointFunctions.GetUserCampus, Contract);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Configure user-campus web-service contract";
        }
    }
}
