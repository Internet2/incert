using Org.InCommon.InCert.Engine.CommandLineProcessors;
using Org.InCommon.InCert.Engine.Engines;

namespace EngineTests.CommandLineProcessors
{
    class MockProcessor2:AbstractCommandLineProcessor
    {
        public MockProcessor2(IEngine engine, string key) : base(engine,key)
        {
        
        }
        
        public override void ProcessCommandLine(string value)
        {
            SettingsManager.SetTemporarySettingString("mock command line processor 2 result", "succeeded");
        }
    }
}
