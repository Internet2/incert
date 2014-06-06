using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu
{
    class CloseCommandModel:AbstractCommandModel
    {
        public CloseCommandModel(IHasEngineFields engine, AdvancedMenuModel model) : base(engine, model)
        {
            Command = new RelayCommand(param => model.CloseDialog());
            IsCancelButton = true;
            IsDefaultButton = false;
            Text = engine.AdvancedMenuManager.CloseButtonText;
           
            ButtonImage = new CommandModels.AbstractCommandModel.CommandButtonImage(engine.SettingsManager, 
                engine.AdvancedMenuManager.CloseButtonImageKey, 
                engine.AdvancedMenuManager.CloseButtonMouseOverImageKey);
        }
    }
}
