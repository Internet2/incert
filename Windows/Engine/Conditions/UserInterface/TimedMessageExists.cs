using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.UserInterface
{
    class TimedMessageExists:AbstractCondition
    {
        private string _key;
        
        public TimedMessageExists(IEngine engine):base(engine)
        {
        
        }

        public override BooleanReason Evaluate()
        {
            if (AppearanceManager.IsTimedMessagePresent(_key))
                return new BooleanReason(true, "Timed message present for key {0}", _key);

            return new BooleanReason(false, "No timed message is present for key {0}", _key);
        }

        public override bool IsInitialized()
        {
            return !string.IsNullOrWhiteSpace(_key);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            _key = XmlUtilities.GetTextFromAttribute(node, "key");
        }
    }
}
