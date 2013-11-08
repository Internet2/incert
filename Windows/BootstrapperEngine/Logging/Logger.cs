using System;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Org.InCommon.InCert.BootstrapperEngine.Logging
{
    public static class Logger
    {
        private static Engine _engine;

        public static void Initialize(Engine engine)
        {
            _engine = engine;
        }
        
        public static void Standard(string message, params object[] parameters)
        {
            Log(LogLevel.Standard, message, parameters);
        }

        public static void Error(string message, params object[] parameters)
        {
            Log(LogLevel.Error, message, parameters);
        }

        public static void Error(Exception e)
        {
            Error("An exception occurred: {0}\n{1}", e.Message, e.StackTrace);
        }

        public static void Debug(string message, params object[] parameters)
        {
            Log(LogLevel.Debug, message, parameters);
        }

        public static void Verbose(string message, params object[] parameters)
        {
            Log(LogLevel.Verbose, message, parameters);
        }

        private static void Log(LogLevel level, string message, params object[] parameters)
        {
            if (_engine == null)
                return;
            
            if (string.IsNullOrWhiteSpace(message))
                return;

            var logMessage = string.Format(message, parameters);
            _engine.Log(level, logMessage);
        }

    }
}
