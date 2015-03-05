using System;
using EngineTests.Dynamics;
using NSubstitute;
using NUnit.Framework;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers;

namespace EngineTests.UserInterface.Content
{
    [TestFixture]
    public class TempObjectFieldContentTests
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
