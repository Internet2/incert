using System;
using System.IO;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Help.SchemeHandlers
{
    class ArchiveSchemeHandler : IHelpSchemeHandler
    {
        private static readonly ILog Log = Logger.Create();

        public Uri LoadDocument(Uri uri)
        {
            try
            {
                if (uri == null)
                    throw new Exception("input uri cannot be null");

                if (string.IsNullOrWhiteSpace(uri.AbsolutePath))
                    throw new Exception("Absolute path is not set for uri");

                var parts = uri.AbsolutePath.Split('/');
                if (parts.Length != 3)
                    throw new Exception(string.Format("uri absolute path ({0}) does not follow expected format", uri.AbsolutePath));

                var archivePath = Path.Combine(PathUtilities.ApplicationFolder, parts[1]);
                if (!File.Exists(archivePath))
                    throw new Exception(string.Format("archive path ({0}) does not exist", archivePath));

                var content = CabArchiveUtilities.ExtractFile(archivePath, parts[2]);
                if (content == null)
                    throw new Exception(string.Format("Could not extract file {0} from archive {1}", parts[2], parts[1]));

                var tempFile = Path.Combine(Path.GetTempPath(), parts[2]);
                File.WriteAllBytes(tempFile, content);

                return new Uri(tempFile);

            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to process an archived webpage: {0}", e.Message);
                return null;
            }

        }
    }
}
