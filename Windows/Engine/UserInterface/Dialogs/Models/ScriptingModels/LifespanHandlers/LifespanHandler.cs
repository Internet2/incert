using CefSharp;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers
{
    class LifespanHandler:ILifeSpanHandler
    {
        
        private static void LaunchPopUp(IWebBrowser browser, string url)
        {
            if (browser.InvokeIfRequired(() => LaunchPopUp(browser, url)))
            {
                return;
            }

            UserInterfaceUtilities.OpenBrowser(url);
        }

        public bool OnBeforePopup(IWebBrowser browser, string sourceUrl, string targetUrl, ref int x, ref int y, ref int width, ref int height)
        {
            LaunchPopUp(browser, targetUrl);
            return true;
        }

        public void OnBeforeClose(IWebBrowser browser)
        {
           
        }
    }
}
