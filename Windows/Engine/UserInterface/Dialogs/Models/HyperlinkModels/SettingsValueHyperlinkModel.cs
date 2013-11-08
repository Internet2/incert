using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels
{
    class SettingsValueHyperlinkModel : AbstractHyperlinkModel
    {
        private readonly ISettingsManager _settingsManager;
        
        public SettingsValueHyperlinkModel(AbstractModel parentModel,ISettingsManager settingsManager)
            : base(parentModel)
        {
            _settingsManager = settingsManager;
            
        }

        public override void LoadContent(AbstractLink wrapper)
        {
            _settingsManager.RemoveTemporarySettingString(wrapper.Target);
            Command = new ButtonSettingsCommand(_settingsManager, Parent, wrapper as SettingsLink);

            base.LoadContent(wrapper);
        }
    }
}
