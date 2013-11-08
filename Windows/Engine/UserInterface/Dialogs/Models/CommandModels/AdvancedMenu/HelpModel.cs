using System.Windows;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu
{
    class HelpModel:AbstractCommandModel
    {
        private readonly IHelpManager _helpManager;

        public HelpModel(IAppearanceManager appearanceManager, IHelpManager helpManager, AdvancedMenuModel model) : base(appearanceManager, model)
        {
            _helpManager = helpManager;

            Visibility = !_helpManager.TopicExists(model.HelpTopic) ? Visibility.Hidden : Visibility.Visible;
            Command = new ClearFocusCommand(
                model.DialogInstance,
                param=>_helpManager.ShowHelpTopic(model.HelpTopic, model.ParentModel));

            Text = "Help";
        }

       
    }
}
