using System.Windows.Input;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class DisabledCommandModel : AbstractCommandModel
    {
        public DisabledCommandModel(AbstractDialogModel model)
            : base(model)
        {
        }

        public override bool Enabled
        {
            get { return false; }
            set { }
        }

        public override ICommand Command
        {
            get { return null; }
        }
    }
}
