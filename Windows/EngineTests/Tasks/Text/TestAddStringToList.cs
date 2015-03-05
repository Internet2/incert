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
    public class TestAddStringToList
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
            var task = new AddStringToList(_engine) {Value = "testing", ListKey = "listkey"};

            var result = task.Execute(null);

            Assert.IsInstanceOf<NextResult>(result,"result shoud be next result" );
            
            var list = _engine.SettingsManager.GetTemporaryObject("listkey") as List<string>;
            
            Assert.IsNotNull(list, "list should be set");
            Assert.AreEqual(list.Count, 1, "There should be one item in the list");
            Assert.IsTrue(list.Contains("testing"), "list should contain value 'testing'");

            task.Value = "testing2";
            result = task.Execute(null);

            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            list = _engine.SettingsManager.GetTemporaryObject("listkey") as List<string>;

            Assert.IsNotNull(list, "list should be set");
            Assert.AreEqual(list.Count, 2, "There should be one item in the list");
            Assert.IsTrue(list.Contains("testing"), "list should contain value 'testing'");
            Assert.IsTrue(list.Contains("testing2"), "list should contain value 'testing2'");

            _engine.SettingsManager.SetTemporarySettingString("testing saved text", "testing3");

            task.Value = "[testing saved text]";
            result = task.Execute(null);

            Assert.IsInstanceOf<NextResult>(result, "result shoud be next result");

            list = _engine.SettingsManager.GetTemporaryObject("listkey") as List<string>;

            Assert.IsNotNull(list, "list should be set");
            Assert.AreEqual(list.Count, 3, "There should be one item in the list");
            Assert.IsTrue(list.Contains("testing"), "list should contain value 'testing'");
            Assert.IsTrue(list.Contains("testing2"), "list should contain value 'testing2'");
            Assert.IsTrue(list.Contains("testing3"), "list should contain value 'testing2'");


        }

    }
}
