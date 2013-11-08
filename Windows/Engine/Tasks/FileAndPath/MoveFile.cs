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
    class MoveFile : AbstractTask
    {
        public MoveFile(IEngine engine)
            : base(engine)
        {
        }


        [PropertyAllowedFromXml]
        public string TargetFile
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DestinationFile
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!File.Exists(TargetFile))
                    return new FileNotFound {Target = TargetFile};
                
                if (File.Exists(DestinationFile))
                    File.Delete(DestinationFile);

                File.Move(TargetFile, DestinationFile);
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Move file";
        }
    }
}
