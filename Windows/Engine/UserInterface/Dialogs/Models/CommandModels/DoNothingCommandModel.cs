using System.Windows.Input;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class DoNothingCommandModel:AbstractCommandModel
    {
        public DoNothingCommandModel(AbstractDialogModel model) : base(model)
        {
        }

        public override ICommand Command
        {
            get { return null; }
        }
    }
}
