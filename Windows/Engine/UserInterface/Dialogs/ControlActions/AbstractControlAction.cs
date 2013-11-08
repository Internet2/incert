using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Conditions;
using Org.InCommon.InCert.Engine.Conditions.Grouping;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    public abstract class AbstractControlAction : AbstractImportable, IControlAction
    {
        private ICondition _rootCondition;
        
        public List<string> ControlKeys { get; private set; }
        public bool OneTime { get; set; }

        protected AbstractControlAction(IEngine engine) : base(engine)
        {
            ControlKeys = new List<string>();
        }

        public abstract void DoAction(AbstractModel model, bool includeOneTime);

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);

            ControlKeys = XmlUtilities.GetStringCollectionFromChildNodes(node, "ControlKey");
            OneTime = XmlUtilities.GetBooleanFromAttribute(node, "onetime", false);

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

        public BooleanReason EvaluateConditions()
        {
            return _rootCondition == null ?
                new BooleanReason(true, "") :
                _rootCondition.Evaluate();
        }

        public override bool Initialized()
        {
            return ControlKeys.Any();
        }

        protected bool IsOneTimeOk(bool includeOneTime)
        {
            if (includeOneTime)
                return true;

            if (!OneTime)
                return true;

            return false;
        }

    }
}
