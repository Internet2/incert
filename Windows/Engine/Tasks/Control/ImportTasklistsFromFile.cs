using System;
using System.IO;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ImportTasklistsFromFile : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public ImportTasklistsFromFile(IEngine engine)
            : base(engine)
        {
        
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!File.Exists(TargetPath))
                    return new FileNotFound { Target = TargetPath };

                var document = XElement.Load(TargetPath);

                var result = BranchManager.ImportBranchesFromXml(document);
                if (!result)
                    return new CouldNotImportContent { Issue = "Could not import branches from the file " + TargetPath };

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Import tasklist from file";
        }
    }
}
