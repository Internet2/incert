using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors
{
    public abstract class AbstractCommandLineProcessor:AbstractImportable, ICommandLineProcessor
    {
        private static readonly ILog Log = Logger.Create();
        
        private string _key;

        protected AbstractCommandLineProcessor(IEngine engine):base(engine)
        {
            
        }

        protected AbstractCommandLineProcessor(IEngine engine, string key):base(engine)
        {
            SetProcessorKey(key);
        }


        public string GetProcessorKey()
        {
            return _key;
        }

        private void SetProcessorKey(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            if (value.Contains(" "))
            {
                Log.WarnFormat("Key for processor {0} is not valid; keys cannot contain spaces.", value);
                return;
            }
                
            _key = value;
        }

        public abstract void ProcessCommandLine(string value);
        
        public virtual bool IsInitialized()
        {
            return !string.IsNullOrWhiteSpace(_key);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement element)
        {
            var keyValue = XmlUtilities.GetTextFromAttribute(element, "key");
            SetProcessorKey(keyValue);

            base.ConfigureFromNode(element);
        }
    }
}
