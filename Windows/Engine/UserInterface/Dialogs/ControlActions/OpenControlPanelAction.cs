using System.Collections.Generic;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    class OpenControlPanelAction : AbstractControlAction
    {
        public OpenControlPanelAction(IEngine engine)
            : base(engine)
        {
            SettingsKeys = new List<string>();
        }

        public SystemUtilities.ControlPanelNames Target { get; set; }

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

            SystemUtilities.OpenControlPanel(Target);
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            Target = XmlUtilities.GetEnumValueFromAttribute(node, "target", SystemUtilities.ControlPanelNames.None);
            SettingsKeys = XmlUtilities.GetStringCollectionFromChildNodes(node, "SettingKey");
        }
    }
}
