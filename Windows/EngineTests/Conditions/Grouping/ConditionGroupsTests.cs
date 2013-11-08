using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Conditions.Grouping;
using Org.InCommon.InCert.Engine.Conditions.Settings;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.Conditions.Grouping
{
    [TestClass]
    public class ConditionGroupsTests
    {
        private IEngine _engine;

        [TestInitialize]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void TestAnyTrueCondition()
        {
            var condition = new AnyTrue(_engine);

            var result = condition.Evaluate();
            Assert.IsNotNull(result, "The evaluate method should return an instantiated BooleanReason class");

            Assert.IsFalse(result.Result,
                           "If no conditions are present, the result should be a negative BooleanReason class");

            condition.Children.Add(new SettingPresent(_engine) { Key = "test setting 1" });
            condition.Children.Add(new SettingPresent(_engine) { Key = "test setting 2" });
            condition.Children.Add(new SettingPresent(_engine) { Key = "test setting 3" });

            result = condition.Evaluate();
            Assert.IsNotNull(result, "The evaluate method should return an instantiated BooleanReason class");

            Assert.IsFalse(result.Result,
               "If none of the conditions are met, the result should be a negative BooleanReason class");


            _engine.SettingsManager.SetTemporarySettingString("test setting 1", "testing");

            result = condition.Evaluate();
            Assert.IsNotNull(result, "The evaluate method should return an instantiated BooleanReason class");

            Assert.IsTrue(result.Result,
                           "If at least of the conditions is met, the result should be a positive BooleanReason class");
        }

        [TestMethod]
        public void TestAllTrueCondition()
        {
            var collection = new AllTrue(_engine);
            collection.Children.Add(new SettingPresent(_engine) { Key = "test setting 1" });
            collection.Children.Add(new SettingPresent(_engine) { Key = "test setting 2" });
            collection.Children.Add(new SettingPresent(_engine) { Key = "test setting 3" });

            var result = collection.Evaluate();
            Assert.IsNotNull(result, "The evaluate method should return an instantiated BooleanReason class");

            Assert.IsFalse(result.Result,
                           "If none of the conditions are met, the result should be a negative BooleanReason class");

            _engine.SettingsManager.SetTemporarySettingString("test setting 1", "testing");

            result = collection.Evaluate();
            Assert.IsNotNull(result, "The evaluate method should return an instantiated BooleanReason class");

            Assert.IsFalse(result.Result,
                           "If not all of the conditions are met, the result should be a negative BooleanReason class");

            _engine.SettingsManager.SetTemporarySettingString("test setting 2", "testing");
            _engine.SettingsManager.SetTemporarySettingString("test setting 3", "testing");
            result = collection.Evaluate();
            Assert.IsNotNull(result, "The evaluate method should return an instantiated BooleanReason class");

            Assert.IsTrue(result.Result,
                           "If all of the conditions are met, the result should be a positive BooleanReason class");
        }




    }
}
