using System.Windows.Input;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class CancelButtonModel : AbstractCommandModel
    {
        private ICommand _command;

        public CancelButtonModel(AbstractDialogModel model)
            : base(model)
        {
        }

        public override ICommand Command
        {
            get
            {
                return _command ?? (
                    _command = new ClearFocusCommand(
                        RootDialogModel.DialogInstance,
                        param => RootDialogModel.DialogInstance.Close()));
            }
        }
    }
}
