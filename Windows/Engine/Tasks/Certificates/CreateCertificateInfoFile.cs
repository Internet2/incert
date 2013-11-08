using System;
using System.IO;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class CreateCertificateInfoFile:AbstractCertificateTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string TargetPath
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public CreateCertificateInfoFile(IEngine engine) : base(engine)
        {
            TargetPath = Path.Combine(PathUtilities.UtilityAppDataFolder, "CertInfo.xml");
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var certificate = GetCertificateFromWrapper();
                if (certificate == null)
                {
                    Log.Warn("Could not retrieve certificate from wrapper. Certificate info file will not be written.");
                    return new NextResult();
                }

                var infoWrapper = new CertificatePropertiesWrapper(certificate);
                infoWrapper.Serialize(TargetPath);
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Export certificate info to file";
        }
    }

    
}
