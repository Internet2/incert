using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CefSharp;
using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers
{
    public abstract  class AbstractSchemeHandler:ISchemeHandler
    {
        public abstract bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback);

        protected static string GetMimeType(string fileName)
        {
            switch (Path.GetExtension(fileName).ToStringOrDefault("").ToLowerInvariant())
            {
                case ".html":
                    return "text/html";
                case ".js":
                    return "text/javascript";
                case ".png":
                    return "image/png";
                case ".css":
                    return "text/css";
                default:
                    return "application/octet-stream";
            }
        }

        protected static string ResolvePath(IEnumerable<string> knownPaths, string value)
        {
            return knownPaths.FirstOrDefault(p => p.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
