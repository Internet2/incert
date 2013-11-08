using Org.InCommon.InCert.Engine.CommandLineProcessors;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.CommandLineProcessors
{
    class MockProcessor:AbstractCommandLineProcessor
    {
        public MockProcessor(IEngine engine, string key) : base(engine,key)
        {
            
        }

        public override void ProcessCommandLine(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            SettingsManager.SetTemporarySettingString("mock commandline processor result",value);
        }
    }
}
