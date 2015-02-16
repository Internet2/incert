using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.CommandLineProcessors;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace EngineTests.CommandLineProcessors
{
    [TestClass]
    public class TestManagerAndInstantiationLogic
    {
       private IEngine _engine;
        
        [TestInitialize]
        public void Intialize()
        {
            _engine = new StandardEngine(new SettingsManager(), null, null, null, null, null, null, null, null, null,null);
        }

        [TestMethod]
        public void TestInstantiation()
        {
            var manager = new CommandLineManager();

            const string invalidKey = "testing testing";
            const string validKey = "testingtesting";
            var processor = new MockProcessor(_engine, invalidKey);
           

            Assert.IsFalse(processor.IsInitialized(), "Setting a key with a space in it will cause the processor to report itself as not initialized.");

            manager.AddProcessorEntry(processor);
            Assert.IsNull(manager.GetProcessor(invalidKey), "Processors with invalid keys should not be added to the processor dictionary.");
            
            processor = new MockProcessor(_engine, validKey);
            Assert.IsTrue(processor.IsInitialized(), "processors with basic keys should report themselves as initialized.");
            
            manager.AddProcessorEntry(processor);
            Assert.IsNotNull(manager.GetProcessor(validKey), "Processors with valid keys should be added to the processor dictionary.");
        }

        [TestMethod]
        public void TestBasicLogic()
        {
            var manager = new CommandLineManager();
            
            var processor = new MockProcessor(_engine,"testing");
            
            manager.AddProcessorEntry(processor);
            Assert.IsNotNull(manager.GetProcessor("testing"), "Processors with valid keys should be added to the processor dictionary.");

            const string commandLine = "-testing=foo";
            manager.ProcessCommandLine(commandLine);

            var result = _engine.SettingsManager.GetTemporarySettingString("mock commandline processor result");
            Assert.IsFalse(string.IsNullOrWhiteSpace(result), "settings value 'mock commandline processor result' should not be empty after executing command-line processor.");

            Assert.IsTrue("foo".Equals(result,StringComparison.Ordinal), "after running command-line processor, store value should be 'foo'");

            _engine.SettingsManager.RemoveTemporarySettingString("mock commandline processor result");
            const string brokenCommandLine = "-testing=";
            manager.ProcessCommandLine(brokenCommandLine);

            result = _engine.SettingsManager.GetTemporarySettingString("mock commandline processor result");
            Assert.IsTrue(string.IsNullOrWhiteSpace(result), "settings value 'mock commandline processor result' should be empty when processing a command-line with a missing parameter with a processor that requires a parameter.");

            _engine.SettingsManager.RemoveTemporarySettingString("mock commandline processor result");
            const string brokenCommandLine2 = "-testing";
            manager.ProcessCommandLine(brokenCommandLine2);

            result = _engine.SettingsManager.GetTemporarySettingString("mock commandline processor result");
            Assert.IsTrue(string.IsNullOrWhiteSpace(result), "settings value 'mock commandline processor result' should be empty when processing a command-line with a missing parameter with a processor that requires a parameter.");

            var processor2 = new MockProcessor2(_engine,"testing2");
            manager.AddProcessorEntry(processor2);
            Assert.IsNotNull(manager.GetProcessor("testing"), "Processors with valid keys should be added to the processor dictionary.");

            const string commandLine2 = "-testing2";
            _engine.SettingsManager.RemoveTemporarySettingString("mock command line processor 2 result");
            manager.ProcessCommandLine(commandLine2);
            result = _engine.SettingsManager.GetTemporarySettingString("mock command line processor 2 result");
            Assert.IsTrue(result.Equals("succeeded",StringComparison.Ordinal), "Command-line processors that don't require parameters should succeed when passed command-lines without parameters");

            const string commandLine3 = "-testing2=blahblah";
            _engine.SettingsManager.RemoveTemporarySettingString("mock command line processor 2 result");
            manager.ProcessCommandLine(commandLine3);
            result = _engine.SettingsManager.GetTemporarySettingString("mock command line processor 2 result");
            Assert.IsTrue(result.Equals("succeeded", StringComparison.Ordinal), "Command-line processors that don't require parameters should succeed when passed command-lines with parameters, ignoring the parameters");
        }
        
        [TestMethod]
        public void TestParameterLogic()
        {
            var manager = new CommandLineManager();
           
            var processor = new MockProcessor(_engine, "testing3");
            manager.AddProcessorEntry(processor);

            const string commandLine = "-testing3=\"testing 'testing' testing\"";
            manager.ProcessCommandLine(commandLine);

            var result = _engine.SettingsManager.GetTemporarySettingString("mock commandline processor result");
            Assert.IsFalse(string.IsNullOrWhiteSpace(result), "settings value 'mock command line processor result' should not be empty.");

            Assert.IsTrue(result.Equals("testing testing testing"), "double and single quotes should be removed from result");
        }
    }
}
