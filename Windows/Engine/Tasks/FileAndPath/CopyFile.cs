using System;
using System.IO;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.FileAndPath
{
    class CopyFile : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string SourcePath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DestinationPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public CopyFile(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SourcePath))
                    throw new Exception("Cannot copy file. Source path is null or empty.");

                if (!File.Exists(SourcePath))
                    return new FileNotFound { Target = SourcePath };

                if (string.IsNullOrWhiteSpace(DestinationPath))
                    throw new Exception("Cannot copy file. Destination path is null or empty.");

                File.Copy(SourcePath, DestinationPath, true);
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            var fileName = "[unknown]";
            if (!string.IsNullOrWhiteSpace(SourcePath))
                fileName = Path.GetFileName(SourcePath);

            return string.Format("copy file ({0})", fileName);
        }
    }
}
