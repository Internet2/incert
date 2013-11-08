using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors.Settings
{
    public class SetSettingProcessor : AbstractCommandLineProcessor
    {

        private static readonly ILog Log = Logger.Create();

        public SetSettingProcessor(IEngine engine)
            : base(engine)
        {

        }

        public SetSettingProcessor(IEngine engine, string key)
            : base(engine, key)
        {

        }

        [PropertyAllowedFromXml]
        public string SettingKey { get; set; }

        public override void ProcessCommandLine(string value)
        {
            if (string.IsNullOrWhiteSpace(SettingKey))
            {
                Log.WarnFormat("Issue executing command-line processor {0}: invalid settings key", GetProcessorKey());
                return;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                Log.WarnFormat("Issue executing command-line processor {0}: invalid value", GetProcessorKey());
                return;
            }

            SettingsManager.SetTemporarySettingString(SettingKey, value);
        }
    }
}
