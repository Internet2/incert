using System;
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
    public class TestTruncateStringLeftTask
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
            var task = new TruncateStringLeft(_engine)
                {
                    Value = "testing string 1",
                    Length = 5,
                    SettingKey = "testing key"
                };

            var result = task.Execute(null);

            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            var settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("testi", StringComparison.InvariantCulture),
                          "result should be equal to 'testi'");

            _engine.SettingsManager.SetTemporarySettingString("testing stored value", "foo bar");
            task.Value = "[testing stored value]";

            result = task.Execute(null);
            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("foo b", StringComparison.InvariantCulture),
                          "result should be equal to 'foo b'");

            task.Value = "12345";
            result = task.Execute(null);
            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("12345", StringComparison.InvariantCulture),
                          "result should be equal to '12345'");

            task.Value = "1234";
            result = task.Execute(null);
            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            settingsText = _engine.SettingsManager.GetTemporarySettingString(task.SettingKey);
            Assert.IsFalse(string.IsNullOrWhiteSpace(settingsText), "Settings text should not be empty");
            Assert.IsTrue(settingsText.Equals("1234", StringComparison.InvariantCulture),
                          "result should be equal to '1234'");
        }

    }
}
