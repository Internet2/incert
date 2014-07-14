using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels.AdvancedMenu
{
    class RunModel:AbstractCommandModel
    {
        public RunModel(IHasEngineFields engine, AdvancedMenuModel model) : base(engine, model)
        {
            
            IsCancelButton = true;
            IsDefaultButton = false;
            Text = engine.AdvancedMenuManager.RunButtonText;

            ButtonImage = new CommandModels.AbstractCommandModel.CommandButtonImage(engine.SettingsManager,
                engine.AdvancedMenuManager.RunButtonImageKey,
                engine.AdvancedMenuManager.RunButtonMouseOverImageKey);
        }
    }
}
