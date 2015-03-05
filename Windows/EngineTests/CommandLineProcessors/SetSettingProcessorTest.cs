using NUnit.Framework;
using Org.InCommon.InCert.Engine.CommandLineProcessors.Settings;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.CommandLineProcessors
{
    [TestFixture]
    public class SetSettingProcessorTest
    {
        private IEngine _engine;

        [SetUp]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null,null);
        }
        
        [Test]
        public void TestProcessor()
        {
            var processor = new SetSettingProcessor(_engine) { SettingKey = "testing" };

            processor.ProcessCommandLine("blah");

            Assert.IsTrue("blah".Equals(_engine.SettingsManager.GetTemporarySettingString("testing")));
        }
        
    }
}
