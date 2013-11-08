using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.CommandLineProcessors.Settings;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.CommandLineProcessors
{
    [TestClass]
    public class SetSettingProcessorTest
    {
        private IEngine _engine;

        [TestInitialize]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null);
        }
        
        [TestMethod]
        public void TestProcessor()
        {
            var processor = new SetSettingProcessor(_engine) { SettingKey = "testing" };

            processor.ProcessCommandLine("blah");

            Assert.IsTrue("blah".Equals(_engine.SettingsManager.GetTemporarySettingString("testing")));
        }
        
    }
}
