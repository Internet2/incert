using NSubstitute;
using NUnit.Framework;
using Org.InCommon.InCert.Engine.Conditions.Settings;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.Conditions.Settings
{
    [TestFixture]
    public class SettingsConditionsTests
    {
       private IEngine _engine;

        [SetUp]
        public void Initialize()
        {
            var valuesResolver = Substitute.For<IValueResolver>();
            valuesResolver.Resolve(Arg.Any<string>(), true).Returns(a => a.Arg<string>());

            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null, valuesResolver);
        }
        
        [Test]
        public void SettingEqualsConditionTest()
        {
            _engine.SettingsManager.SetTemporarySettingString("test key", "test value");

            var condition = new SettingEquals(_engine) {Value = "test value", Key = "test key"};
            var result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsTrue(result.Result,
                          "a settings equals condition should return a positive boolean reason when the parameters match");

            condition.Value = "wrong value";
            result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsFalse(result.Result,
                          "a settings equals condition should return a negative boolean reason when the parameters match");
        }

        [Test]
        public void SettingNotEqualTest()
        {
            _engine.SettingsManager.SetTemporarySettingString("test key", "test value");
            var condition = new SettingNotEqual(_engine) { Value = "test value", Key = "test key" };
            var result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsFalse(result.Result,
                          "A settings not equal condition should return a false boolean reason when the parameters match");

            condition.Value = "wrong value";
            result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsTrue(result.Result,
                          "A settings not equal condition should return a positive boolean reason when the parameters match");
        }

        [Test]
        public void SettingPresentText()
        {
            _engine.SettingsManager.SetTemporarySettingString("test key", "test value");
            var condition = new SettingPresent(_engine) {Key = "test key"};
            var result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsTrue(result.Result,
                         "a setting present condition should return a positive boolean reason when the setting value is present");

            _engine.SettingsManager.RemoveTemporarySettingString("test key");

            result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsFalse(result.Result,
                         "a setting present condition should return a negative boolean reason when the setting value is absent");
        }

        [Test]
        public void SettingNotPresentTest()
        {
            _engine.SettingsManager.SetTemporarySettingString("test key", "test value");
            var condition = new SettingNotPresent(_engine) { Key = "test key" };
            var result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsFalse(result.Result,
                         "a setting not present condition should return a negative boolean reason when the setting value is present");

            _engine.SettingsManager.RemoveTemporarySettingString("test key");

            result = condition.Evaluate();
            Assert.IsNotNull(result, "The condition should produce an instantiated boolean reason");
            Assert.IsTrue(result.Result,
                         "a setting not present condition should return a positive boolean reason when the setting value is absent");
        }
    }
}
