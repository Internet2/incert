using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Properties;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels
{
    public interface IScriptingModel
    {
        bool InCertPresent();
        string GetValue(string key);
        void SetValue(string key, string value);
        void ReturnNext();
        void ReturnBack();
        void ShowAdvancedMenu(string group="");
        void ShowHelpTopic(string value);
        
    }
}
