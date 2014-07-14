using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Services;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Services
{
    class SetServiceStartupValue : AbstractTask
    {
        public SetServiceStartupValue(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public ServiceUtilities.ServiceStartupValues StartupValue { get; set; }

        [PropertyAllowedFromXml]
        public string ServiceName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var instance = ServiceUtilities.GetServiceInstance(ServiceName))
                {
                    if (instance == null)
                    {
                        if (!ServiceUtilities.IsServiceInfoInRegistry(ServiceName))
                            return new ServiceInfoNotInRegistry { ServiceName = ServiceName };

                        return new ServiceInstanceNotAvailable { ServiceName = ServiceName };
                    }
                    
                    var result = ServiceUtilities.SetServiceStartupType(instance, StartupValue);
                    if (!result.Result)
                        throw new Exception(result.Reason);
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
            return string.Format("Set {0} service startup value to {1}", ServiceName, StartupValue);
        }
    }
}
