using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Control
{
    class SecondsHaveElapsed:AbstractCondition
    {
        public SecondsHaveElapsed(IEngine engine):base(engine)
        {
        }

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public int Seconds { get; set; }

        public override BooleanReason Evaluate()
        {
            var timestamp = Properties.Settings.Default.GetKeyedTimestamp(Key);

            if (!timestamp.HasValue)
                return new BooleanReason(true, "No timestamp is set for the key {0}",Key);

            var elapsed = DateTime.UtcNow.Subtract(timestamp.Value);
            return elapsed.TotalSeconds > Seconds ?
                new BooleanReason(true, "more than {0} seconds have elapsed since the last timestamp {1}",Seconds, timestamp.Value) : 
                new BooleanReason(false, "less than {0} seconds have elapsed since the last timestamp {1}", Seconds, timestamp.Value);
        }

        public override bool IsInitialized()
        {
            if (string.IsNullOrWhiteSpace(Key))
                return false;

            if (Seconds <= 0)
                return false;

            return true;
        }

        public override void ConfigureFromNode(XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
            Seconds = XmlUtilities.GetIntegerFromAttribute(node, "value", 0);
        }
    }
}
