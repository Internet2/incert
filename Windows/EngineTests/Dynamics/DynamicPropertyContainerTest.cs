using NSubstitute;
using NUnit.Framework;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.Dynamics
{
    [TestFixture]
    public class DynamicPropertyContainerTest
    {
       private IEngine _engine;

        [SetUp]
        public void Initialize()
        {
            var settingsManager = new SettingsManager();
            var tokenResolver = Substitute.For<IStandardTokens>();
            tokenResolver.ResolveTokens(Arg.Any<string>()).Returns(a => a.Arg<string>());
            var valuesResolver = new ValueResolver(settingsManager, tokenResolver);
            
            _engine = new StandardEngine(settingsManager, null, null, null, null, null, null, null, null, null, valuesResolver);
        }
        
        [Test]
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

           
            Assert.That(mockObject.DynamicProperty1, Is.EqualTo("test value"), "after resolving dynamic properties, property value should equal setting value.");
            Assert.That(mockObject.DynamicProperty2, Is.EqualTo("test value.test value 2"), "after resolving dynamic properties, property value should equal setting value.");
            Assert.That(mockObject.StaticProperty1, Is.EqualTo(staticProperty1), "non dynamic properties should always equal initial values");
            Assert.That(mockObject.StaticProperty2, Is.EqualTo(staticProperty2), "non dynamic properties should always equal initial values");

           
        }

    }
}
