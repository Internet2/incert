using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CefSharp;
using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers
{
    public abstract  class AbstractSchemeHandler:ISchemeHandler
    {
        private static readonly Dictionary<string, string> MimeTypesDictionary = new Dictionary<string, string>
        {
            {".html", "text/html"},
            {".js", "text/js"},
            {".png", "image/png"},
            {".css", "text/css"},
            {".eot","application/vnd.ms-fontobject"},
            {".ttf","application/octet-stream"},
            {".svg","image/svg+xml"},
            {".woff","application/x-woff"},
            {".otf","font/opentype"}
        }; 
        
        public abstract bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback);

        protected static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToStringOrDefault("").ToLowerInvariant();
            return MimeTypesDictionary.ContainsKey(extension)
                ? MimeTypesDictionary[extension]
                : "application/octet-stream";
        }

        protected static string ResolvePath(IEnumerable<string> knownPaths, string value)
        {
            return knownPaths.FirstOrDefault(p => p.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        protected static void AddAccessControlHeader(ISchemeHandlerResponse response)
        {
            if (response.ResponseHeaders == null)
            {
                response.ResponseHeaders = new WebHeaderCollection();
            }

            response.ResponseHeaders.Set("Access-Control-Allow-Origin", "*");
        }
    }
}
