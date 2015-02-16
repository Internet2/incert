using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.Tasks.Text;

namespace EngineTests.Tasks.Text
{
    [TestClass]
    public class TestAddStringToList
    {
        
        private IEngine _engine;

        [TestInitialize]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null,null);
        }

        [TestMethod]
        public void TestTask()
        {
            var task = new AddStringToList(_engine) {Value = "testing", ListKey = "listkey"};

            var result = task.Execute(null);

            Assert.IsInstanceOfType(result, typeof(NextResult), "result shoud be next result");

            var list = _engine.SettingsManager.GetTemporaryObject("listkey") as List<string>;
            
            Assert.IsNotNull(list, "list should be set");
            Assert.AreEqual(list.Count, 1, "There should be one item in the list");
            Assert.IsTrue(list.Contains("testing"), "list should contain value 'testing'");

            task.Value = "testing2";
            result = task.Execute(null);

            Assert.IsInstanceOfType(result, typeof(NextResult), "result shoud be next result");

            list = _engine.SettingsManager.GetTemporaryObject("listkey") as List<string>;

            Assert.IsNotNull(list, "list should be set");
            Assert.AreEqual(list.Count, 2, "There should be one item in the list");
            Assert.IsTrue(list.Contains("testing"), "list should contain value 'testing'");
            Assert.IsTrue(list.Contains("testing2"), "list should contain value 'testing2'");

            _engine.SettingsManager.SetTemporarySettingString("testing saved text", "testing3");

            task.Value = "[testing saved text]";
            result = task.Execute(null);

            Assert.IsInstanceOfType(result, typeof(NextResult), "result shoud be next result");

            list = _engine.SettingsManager.GetTemporaryObject("listkey") as List<string>;

            Assert.IsNotNull(list, "list should be set");
            Assert.AreEqual(list.Count, 3, "There should be one item in the list");
            Assert.IsTrue(list.Contains("testing"), "list should contain value 'testing'");
            Assert.IsTrue(list.Contains("testing2"), "list should contain value 'testing2'");
            Assert.IsTrue(list.Contains("testing3"), "list should contain value 'testing2'");


        }

    }
}
