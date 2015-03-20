using System;
using System.Linq;
using System.Runtime.InteropServices;
using CefSharp.Wpf;
using Newtonsoft.Json;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels
{
    [ComVisible(true)]
    public class ScriptingModel : IScriptingModel
    {
        private const string EventScriptFormat = "if (typeof document.raiseEngineEvent!='undefined'){{document.raiseEngineEvent('{0}',{1});}}";

        private const string TaskStartEventName = "engine_task_start";
        private const string TaskFinishEventName = "engine_task_finish";
        private const string IssueEventName = "issue_occurred";
        private const string AdvancedMenuBranchStartEventName = "engine_advanced_menu_branch_start";
        private const string AdvancedMenuBranchFinishEventName = "engine_advanced_menu_branch_finish";

        private readonly ISettingsManager _settingsManager;
        private readonly IHelpManager _helpManager;
        private readonly IAdvancedMenuManager _advancedMenuManager;
        private readonly IEngine _engine;
        private readonly AbstractDialogModel _dialogModel;
        private readonly ChromiumWebBrowser _browser;

        public ScriptingModel(IEngine engine, AbstractDialogModel dialogModel, ChromiumWebBrowser browser)
        {
            _settingsManager = engine.SettingsManager;
            _helpManager = engine.HelpManager;
            _advancedMenuManager = engine.AdvancedMenuManager;
            _engine = engine;
            _dialogModel = dialogModel;
            _browser = browser;

            SubscribeToEngineEvents(engine as IHasEngineEvents);
        }

        private void SubscribeToEngineEvents(IHasEngineEvents engine)
        {
            if (_engine == null)
            {
                throw new Exception("Could not subscribe to engine events.");
            }

            engine.IssueOccurred += OnIssueOccurred;
            engine.TaskStarted += OnTaskStarted;
            engine.TaskCompleted += OnTaskCompleted;

        }

        private void OnTaskCompleted(object sender, TaskEventData e)
        {
            if (!e.HasContent())
            {
                return;
            }

            RaiseEvent(TaskStartEventName, e);
        }

        private void OnTaskStarted(object sender, TaskEventData e)
        {
            if (!e.HasContent())
            {
                return;
            }

            RaiseEvent(TaskFinishEventName, e);
        }

        private void OnIssueOccurred(object sender, IssueEventData e)
        {
            RaiseEvent(IssueEventName, e);
        }

        private void RaiseEvent(string eventName, AbstractEventData e)
        {
            var script = string.Format(EventScriptFormat, eventName, e.ToJson());
            _browser.EvaluateScriptAsync(script);
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

        public bool SettingExists(string key)
        {
            return _settingsManager.IsTemporarySettingStringPresent(key)
                || _settingsManager.IsTemporaryObjectPresent(key);
        }

        public void ReturnLeaveBranchNextResult()
        {
            _dialogModel.Result = new LeaveBranchNextResult();
        }

        public void ReturnNextResult()
        {
            _dialogModel.Result = new NextResult();
        }

        public void ReturnRepeatBranchingTaskResult()
        {
            _dialogModel.Result = new RepeatBranchingTaskResult();
        }

        public void ReturnRepeatCurrentBranchResult()
        {
            _dialogModel.Result = new RepeatCurrentBranchResult();
        }

        public void ReturnRepeatCurrentTaskResult()
        {
            _dialogModel.Result = new RepeatCurrentTaskResult();
        }

        public void ReturnRepeatParentBranchResult()
        {
            _dialogModel.Result = new RepeatParentBranchResult();
        }

        public void ReturnRestartComputerResult()
        {
            _dialogModel.Result = new RestartComputerResult();
        }

        public void ReturnBackResult()
        {
            _dialogModel.Result = new BackResult();
        }

        public void ReturnCloseResult()
        {
            _dialogModel.Result = new CloseResult();
        }

        public void ReturnExitUtilityResult()
        {
            _dialogModel.Result = new ExitUtilityResult();
        }

        public void ReturnLeaveBranchBackResult()
        {
            _dialogModel.Result = new LeaveBranchBackResult();
        }

        public void ReturnErrorResult(string errorType)
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

        public string GetAdvancedMenuItems()
        {
            if (!_dialogModel.DialogInstance.Dispatcher.CheckAccess())
            {
                return _dialogModel.DialogInstance.Dispatcher.Invoke(() => GetAdvancedMenuItems());
            }

            var items = _engine.AdvancedMenuManager.Items.Values.Where(i => i.Show).Select(i => new AdvancedMenuExportable(i)).ToArray();
            var result = JsonConvert.SerializeObject(items);
            return result;
        }

        public void ShowHelpTopic(string value)
        {
            _helpManager.ShowHelpTopic(value, _dialogModel);
        }

        public bool HelpTopicAvailable(string topic)
        {
            topic = _engine.ValueResolver.Resolve(topic, true);
            return _helpManager.TopicExists(topic);
        }

        public void SuppressCloseQuestion(bool value)
        {
            _dialogModel.SuppressCloseQuestion = value;
        }

        public void DisableCloseButton(bool value)
        {
            _dialogModel.CanClose = !value;
        }

        public void RunTaskBranch(string branchName)
        {
            if (!_dialogModel.DialogInstance.Dispatcher.CheckAccess())
            {
                _dialogModel.DialogInstance.Dispatcher.Invoke(() => RunTaskBranch(branchName));
                return;
            }
            
            var branch = _engine.BranchManager.GetBranch(branchName);
            if (branch == null)
            {
                return;
            }

            RaiseEvent(AdvancedMenuBranchStartEventName, new BranchEventData(branch));
            var result = ExecuteBranch(branch);
            RaiseEvent(AdvancedMenuBranchFinishEventName, new BranchEventData(branch, result));

            if (!result.IsRestartOrExitResult()) return;

            _dialogModel.Result = result;
        }

        private IResult ExecuteBranch(ITaskBranch branch)
        {
            try
            {
                _dialogModel.EnableDisableAllControls(false);
                return branch.Execute(new NextResult()); 
            }
            finally
            {
                _dialogModel.EnableDisableAllControls(true);

            }
        }
    }
}