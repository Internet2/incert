using System;
using System.Net;
using System.Reflection;
using CefSharp;

// adapted from http://thechriskent.com/2014/05/12/use-embedded-resources-in-cefsharp/
namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers
{
    public class EmbeddedResourceSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public static string SchemeName { get { return "resource"; } }

        public static bool IsSchemeUrl(string url)
        {
            return !string.IsNullOrWhiteSpace(url) 
                && url.StartsWith(SchemeName, StringComparison.InvariantCultureIgnoreCase);
        }

        public ISchemeHandler Create()
        {
            return new EmbeddedResourceSchemeHandler();
        }
    }

    public class EmbeddedResourceSchemeHandler : AbstractSchemeHandler
    {
        private const string AssemblyNameHeader = "Org.Incommon.InCert.Engine.Content.";
        public override bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback)
        {
            try
            {
                var u = new Uri(request.Url);
                var file = u.Authority + u.AbsolutePath;

                var assembly = Assembly.GetExecutingAssembly();
                var resourcePath = ResolvePath(assembly.GetManifestResourceNames(), GetTargetPath(file));
                if (string.IsNullOrWhiteSpace(resourcePath) || assembly.GetManifestResourceInfo(resourcePath) == null)
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return false;
                }

                response.ResponseStream = assembly.GetManifestResourceStream(resourcePath);
                response.MimeType = GetMimeType(file);
                response.StatusCode = (int)HttpStatusCode.OK;
                AddAccessControlHeader(response);
                return true;
            }
            finally
            {
                requestCompletedCallback();
            }
            
        }

        private static string GetTargetPath(string value)
        {
            return AssemblyNameHeader + value.Replace("/", ".");
        }
    }


}
