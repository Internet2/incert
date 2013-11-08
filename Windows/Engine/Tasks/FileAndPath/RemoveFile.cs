using System;
using System.IO;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.FileAndPath
{
    class RemoveFile:AbstractTask
    {
        public RemoveFile(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!File.Exists(TargetPath))
                    return new NextResult();

                File.Delete(TargetPath);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Remove file";
        }
    }
}
