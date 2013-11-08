using System;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Downloader;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Downloader
{
    class VerifyDownloadedFile : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public VerifyDownloadedFile(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw new Exception("No settings key specified");

                var info = SettingsManager.GetTemporaryObject(SettingKey) as FileInfoWrapper;
                if (info == null)
                    throw new Exception("Data object is invalid");

                var result = info.VerifyFile(PathUtilities.DownloadFolder);
                if (!result.Result)
                {
                    Log.Warn(result.Reason);
                    return new CouldNotVerifyFile { Target = info.FileName, Issue = result.Reason };
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
            return "Verify downloaded file";
        }
    }
}
