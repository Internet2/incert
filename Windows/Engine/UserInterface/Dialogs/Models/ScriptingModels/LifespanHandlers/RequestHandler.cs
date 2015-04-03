using System;
using System.Collections.Generic;
using System.IO;
using CefSharp;
using log4net;
using Org.InCommon.InCert.Engine.Logging;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers
{
    internal class RequestHandler : IRequestHandler
    {
        private static readonly ILog Log = Logger.Create();
        private readonly Dictionary<string, string> _redirectDictionary;

        public RequestHandler(Dictionary<string, string> redirectDictionary)
        {
            _redirectDictionary = redirectDictionary;
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
        {
            return false;
        }

        public bool OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            // todo: figure out what correct action is here
            return false;
        }

        public void OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest request, IResponse response)
        {
            var redirectUrl = GetRedirectUrl(request.Url);
            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                Log.DebugFormat("redirecting {0} to {1}", request.Url, redirectUrl);
                response.Redirect(redirectUrl);
                return false;
            }

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
            var test = 0;
        }

        private string GetRedirectUrl(string url)
        {
            var target = Path.GetFileName(url);
            if (string.IsNullOrWhiteSpace(target))
            {
                return string.Empty;
            }

            target = target.ToLowerInvariant();
            if (!_redirectDictionary.ContainsKey(target))
            {
                return string.Empty;
            }

            var redirectUrl = _redirectDictionary[target];
            return redirectUrl.Equals(url, StringComparison.InvariantCultureIgnoreCase)
                ? string.Empty
                : redirectUrl;
        }
    }
}