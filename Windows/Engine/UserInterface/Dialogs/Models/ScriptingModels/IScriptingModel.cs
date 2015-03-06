namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels
{
    public interface IScriptingModel
    {
        bool InCertPresent();
        string GetValue(string key);
        string ResolveValue(string value);
        void SetValue(string key, string value);
        
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

        // other results
        void ReturnErrorResult(string errorType);
        void ReturnStoredResult(string settingKey);
        
        void ShowAdvancedMenu(string group="");
        void ShowHelpTopic(string value);
        
    }
}
