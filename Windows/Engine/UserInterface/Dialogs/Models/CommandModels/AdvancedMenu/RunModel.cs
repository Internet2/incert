using System.Windows.Input;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu
{
    class RunModel:AbstractCommandModel
    {
        public RunModel(IAppearanceManager appearanceManager, AdvancedMenuModel model) : base(appearanceManager, model)
        {
            
            IsCancelButton = true;
            IsDefaultButton = false;
            Text = "Run";
        }
    }
}
