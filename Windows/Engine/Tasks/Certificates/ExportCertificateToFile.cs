using System;
using System.IO;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class ExportCertificateToFile:AbstractCertificateTask
    {
        [PropertyAllowedFromXml]
        public string Target
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ExportCertificateToFile(IEngine engine):base(engine)
        {
        }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                Target = Path.Combine(PathUtilities.DownloadFolder, Target);
                
                var content =GetCertificateTextFromWrapper();
                File.WriteAllText(Target, content);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Export certificate to pfx file";
        }
    }
}
