using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.Tasks.Firewall;

namespace EngineTests.Tasks.Firewall
{
    [TestClass]
    public class TestAbstractFirewallTask
    {
        private IEngine _engine;
        
        [TestInitialize]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void TestSettingProfileTypes()
        {
            var instance = new AddRule(_engine);

            Assert.AreEqual( instance.Profile, AbstractFirewallTask.Profiles.None, "If no value is set, the profile value should equal zero");

            instance.Profile = AbstractFirewallTask.Profiles.Domain;
            instance.Profile = AbstractFirewallTask.Profiles.Private;

            Assert.AreEqual(
                instance.Profile, AbstractFirewallTask.Profiles.Domain | AbstractFirewallTask.Profiles.Private, 
                "Adding the domain and private type profile values should produce a value that is the result of OR'ing both original values");
        }
    }
}
