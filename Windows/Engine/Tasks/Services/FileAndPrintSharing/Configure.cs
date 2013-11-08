using System;
using System.ServiceProcess;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Services;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Services.FileAndPrintSharing
{
    class Configure : AbstractTask
    {
        public Configure(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public bool Enabled { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var controller = ServiceUtilities.GetServiceInstance("LanmanServer"))
                {
                    if (controller == null)
                    {
                        if (!ServiceUtilities.IsServiceInfoInRegistry("LanmanServer"))
                            return new ServiceInfoNotInRegistry { ServiceName = "LanmanServer"};

                        return new ServiceInstanceNotAvailable { ServiceName = "LanmanServer" };
                    }
                        
                    var result = Enabled ? EnableService(controller) : DisableService(controller);
                    if (!result.Result)
                        throw  new Exception(result.Reason);

                    return new NextResult();
                }
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static BooleanReason EnableService(ServiceController controller)
        {
            var result = ServiceUtilities.EnableService(controller);
            if (!result.Result)
                return result;

            result = ServiceUtilities.SetServiceStartupType(controller,
                ServiceUtilities.ServiceStartupValues.Automatic);
            if (!result.Result)
                return result;

            return new BooleanReason(true, "File and print sharing enabled");
        }

        private static BooleanReason DisableService(ServiceController controller)
        {
            var result = ServiceUtilities.DisableService(controller);
            if (!result.Result)
                return result;

            result = ServiceUtilities.SetServiceStartupType(controller,
                ServiceUtilities.ServiceStartupValues.Disabled);
            if (!result.Result)
                return result;

            return new BooleanReason(true, "File and print sharing disabled");
        }

        public override string GetFriendlyName()
        {
            return string.Format("Configure file and print sharing (enable={0})", Enabled);
        }
    }
}
