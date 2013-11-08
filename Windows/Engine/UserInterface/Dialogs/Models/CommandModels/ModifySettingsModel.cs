using System.Windows.Input;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class ModifySettingsModel : AbstractCommandModel
    {
        private readonly ISettingsManager _manager;
        private ICommand _command;
        private readonly SettingsLink _wrapper;


        public ModifySettingsModel(AbstractDialogModel model, ISettingsManager manager, SettingsLink wrapper)
            : base(model)
        {
            _manager = manager;
            _wrapper = wrapper;
        }

        public override ICommand Command
        {
            get { return _command ?? (_command = new ButtonSettingsCommand(_manager, this, _wrapper)); }
        }
    }
}
