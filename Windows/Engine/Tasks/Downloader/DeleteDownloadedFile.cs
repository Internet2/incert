using System;
using System.IO;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Downloader
{
    class DeleteDownloadedFile:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public DeleteDownloadedFile(IEngine engine):base(engine)
        {
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    return new CouldNotRetrieveFileInfo { Issue = "No settings key specified" };

                var info = SettingsManager.GetTemporaryObject(SettingKey) as FileInfoWrapper;
                if (info == null)
                    return new CouldNotRetrieveFileInfo { Issue = "Data object is invalid" };

                var target = UriUtilities.GetTargetFromUri(info.FileName);
                target = Path.Combine(PathUtilities.DownloadFolder, target);
                if (!File.Exists(target))
                    return new NextResult();
            

                File.Delete(target);
                return new NextResult();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to delete a file: {0}", e.Message);
                return new NextResult();
            }
        }

        public override string GetFriendlyName()
        {
            return "Delete downloaded file";
        }
    }
}
