using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.WebServices
{
    class ConfigureLoggingContracts:AbstractContractConfigTask
    {
        

        public ConfigureLoggingContracts(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string AsynchronousContract
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string SynchronousContract
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(EndpointUrl))
                {
                    EndpointManager.SetEndpointForFunction(EndPointFunctions.LogAsync, EndpointUrl);
                    EndpointManager.SetEndpointForFunction(EndPointFunctions.LogWait, EndpointUrl);
                }

                if (!string.IsNullOrWhiteSpace(AsynchronousContract))
                    EndpointManager.SetContractForFunction(EndPointFunctions.LogAsync, AsynchronousContract);

                if (!string.IsNullOrWhiteSpace(SynchronousContract))
                    EndpointManager.SetContractForFunction(EndPointFunctions.LogWait, SynchronousContract);
                    
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Configure logging contracts";
        }
    }
}
