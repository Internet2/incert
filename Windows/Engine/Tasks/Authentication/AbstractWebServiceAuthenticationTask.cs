using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Authentication
{
    abstract class AbstractWebServiceAuthenticationTask:AbstractTask
    {


        protected AbstractWebServiceAuthenticationTask(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string UsernameKey { get; set; }
        
        [PropertyAllowedFromXml]
        public string PassphraseKey { get; set; }
        
        [PropertyAllowedFromXml]
        public string Credential2Key { get; set; }
        
        [PropertyAllowedFromXml]
        public string Credential3Key { get; set; }
        
        [PropertyAllowedFromXml]
        public string Credential4Key { get; set; }
        
        [PropertyAllowedFromXml]
        public string CertificateProvider
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
    }
}
