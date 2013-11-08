using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.Tasks.Text;

namespace EngineTests.Tasks.Text
{
    [TestClass]
    public class TestConvertListToString
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
            var task = new ConvertListToString(_engine) {ListKey = "listkey", SettingKey = "resultkey"};

            var list = new List<string>
                {
                    "testing1",
                    "testing2",
                    "testing3"
                };

            _engine.SettingsManager.SetTemporaryObject("listkey", list);

            var result = task.Execute(null);

            Assert.IsInstanceOfType(result, typeof(NextResult), "result shoud be next result");

            var resultString = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString), "result string should not be empty");
            Assert.IsTrue(resultString.Equals("testing1, testing2, and testing3", StringComparison.InvariantCulture), "result string should be correct");

            _engine.SettingsManager.SetTemporaryObject("listkey", new List<string>{"testing"});

            result = task.Execute(null);

            Assert.IsInstanceOfType(result, typeof(NextResult), "result shoud be next result");

            resultString = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString), "result string should not be empty");
            Assert.IsTrue(resultString.Equals("testing", StringComparison.InvariantCulture), "result string should be correct");

            _engine.SettingsManager.SetTemporaryObject("listkey", new List<string> { "testing", "testing2" });

            result = task.Execute(null);

            Assert.IsInstanceOfType(result, typeof(NextResult), "result shoud be next result");

            resultString = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString), "result string should not be empty");
            Assert.IsTrue(resultString.Equals("testing and testing2", StringComparison.InvariantCulture), "result string should be correct");
        }
    }
}
