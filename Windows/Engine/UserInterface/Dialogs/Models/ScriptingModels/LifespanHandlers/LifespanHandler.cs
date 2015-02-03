using CefSharp;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers
{
    class LifespanHandler:ILifeSpanHandler
    {
        
        
        public bool OnBeforePopup(IWebBrowser browser, string url, ref int x, ref int y, ref int width, ref int height)
        {
            UserInterfaceUtilities.OpenBrowser(url);
            return true;
        }

        public void OnBeforeClose(IWebBrowser browser)
        {
           
        }
    }
}
