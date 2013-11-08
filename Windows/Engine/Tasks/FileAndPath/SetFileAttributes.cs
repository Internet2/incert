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
    class SetFileAttributes:AbstractTask
    {
        public SetFileAttributes(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public FileAttributes Attributes { get; set; }

        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!File.Exists(TargetPath))
                    return new FileNotFound {Target = TargetPath};
                
                var info = new FileInfo(TargetPath);
                info.Attributes = info.Attributes | Attributes;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set file attributes";
        }
    }
}
