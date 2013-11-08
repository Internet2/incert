using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Services;
using Org.InCommon.InCert.Engine.Results.Errors.WindowsServices;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Network.Dhcp
{
    class EnableDhcpService : AbstractTask
    {
        public EnableDhcpService(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var controller = ServiceUtilities.GetServiceInstance("Dhcp"))
                {
                    if (controller == null)
                    {
                        if (!ServiceUtilities.IsServiceInfoInRegistry("Dhcp"))
                            return new ServiceInfoNotInRegistry { ServiceName = "Dhcp" };

                        return new ServiceInstanceNotAvailable { ServiceName = "Dhcp" };
                    }
                    
                    if (ServiceUtilities.IsServiceRunning(controller))
                        return new NextResult();

                    var result = ServiceUtilities.SetServiceStartupType(controller,
                        ServiceUtilities.ServiceStartupValues.Automatic);

                    if (!result.Result)
                        return new ServiceIssue { Name = "Dhcp", Issue = result.Reason };

                    result = ServiceUtilities.EnableService(controller);
                    if (!result.Result)
                        return new ServiceIssue { Name = "Dhcp", Issue = result.Reason };

                    return new NextResult();
                }
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Enable Dhcp service";
        }
    }
}
