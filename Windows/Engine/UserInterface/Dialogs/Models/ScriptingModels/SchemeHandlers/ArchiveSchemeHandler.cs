using System;
using System.IO;
using System.Linq;
using System.Net;
using CefSharp;
using log4net;
using Org.InCommon.InCert.Engine.Logging;
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

        public static bool IsSchemeUrl(string url)
        {
            return !string.IsNullOrWhiteSpace(url)
                && url.StartsWith(SchemeName, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public class ArchiveSchemeHandler : AbstractSchemeHandler
    {
        private static readonly ILog Log = Logger.Create();

        public override bool ProcessRequestAsync(IRequest request, ISchemeHandlerResponse response, OnRequestCompletedHandler requestCompletedCallback)
        {
            try
            {
                var uri = new Uri(request.Url);

                var archivePath = FindArchive(uri.Authority);
                if (string.IsNullOrWhiteSpace(archivePath))
                {
                    Log.WarnFormat("Cannot load content from archive url {0}: archive not found", request.Url);
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    return false;
                }

                var fileName = ResolvePath(CabArchiveUtilities.GetFilesInArchive(archivePath), uri.Segments.Last());
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Log.WarnFormat("Cannot load content from archive url {0}: resource not found in archive ({1})", request.Url, uri.Segments.Last());
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    return false;
                }

                var contentBytes = CabArchiveUtilities.ExtractFile(archivePath, fileName);
                if (contentBytes == null)
                {
                    Log.WarnFormat("Cannot load content from archive url {0}: could not load resource", request.Url);
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    return false;
                }

                var stream = new MemoryStream(contentBytes);
                response.MimeType = GetMimeType(fileName);
                response.ResponseStream = stream;
                response.StatusCode = (int) HttpStatusCode.OK;
                AddAccessControlHeader(response);
                return true;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred trying to load content from archive url: {0}",e);
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return false;
            }
            finally
            {
                requestCompletedCallback();
            }
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
