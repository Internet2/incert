using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Results;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels
{
    public interface IScriptingModel
    {
        bool InCertPresent();

        // settings
        string GetValue(string key);
        string ResolveValue(string value);
        void SetValue(string key, string value);
        bool SettingExists(string key);

        // control results
        void ReturnBackResult();
        void ReturnCloseResult();
        void ReturnExitUtilityResult();
        void ReturnLeaveBranchBackResult();
        void ReturnLeaveBranchNextResult();
        void ReturnNextResult();
        void ReturnRepeatBranchingTaskResult();
        void ReturnRepeatCurrentBranchResult();
        void ReturnRepeatCurrentTaskResult();
        void ReturnRepeatParentBranchResult();
        void ReturnRestartComputerResult();
        void ReturnBranchResult(string branch);

        // other results
        void ReturnErrorResult(string errorType);
        void ReturnStoredResult(string settingKey);

        // advanced menu
        void ShowAdvancedMenu(string group = "");
        string GetAdvancedMenuItems();

        // help 
        void ShowHelpTopic(string topic);
        bool HelpTopicAvailable(string topic);

        // dialog behavior, etc.
        void SuppressCloseQuestion(bool value);
        void DisableCloseButton(bool value);

        void RunTaskBranch(string branchName);
    }
}
