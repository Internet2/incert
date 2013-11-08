using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.WebServices
{
    abstract  class AbstractContractConfigTask:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string Contract
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string EndpointUrl
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        protected AbstractContractConfigTask(IEngine engine) : base (engine)
        {
            
        }
    }
}
