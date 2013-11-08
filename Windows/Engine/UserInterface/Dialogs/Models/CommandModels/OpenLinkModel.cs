using System.Windows.Input;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class OpenLinkModel:AbstractCommandModel
    {
        private readonly AbstractLink _link;

        private ICommand _command;
        
        public OpenLinkModel(AbstractDialogModel model, AbstractLink link) : base(model)
        {
            _link = link;
        }

        public override ICommand Command
        {
            get
            {
                return _command ?? (
                    _command = new ClearFocusCommand(
                        RootDialogModel.DialogInstance,
                        param => UserInterfaceUtilities.OpenBrowser(_link.Target)));
            }
        }
    }
}
