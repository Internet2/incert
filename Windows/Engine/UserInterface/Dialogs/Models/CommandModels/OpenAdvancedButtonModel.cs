using System.Windows.Input;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.CommandModels
{
    class OpenAdvancedButtonModel:AbstractCommandModel
    {
        private readonly IHasEngineFields _engine;
        private readonly string _group;
        private ICommand _command;


        public OpenAdvancedButtonModel(AbstractDialogModel model, IHasEngineFields engine, string group) : base(model)
        {
            _engine = engine;
            _group = group;
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

        public void ShowAdvancedMenu(AbstractDialogModel model, string group)
        {
            try
            {

                if (model == null)
                    return;

                var left = model.DialogInstance.Left;
                var top = model.DialogInstance.Top;

                model.EnableDisableAllControls(false);
                var advancedMenuModel = new AdvancedMenuModel(_engine,model);
                advancedMenuModel.ShowDialog(
                    left,
                    top,
                    group.Resolve(_engine.SettingsManager, true));

                if (advancedMenuModel.Result != null)
                {
                    if (advancedMenuModel.Result is RestartComputerResult ||
                        advancedMenuModel.Result is SilentRestartComputerResult ||
                        advancedMenuModel.Result is ExitUtilityResult)
                    {
                        model.Result = advancedMenuModel.Result;
                        model.SuppressCloseQuestion = true;
                        model.DialogInstance.Close();
                    }
                }
            }
            finally
            {
                if (model != null)
                    model.EnableDisableAllControls(true);

            }

        }
    }
}
