using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu
{
    class CloseCommandModel:AbstractCommandModel
    {
        public CloseCommandModel(IAppearanceManager appearanceManager, AdvancedMenuModel model) : base(appearanceManager, model)
        {
            Command = new RelayCommand(param => model.CloseDialog());
            IsCancelButton = true;
            IsDefaultButton = false;
            Text = "Close";
           
        }
    }
}
