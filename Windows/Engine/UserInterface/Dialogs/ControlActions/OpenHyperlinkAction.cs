using System.Collections.Generic;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    class OpenHyperlinkAction : AbstractControlAction
    {
        public OpenHyperlinkAction(IEngine engine)
            : base(engine)
        {
            SettingsKeys = new List<string>();
        }

        public string Target { get; set; }

        public List<string> SettingsKeys { get; private set; }

        public override void DoAction(AbstractModel model, bool includeOneTime)
        {
            if (!IsOneTimeOk(includeOneTime))
                return;

            var conditionResult = EvaluateConditions();
            if (!conditionResult.Result)
                return;

            foreach (var key in SettingsKeys)
            {
                SettingsManager.RemoveTemporarySettingString(key);
            }

            UserInterfaceUtilities.OpenBrowser(Target);
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            Target = XmlUtilities.GetTextFromAttribute(node, "target");
            SettingsKeys = XmlUtilities.GetStringCollectionFromChildNodes(node, "SettingKey");
        }
    }
}
