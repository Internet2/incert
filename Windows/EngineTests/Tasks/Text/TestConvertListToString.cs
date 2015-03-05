using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.Tasks.Text;

namespace EngineTests.Tasks.Text
{
    [TestFixture]
    public class TestConvertListToString
    {

        private IEngine _engine;

        [SetUp]
        public void Initialize()
        {
            var settingsManager = new SettingsManager();
            var tokenResolver = Substitute.For<IStandardTokens>();
            tokenResolver.ResolveTokens(Arg.Any<string>()).Returns(a => a.Arg<string>());
            var valuesResolver = new ValueResolver(settingsManager, tokenResolver);
            _engine = new StandardEngine(settingsManager, null, null, null, null, null, null, null, null, null,valuesResolver);
        }

        [Test]
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

            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            var resultString = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString), "result string should not be empty");
            Assert.IsTrue(resultString.Equals("testing1, testing2, and testing3", StringComparison.InvariantCulture), "result string should be correct");

            _engine.SettingsManager.SetTemporaryObject("listkey", new List<string>{"testing"});

            result = task.Execute(null);

            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            resultString = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString), "result string should not be empty");
            Assert.IsTrue(resultString.Equals("testing", StringComparison.InvariantCulture), "result string should be correct");

            _engine.SettingsManager.SetTemporaryObject("listkey", new List<string> { "testing", "testing2" });

            result = task.Execute(null);

            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            resultString = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultString), "result string should not be empty");
            Assert.IsTrue(resultString.Equals("testing and testing2", StringComparison.InvariantCulture), "result string should be correct");
        }
    }
}
