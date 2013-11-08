using System;
using System.IO;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.FileAndPath
{
    class CreateFolder:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public CreateFolder(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TargetPath))
                    throw new Exception("Cannot create folder; target path is empty or null");

                if (Directory.Exists(TargetPath))
                    return new NextResult();

                Directory.CreateDirectory(TargetPath);
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Create folder";
        }
    }
}
