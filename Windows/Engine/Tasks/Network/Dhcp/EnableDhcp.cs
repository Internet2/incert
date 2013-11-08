using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Network.Dhcp
{
    class EnableDhcp : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public EnableDhcp(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (NetworkAdapterConfiguration instance in NetworkAdapterConfiguration.GetInstances())
                {
                    if (instance.IsIpEnabledNull)
                        continue;

                    if (!instance.IpEnabled)
                        continue;

                    if (instance.IsDhcpEnabledNull)
                        continue;

                    if (instance.DhcpEnabled)
                        continue;

                    Log.WarnFormat("Dhcp not enabled for adapter {0} ({1}); attempting to enable", instance.Description,
                                   instance.MacAddress);

                    var result = instance.EnableDhcp();
                    Log.InfoFormat(
                        result == 0
                            ? "Dhcp successfull enabled for adapter {0} ({1})"
                            : "Could not enable Dhcp for adapter {0} ({1})", instance.Description,
                        instance.MacAddress);
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
            return "Enable Dhcp";
        }
    }


}
