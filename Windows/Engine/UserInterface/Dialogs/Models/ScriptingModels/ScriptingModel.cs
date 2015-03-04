using System;
using System.Runtime.InteropServices;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors;
using Org.InCommon.InCert.Engine.Results.Errors.General;
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

        public string ResolveValue(string value)
        {
            return _engine.ValueResolver.Resolve(value, true);
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

        public void ReturnClose()
        {
            _dialogModel.Result = new CloseResult();
        }

        public void ReturnError(string errorType)
        {
            var result = ErrorResult.FromTypeName(errorType) 
                ?? new ExceptionOccurred(new Exception(string.Format("Could not result error type {0}", errorType)));

            _dialogModel.Result = result;
        }

        public void ReturnStoredResult(string settingKey)
        {
            _dialogModel.Result = _settingsManager.GetTemporaryObject(settingKey) as AbstractTaskResult 
                ?? new ExceptionOccurred(new Exception(string.Format("No valid result object exists for the key {0}", settingKey)));
        }

        public void ShowAdvancedMenu(string group)
        {
            if (!_dialogModel.DialogInstance.Dispatcher.CheckAccess())
            {
                _dialogModel.DialogInstance.Dispatcher.Invoke(() => ShowAdvancedMenu(group));
                return;
            }

            try
            {
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