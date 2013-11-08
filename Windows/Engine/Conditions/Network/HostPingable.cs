using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Network
{
    class HostPingable:AbstractCondition
    {

        private string Host
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        private int _timeout;
        
        public HostPingable(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                return NetworkUtilities.HostPingable(Host, _timeout);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while attempting to ping {0}: {1}", Host, e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return !IsPropertySet("Host");
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            Host = XmlUtilities.GetTextFromAttribute(node, "host");

            _timeout = XmlUtilities.GetIntegerFromAttribute(node, "timeout", 10000);
        }
    }
}
