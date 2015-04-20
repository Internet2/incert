using System;
using System.Collections.Generic;
using System.IO;
using CefSharp;
using log4net;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers
{
    internal class RequestHandler : IRequestHandler
    {
        private static readonly ILog Log = Logger.Create();
        private readonly LinkPolicy _linkPolicy;
        private readonly Uri _originalUri;
        private readonly Dictionary<string, string> _redirectDictionary;

        public RequestHandler(string url, Dictionary<string, string> redirectDictionary, LinkPolicy linkPolicy)
        {
            _redirectDictionary = redirectDictionary;
            _linkPolicy = linkPolicy;

            _originalUri = new Uri(url, UriKind.RelativeOrAbsolute);
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect, bool isMainFrame)
        {
            if (browser.InvokeRequired())
            {
                return browser.Invoke(() => OnBeforeBrowse(browser, request, isRedirect, isMainFrame));
            }

            if (!OpenInBrowserInstance(request.Url))
            {
                UserInterfaceUtilities.OpenBrowser(request.Url);
                return true;
            }

            return false;
        }

        public bool OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            // todo: figure out what correct action is here
            return false;
        }

        public void OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            var test = 0;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest request, bool isMainFrame)
        {
            if (browser.InvokeRequired())
            {
                return browser.Invoke(() => OnBeforeResourceLoad(browser, request, isMainFrame));
            }

            var redirectUrl = GetRedirectUrl(request.Url);
            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                Log.DebugFormat("redirecting {0} to {1}", request.Url, redirectUrl);
                request.Url = redirectUrl;
                return false;
            }

            Log.DebugFormat("resource load request: {0}", request.Url);
            return false;
        }
        
        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        public bool OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, WebPluginInfo info)
        {
            return false;
        }
        
        public void OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
            if (browser.InvokeIfRequired(() => OnRenderProcessTerminated(browser, status)))
            {
                return;
            }

            Log.WarnFormat("Render process terminated with status of {0}. Attempting to reload current page.", status);
            browser.Reload();
        }

        private bool OpenInBrowserInstance(string url)
        {
            try
            {
                var uri = new Uri(url);
                if (uri.Equals(_originalUri))
                {
                    return true;
                }

                switch (_linkPolicy)
                {
                    case LinkPolicy.All:
                        return true;
                    case LinkPolicy.None:
                        return false;
                    case LinkPolicy.Internal:
                        return uri.IsInternalScheme();
                    case LinkPolicy.InternalAndSameHost:
                        return uri.IsInternalScheme() || uri.IsSameHost(_originalUri);
                }

                return false;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while evaluating a browse request url ({0}): {1}", url, e);
                return false;
            }
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