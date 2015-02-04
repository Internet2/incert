using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using log4net;
using Org.InCommon.InCert.Engine.Logging;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers
{
    class RequestHandler:IRequestHandler
    {
        private static readonly ILog Log = Logger.Create();

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
        {
            return false;
        }

        public bool OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            return true;
        }

        public void OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            return;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest request, IResponse response)
        {
           
            Log.DebugFormat("resource load request: {0}", request.Url);
            return false;
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        public bool OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, IWebPluginInfo info)
        {
            return false;
        }

        public void OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
            
        }
    }
}
