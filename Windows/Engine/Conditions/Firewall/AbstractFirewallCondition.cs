using System;
using NetFwTypeLib;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Firewall
{
    abstract class AbstractFirewallCondition:AbstractCondition
    {
        private static readonly ILog Log = Logger.Create();

        protected AbstractFirewallCondition(IEngine engine):base(engine)
        {
        }

        protected static INetFwPolicy2 GetFirewallPolicy()
        {
            try
            {
                var manager = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                return manager as INetFwPolicy2;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to instantiate a Windows firewall policy object: {0}", e);
                return null;
            }
        }

        protected static INetFwMgr GetFirewallManager()
        {
            try
            {
                var manager = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
                return manager as INetFwMgr;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to instantiate a Windows firewall manager: {0}", e);
                return null;
            }
        }
    }
}
