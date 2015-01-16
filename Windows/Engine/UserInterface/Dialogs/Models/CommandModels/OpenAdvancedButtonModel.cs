using System.Windows.Input;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class OpenAdvancedButtonModel : AbstractCommandModel
    {
        private readonly IHasEngineFields _engine;
        private readonly string _group;
        private ICommand _command;
        private IAdvancedMenuManager _advancedMenuManager;


        public OpenAdvancedButtonModel(AbstractDialogModel model, IHasEngineFields engine, string group)
            : base(model)
        {
            _engine = engine;
            _group = group;
            _advancedMenuManager = engine.AdvancedMenuManager;
        }

        public override ICommand Command
        {
            get
            {
                return _command ?? (
                    _command = new ClearFocusCommand(
                        RootDialogModel.DialogInstance,
                        param => ShowAdvancedMenu(
                            RootDialogModel,
                            _group)));
            }
        }

        private void ShowAdvancedMenu(AbstractDialogModel model, string group)
        {
            try
            {
                model.EnableDisableAllControls(false);

                var result = _advancedMenuManager.ShowAdvancedMenu(_engine, model, group);

                if (!result.IsRestartOrExitResult()) return;

                model.Result = result;
                model.SuppressCloseQuestion = true;
                model.DialogInstance.Close();
            }
            finally
            {
                model.EnableDisableAllControls(true);

            }

        }
    }
}
