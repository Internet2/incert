using System.Windows;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu
{
    class HelpModel:AbstractCommandModel
    {
        private readonly IHelpManager _helpManager;

        public HelpModel(IHasEngineFields engine, AdvancedMenuModel model) : base(engine, model)
        {
            _helpManager = engine.HelpManager;

            Visibility = !_helpManager.TopicExists(model.HelpTopic) ? Visibility.Hidden : Visibility.Visible;
            Command = new ClearFocusCommand(
                model.DialogInstance,
                param=>_helpManager.ShowHelpTopic(model.HelpTopic, model.ParentModel));

            Text = engine.AdvancedMenuManager.HelpButtonText;

            ButtonImage = new CommandModels.AbstractCommandModel.CommandButtonImage(engine.SettingsManager,
                engine.AdvancedMenuManager.HelpButtonImageKey,
                engine.AdvancedMenuManager.HelpButtonMouseOverImageKey);
        }

       
    }
}
