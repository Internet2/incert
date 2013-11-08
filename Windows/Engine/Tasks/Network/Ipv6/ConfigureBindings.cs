using System;
using System.Runtime.InteropServices;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.NativeCode.NetConfig;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Network.Ipv6
{
    class ConfigureBindings : AbstractTask
    {
        public ConfigureBindings(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public bool EnableBindings { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            INetCfg netConfig = null;
            INetCfgLock netLock = null;
            try
            {
                netConfig = NetConfigExtensions.GetNetConfigInstance();
                netLock = netConfig.ToInitializedLock();

                var bindingPathEnum = netConfig.ToComponentBindings("ms_tcpip6");
                bindingPathEnum.Reset();

                do
                {
                    var binding = bindingPathEnum.GetNextBindingPath();
                    if (binding == null)
                        break;

                    binding.Enable(EnableBindings);
                } while (true);

                netConfig.Apply();

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (netLock != null)
                    netLock.ReleaseWriteLock();

                if (netConfig != null)
                    Marshal.ReleaseComObject(netConfig);
            }

        }

        public override string GetFriendlyName()
        {
            return string.Format("Configure Ipv6 Bindings (Enabled = {0})", EnableBindings);
        }
    }
}
