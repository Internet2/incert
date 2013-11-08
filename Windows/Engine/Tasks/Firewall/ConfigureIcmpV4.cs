using System;
using NetFwTypeLib;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Firewall
{
    class ConfigureIcmpV4:AddRule
    {
        private const int NetFwIpProtocolIcmpV4 = 1;
       
        public ConfigureIcmpV4(IEngine engine):base(engine)
        {
        }


        protected override INetFwRule CreateRule()
        {
            var rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            
            rule.Name = Name;

            if (!string.IsNullOrWhiteSpace(Description))
                rule.Description = Description;

            rule.Protocol = NetFwIpProtocolIcmpV4;
            rule.IcmpTypesAndCodes = "8:*";
            rule.Enabled = Enabled;

            if (!string.IsNullOrWhiteSpace(Grouping))
                rule.Grouping = Grouping;
            
            if (Profile == 0)
                rule.Profiles = (int)Profiles.All;
            else
                rule.Profiles = (int)Profile;

            rule.Action = Action;

            return rule;
        }

        public override string GetFriendlyName()
        {
            return string.Format("Configure incoming echo (enabled = {0}, profile = {1}, action = {2})", Enabled, Profile, Action);
        }
    }
}
