using System;
using NetFwTypeLib;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Firewall
{
    public abstract class AbstractFirewallTask : AbstractTask
    {
        
        [Flags]
        public enum Profiles
        {
            None = 0,
            Domain = 1,
            Private =2,
            Public = 4,
            All = Domain | Private | Public
        }
        
        private static readonly ILog Log = Logger.Create();
        private Profiles _profileList;

        protected AbstractFirewallTask(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public Profiles Profile
        {
            get { return _profileList; } 
            set { _profileList = _profileList | value; }
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
