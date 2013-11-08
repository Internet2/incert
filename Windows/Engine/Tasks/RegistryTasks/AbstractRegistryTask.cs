using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.RegistryTasks
{
    abstract class AbstractRegistryTask : AbstractTask
    {
        protected AbstractRegistryTask(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public RegistryUtilities.RegistryRootValues RegistryRoot { get; set; }

        [PropertyAllowedFromXml]
        public string RegistryKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string RegistryValue
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

       
    }
}
