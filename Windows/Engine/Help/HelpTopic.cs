using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Conditions;
using Org.InCommon.InCert.Engine.Conditions.Grouping;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Help
{
    public class HelpTopic : AbstractImportable, IHelpTopic
    {
        private ICondition _rootCondition;

        public HelpTopic(IEngine engine) : base(engine)
        {
        }

        public string Identifier { get; private set; }
        public string Url { get; private set; }
        public bool IsValid
        {
            get
            {
                if (_rootCondition == null)
                    return true;

                return _rootCondition.Evaluate().Result;
            }
        }

        public bool IsInitialized
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Identifier))
                    return false;

                if (string.IsNullOrWhiteSpace(Url))
                    return false;

                return true;
            }
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);

            Identifier = XmlUtilities.GetTextFromAttribute(node, "id");
            Url = XmlUtilities.GetTextFromAttribute(node, "url");

            SetRootCondition(GetInstanceFromNode<AllTrue>(node.Element("Conditions.All")));
            SetRootCondition(GetInstanceFromNode<AnyTrue>(node.Element("Conditions.Any")));
        }

        private void SetRootCondition(ICondition value)
        {
            // only replace value if value !=null
            if (value == null)
                return;

            _rootCondition = value;
        }
    }


}
