using System.Windows;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class InvisibleCommandModel:DisabledCommandModel
    {
        public InvisibleCommandModel(AbstractDialogModel model) : base(model)
        {
        }

        public override Visibility Visibility
        {
            get { return Visibility.Collapsed; }
            set {}
        }

        public override ICommand Command
        {
            get { return null; }
        }
    }
}
