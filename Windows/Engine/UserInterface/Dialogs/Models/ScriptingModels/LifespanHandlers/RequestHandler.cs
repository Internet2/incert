using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CefSharp;
using log4net;
using Org.InCommon.InCert.Engine.Logging;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.LifespanHandlers
{
    class RequestHandler : IRequestHandler
    {
        private readonly Dictionary<string, string> _embeddedResourceDictionary;

        private static readonly ILog Log = Logger.Create();

        public RequestHandler()
        {
            _embeddedResourceDictionary = BuildEmbeddedResourceDictionary();
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
            return;
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

        private static Dictionary<string, string> BuildEmbeddedResourceDictionary()
        {
            var dictionary = new Dictionary<string, string>();

            AddPathsToDictonary(dictionary, "Org.InCommon.InCert.Engine.Content.Html.Fonts", "resource://html/fonts/");
            AddPathsToDictonary(dictionary, "Org.InCommon.InCert.Engine.Content.Html.Css", "resource://html/css/");
            AddPathsToDictonary(dictionary, "Org.InCommon.InCert.Engine.Content.Html.Scripts", "resource://html/scripts/");

            return dictionary;
        }

        private static void AddPathsToDictonary(IDictionary<string, string> dictionary, string manifestPrefix, string urlPrefix)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames()
                .Where(p => p.StartsWith(manifestPrefix));

            foreach (var resource in resources.Select(r=>RemoveManifestPrefix(r, manifestPrefix)))
            {
                dictionary[resource.ToLowerInvariant()] = urlPrefix + resource;
            }
        }

        private static string RemoveManifestPrefix(string value, string prefix)
        {
            value = value.Replace(prefix, "");
            return value.TrimStart('.');
        }

        private string GetRedirectUrl(string url)
        {
            var target = Path.GetFileName(url);
            if (string.IsNullOrWhiteSpace(target))
            {
                return string.Empty;
            }
            
            target = target.ToLowerInvariant();
            if (!_embeddedResourceDictionary.ContainsKey(target))
            {
                return string.Empty;
            }

            var redirectUrl = _embeddedResourceDictionary[target];
            return redirectUrl.Equals(url, StringComparison.InvariantCultureIgnoreCase) 
                ? string.Empty 
                : redirectUrl;
        }
    }
}
