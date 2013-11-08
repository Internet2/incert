using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ImportContentFromSignedArchive : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ImportContentFromSignedArchive(IEngine engine ) :
            base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TargetPath))
                    return new ExceptionOccurred(new InvalidOperationException("The archive parameter cannot be null or empty"));
                
                if (!File.Exists(TargetPath))
                    return new FileNotFound { Target = TargetPath };

                var trustedResult = CertificateUtilities.IsFileTrusted(TargetPath);
                if (!trustedResult.Result)
                    return new CouldNotImportContent { Issue = trustedResult.Reason };

                var entries = CabArchiveUtilities.GetFilesInArchive(TargetPath);
                if (!entries.Any())
                {
                    Log.WarnFormat("The archive {0} contains no valid files", TargetPath);
                    return new NextResult();
                }

                foreach (var entry in entries)
                {
                    if (string.IsNullOrWhiteSpace(entry))
                        continue;

                    if (!entry.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var contentBytes = CabArchiveUtilities.ExtractFile(TargetPath, entry);
                    if (contentBytes == null)
                    {
                        Log.WarnFormat("Could not extract file {0} from archive {1}", entry, TargetPath);
                        continue;
                    }

                    if (contentBytes.Length == 0)
                    {
                        Log.WarnFormat("Could not extract file {0} from archive {1}; invalid contents returned", entry, TargetPath);
                        continue;
                    }

                    using (var stream = new MemoryStream(contentBytes))
                    {
                        XElement content = null;
                        try
                        {
                            content = XElement.Load(stream);
                        }
                        catch (Exception e)
                        {
                            Log.WarnFormat("An issue occurred while attempting to load content from the archived file {0}: {1}", entry, e);
                        }

                        if (content == null)
                            continue;

                        BranchManager.ImportBranchesFromXml(content);
                        ErrorManager.ImportEntriesFromXml(content);
                        BannerManager.ImportBannersFromXml(content);
                        CommandLineManager.ImportProcessorsFromXml(content);
                        HelpManager.ImportTopicsFromXml(content);
                        AdvancedMenuManager.ImportItemsFromXml(content);
                    }
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Import content from signed archive";
        }
    }
}
