using System.Windows.Input;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class OpenAdvancedButtonModel:AbstractCommandModel
    {
        private readonly IAdvancedMenuManager _advancedMenuManager;
        private readonly string _group;
        private ICommand _command;


        public OpenAdvancedButtonModel(AbstractDialogModel model, IAdvancedMenuManager advancedMenuManager, string group) : base(model)
        {
            _advancedMenuManager = advancedMenuManager;
            _group = group;
        }

        public override ICommand Command
        {
            get
            {
                return _command ?? (
                    _command = new ClearFocusCommand(
                        RootDialogModel.DialogInstance,
                        param => _advancedMenuManager.ShowAdvancedMenu(
                            RootDialogModel, 
                            _group)));
            }
        }
    }
}
