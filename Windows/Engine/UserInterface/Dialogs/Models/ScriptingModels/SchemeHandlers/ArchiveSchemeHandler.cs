using System;
using System.IO;
using System.Linq;
using System.Net;
using CefSharp;
using Org.InCommon.InCert.Engine.Utilities;

// adapted from http://thechriskent.com/2014/05/12/use-embedded-resources-in-cefsharp/
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
            
            var archivePath = FindArchive(uri.Authority);
            if (string.IsNullOrWhiteSpace(archivePath))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return false;
            }

            var fileName = ResolvePath(CabArchiveUtilities.GetFilesInArchive(archivePath), uri.Segments.Last());
            if (string.IsNullOrWhiteSpace(fileName))
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
