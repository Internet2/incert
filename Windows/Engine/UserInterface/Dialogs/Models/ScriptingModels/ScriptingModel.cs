using System.Runtime.InteropServices;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels
{
    [ComVisible(true)]
    public class ScriptingModel : IScriptingModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IHelpManager _helpManager;
        private readonly IAdvancedMenuManager _advancedMenuManager;
        private readonly IHasEngineFields _engine;
        private readonly AbstractDialogModel _dialogModel;
        
        public ScriptingModel(IHasEngineFields engine, AbstractDialogModel dialogModel)
        {
            _settingsManager = engine.SettingsManager;
            _helpManager = engine.HelpManager;
            _advancedMenuManager = engine.AdvancedMenuManager;
            _engine = engine;
            _dialogModel = dialogModel;
        }
        
        public bool InCertPresent()
        {
            return true;
        }

        public string GetValue(string key)
        {
            return _settingsManager.GetTemporarySettingString(key);
        }

        public void SetValue(string key, string value)
        {
            _settingsManager.SetTemporarySettingString(key, value);
        }

        public void ReturnNext()
        {
            _dialogModel.Result = new NextResult();
        }

        public void ReturnBack()
        {
            _dialogModel.Result = new BackResult();
        }

        public void ShowAdvancedMenu(string group="")
        {
            try
            {
                _dialogModel.EnableDisableAllControls(true);

                var result = _advancedMenuManager.ShowAdvancedMenu(_engine, _dialogModel, group);
                if (!result.IsRestartOrExitResult())
                {
                    return;
                }

                _dialogModel.Result = result;
                _dialogModel.SuppressCloseQuestion = true;
                _dialogModel.DialogInstance.Close();
            }
            finally
            {
                _dialogModel.EnableDisableAllControls(true);
            }
        }

        public void ShowHelpTopic(string value)
        {
            _helpManager.ShowHelpTopic(value, _dialogModel);
        }
    }
}