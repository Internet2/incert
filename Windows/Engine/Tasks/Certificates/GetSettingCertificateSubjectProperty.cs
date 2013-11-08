using System;
using System.Security.Cryptography.X509Certificates;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Certificates
{
    class GetSettingCertificateSubjectProperty:AbstractCertificateTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public X509NameType Property { get; set; }

        [PropertyAllowedFromXml]
        public bool ForIssuer { get; set; }

        public GetSettingCertificateSubjectProperty(IEngine engine):base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                {
                    Log.Warn("Cannot retrieve property from stored certificate; destination key is invalid");
                    return new NextResult();
                }

                var certificate = GetCertificateFromWrapper();
                if (certificate == null)
                {
                    Log.Warn("Could not retrieve certificate from settings.");
                    return new NextResult();
                }

                SettingsManager.SetTemporarySettingString(SettingKey, certificate.GetNameInfo(Property, ForIssuer));
                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Get subject property from certificate stored in settings";
        }
    }
}
