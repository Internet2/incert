using System;
using System.Linq;
using System.Runtime.InteropServices;
using NetFwTypeLib;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Firewall
{
    public class AddRule : AbstractFirewallTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public NET_FW_ACTION_ Action { get; set; }

        [PropertyAllowedFromXml]
        public string ApplicationName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Description
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public NET_FW_RULE_DIRECTION_ Direction { get; set; }

        [PropertyAllowedFromXml]
        public bool EdgeTraversal { get; set; }

        [PropertyAllowedFromXml]
        public bool Enabled { get; set; }

        [PropertyAllowedFromXml]
        public string Grouping
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string IcmpTypesAndCodes
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Interfaces
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string InterfaceTypes
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string LocalAddresses
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string LocalPorts
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Name
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public NET_FW_IP_PROTOCOL_ Protocol { get; set; }

        [PropertyAllowedFromXml]
        public string RemoteAddresses
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string RemotePorts
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ServiceName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public AddRule(IEngine engine)
            : base (engine)
        {
            Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
        }

        public override IResult Execute(IResult previousResults)
        {
            INetFwPolicy2 policy = null;
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                    throw new Exception("Cannot set filewall rule; no rule name specified");

                policy = GetFirewallPolicy();
                if (policy == null)
                    throw new Exception("Could not initialize firewall policy object");

                // define the rule
                var rule = CreateRule();

                // first remove the existing rule (so it won't be duplicated if it already exists)
                RemoveExistingRules(policy);

                // now add the new rule
                policy.Rules.Add(rule);
                Log.InfoFormat("Successfully added file rule {0}", Name);
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (policy != null)
                    Marshal.ReleaseComObject(policy);
            }

        }

        private void RemoveExistingRules(INetFwPolicy2 policy)
        {
            var removeList = policy.Rules.Cast<INetFwRule>().Where(rule => rule.Name.Equals(Name, StringComparison.InvariantCulture)).ToList();

            foreach (var rule in removeList)
                policy.Rules.Remove(rule.Name);

        }

        protected virtual INetFwRule CreateRule()
        {
            var rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            rule.Action = Action;

            if (!string.IsNullOrWhiteSpace(ApplicationName))
                rule.ApplicationName = ApplicationName;

            if (!string.IsNullOrWhiteSpace(Description))
                rule.Description = Description;

            rule.Direction = Direction;

            rule.EdgeTraversal = EdgeTraversal;
            rule.Enabled = Enabled;

            if (!string.IsNullOrWhiteSpace(Grouping))
                rule.Grouping = Grouping;

            if (!string.IsNullOrWhiteSpace(IcmpTypesAndCodes))
                rule.IcmpTypesAndCodes = IcmpTypesAndCodes;

            if (!string.IsNullOrWhiteSpace(InterfaceTypes))
                rule.InterfaceTypes = InterfaceTypes;

            if (!string.IsNullOrWhiteSpace(Interfaces))
                rule.Interfaces = Interfaces;

            // needs to be set before addresses and ports
            rule.Protocol = (int)Protocol;

            if (!string.IsNullOrWhiteSpace(LocalAddresses))
                rule.LocalAddresses = LocalAddresses;

            if (!string.IsNullOrWhiteSpace(LocalPorts))
                rule.LocalPorts = LocalPorts;

            rule.Name = Name;

            if (Profile == 0)
                rule.Profiles = (int)Profiles.All;
            else
                rule.Profiles = (int)Profile;
            
            if (!string.IsNullOrWhiteSpace(RemoteAddresses))
                rule.RemoteAddresses = RemoteAddresses;

            if (!string.IsNullOrWhiteSpace(RemotePorts))
                rule.RemotePorts = RemotePorts;

            if (!string.IsNullOrWhiteSpace(ServiceName))
                rule.serviceName = ServiceName;

            return rule;
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set firewall rule {0}", Name);
        }
    }
}
