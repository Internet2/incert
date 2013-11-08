using System;
using System.IO;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Network.Ipv6
{
    class SetRegistryRestriction: AbstractTask
    {
        
        public enum Restrictions:uint
        {
            NoRestrictions = 0,
            Disabled = 0xffffffff,
            PreferIpv4 = 0x20,
            DisableOnNonTunnelInterfaces = 0x10,
            DisableTunneling = 0x01,
            DisableAllButLoopback = 0x11
        }

        [PropertyAllowedFromXml]
        public Restrictions Restriction { get; set; }

        public SetRegistryRestriction(IEngine engine) : base(engine){
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var path = Path.Combine(new[] {"SYSTEM", "CurrentControlSet", "Services", "Tcpip6", "Parameters"});

                using (var key = Registry.LocalMachine.OpenSubKey(path, true))
                {
                    if (key == null)
                        throw new Exception("Registry key does not exist.");

                    key.SetValue("DisabledComponents", (uint)Restriction);
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
            return string.Format("Set Ipv6 registry restriction ({0})", Restriction);
        }
    }
}
