using System;
using EngineTests.Dynamics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers;

namespace EngineTests.UserInterface.Content
{
    [TestClass]
    public class TempObjectFieldContentTests
    {
       private IEngine _engine;
        
        [TestInitialize]
        public void Initialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null,null);
        }

        [TestMethod]
        public void TestTempObjectFieldContent()
        {
            var mockObject = new MockDynamicObject(_engine)
                {
                    DynamicProperty1 = "test1",
                    DynamicProperty2 = "test2",
                    StaticProperty1 = "test3",
                    StaticProperty2 = "test4"
                };

            _engine.SettingsManager.SetTemporaryObject("test object", mockObject);

            var content = new TempObjectFieldContent(_engine)
                {
                    BaseText = "{DynamicProperty1} {DynamicProperty2} {StaticProperty1} {StaticProperty2}",
                    Key = "test object"
                };

            Assert.IsTrue(content.GetText().Equals("test1 test2 test3 test4", StringComparison.Ordinal));

        }
    }
}
