using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers
{
    public class ArchiveSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public static string SchemeName { get { return "archive"; } }

        public ISchemeHandler Create()
        {
            return new ArchiveSchemeHandler();
        }
    }

    public class ArchiveSchemeHandler : AbstractSchemeHandler
    {
        public override bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback)
        {
            var uri = new Uri(request.Url);
            var fileName = uri.Segments.Last();
            var archivePath = FindArchive(uri.Authority);
            if (string.IsNullOrWhiteSpace(archivePath))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return false;
            }

            var contentBytes = CabArchiveUtilities.ExtractFile(archivePath, fileName);
            if (contentBytes == null)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return false;
            }

            var stream = new MemoryStream(contentBytes);
            response.MimeType = GetMimeType(fileName);
            response.ResponseStream = stream;
            response.StatusCode = (int)HttpStatusCode.OK;
            requestCompletedCallback();
            return true;
        }

        private static string FindArchive(string archiveName)
        {
            var locations = new[]
            {
                Path.Combine(PathUtilities.DownloadFolder, archiveName),
                Path.Combine(PathUtilities.ApplicationFolder, archiveName)
            };

            return locations.FirstOrDefault(File.Exists);
        }
    }
}
