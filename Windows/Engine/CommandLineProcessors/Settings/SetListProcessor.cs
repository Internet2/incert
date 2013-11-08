using System.Linq;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors.Settings
{
    class SetListProcessor:SetSettingProcessor
    {
        public SetListProcessor(IEngine engine) : base(engine)
        {
        }
        
        public override void ProcessCommandLine(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var values = value.Split(',');
            SettingsManager.SetTemporaryObject(SettingKey, values.ToList());
        }
    }
}
