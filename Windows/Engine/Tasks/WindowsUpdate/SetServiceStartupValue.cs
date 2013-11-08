using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class SetServiceStartupValue:AbstractTask
    {
        private const string WindowsUpdateServiceName = "wuauserv";
        
        [PropertyAllowedFromXml]
        public ServiceUtilities.ServiceStartupValues StartupValue { get; set; }
        
        public SetServiceStartupValue(IEngine engine) : base(engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var instance = ServiceUtilities.GetServiceInstance(WindowsUpdateServiceName))
                {
                    var result = ServiceUtilities.SetServiceStartupType(instance, StartupValue);
                    if (!result.Result)
                        throw  new Exception(result.Reason);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set Windows update service startup value to {0}", StartupValue);
        }
    }
}
