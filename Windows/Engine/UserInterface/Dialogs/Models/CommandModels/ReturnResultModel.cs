using System.Windows.Input;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    internal class ReturnResultModel : AbstractCommandModel
    {
        private readonly AbstractTaskResult _result;
        private ICommand _command;

        public ReturnResultModel(AbstractDialogModel model, AbstractTaskResult result)
            : base(model)
        {
            _result = result;
        }

        public override ICommand Command
        {
            get
            {
                return _command ?? (
                    _command = new ClearFocusCommand(
                        RootDialogModel.DialogInstance,
                        param => RootDialogModel.Result = _result));
            }
        }

       
    }
}
