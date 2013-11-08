using System;
using System.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Wireless
{
    class ConnectedToNetwork:AbstractCondition
    {
        public ConnectedToNetwork(IEngine engine):base(engine)
        {
        }

        public string Network
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var instances = NetworkUtilities.GetWirelessAdapters();
                if (!instances.Any())
                    return new BooleanReason(false, "No wireless adapters are present");
                
                foreach (var instance in instances)
                {
                    var connectedNetwork = instance.GetConnectedWirelessNetwork();
                    if (string.IsNullOrWhiteSpace(connectedNetwork))
                        continue;

                    if (Network.Equals(connectedNetwork, StringComparison.InvariantCulture))
                        return new BooleanReason(true, "The interface {0} is currently connected to {1}", instance.Description, connectedNetwork);
                }

                return new BooleanReason(false, "No wireless adapters are currently connected to {0}", Network);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("Network");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Network = XmlUtilities.GetTextFromAttribute(node, "network");
        }
    }
}
