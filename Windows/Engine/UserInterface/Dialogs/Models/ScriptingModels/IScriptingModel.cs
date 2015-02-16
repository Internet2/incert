﻿namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels
{
    public interface IScriptingModel
    {
        bool InCertPresent();
        string GetValue(string key);
        string ResolveValue(string value);
        void SetValue(string key, string value);
        void ReturnNext();
        void ReturnBack();
        void ShowAdvancedMenu(string group="");
        void ShowHelpTopic(string value);
        
    }
}
