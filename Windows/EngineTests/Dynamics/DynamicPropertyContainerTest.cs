using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.Dynamics
{
    [TestClass]
    public class DynamicPropertyContainerTest
    {
       private IEngine _engine;

        [TestInitialize]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null, null);
        }
        
        [TestMethod]
        public void TestDynamicPropertyContainer()
        {
            _engine.SettingsManager.SetTemporarySettingString("test key 1", "test value");
            _engine.SettingsManager.SetTemporarySettingString("test key 2", "test value 2");

            const string staticProperty1 = "static value";
            const string staticProperty2 = "static value";

            var mockObject = new MockDynamicObject(_engine)
                                {
                    DynamicProperty1 = "[test key 1]",
                    DynamicProperty2 = "[test key 1].[test key 2]",
                    StaticProperty1 = staticProperty1,
                    StaticProperty2 = staticProperty2
                };

           
            Assert.IsTrue(mockObject.DynamicProperty1.Equals("test value"), "after resolving dynamic properties, property value should equal setting value.");
            Assert.IsTrue(mockObject.DynamicProperty2.Equals("test value.test value 2"), "after resolving dynamic properties, property value should equal setting value.");
            Assert.IsTrue(mockObject.StaticProperty1.Equals(staticProperty1), "non dynamic properties should always equal initial values");
            Assert.IsTrue(mockObject.StaticProperty2.Equals(staticProperty2), "non dynamic properties should always equal initial values");

           
        }

    }
}
