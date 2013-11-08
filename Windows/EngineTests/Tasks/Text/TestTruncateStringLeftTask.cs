using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.Tasks.Text;

namespace EngineTests.Tasks.Text
{
    [TestClass]
    public class TestTruncateStringLeftTask
    {


        private IEngine _engine;

        [TestInitialize]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void TestTask()
        {
            var task = new TruncateStringLeft(_engine)
                {
                    Value = "testing string 1",
                    Length = 5,
                    SettingKey = "testing key"
                };

            var result = task.Execute(null);

            Assert.IsInstanceOfType(result, typeof(NextResult), "Result should be next result");

            var settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("testi", StringComparison.InvariantCulture),
                          "result should be equal to 'testi'");

            _engine.SettingsManager.SetTemporarySettingString("testing stored value", "foo bar");
            task.Value = "[testing stored value]";

            result = task.Execute(null);
            Assert.IsInstanceOfType(result, typeof(NextResult), "Result should be next result");

            settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("foo b", StringComparison.InvariantCulture),
                          "result should be equal to 'foo b'");

            task.Value = "12345";
            result = task.Execute(null);
            Assert.IsInstanceOfType(result, typeof(NextResult), "Result should be next result");

            settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("12345", StringComparison.InvariantCulture),
                          "result should be equal to '12345'");

            task.Value = "1234";
            result = task.Execute(null);
            Assert.IsInstanceOfType(result, typeof(NextResult), "Result should be next result");

            settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("1234", StringComparison.InvariantCulture),
                          "result should be equal to '1234'");
        }

    }
}
