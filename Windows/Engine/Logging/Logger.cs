using System.Globalization;
using System.Linq;
using System.Windows;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Org.InCommon.InCert.Engine.Logging
{
    public class Logger
    {
        private const string LogPattern = "%date{HH:mm:ss} %-5p %m%n";

        static Logger()
        {
            ThreadContext.Properties["Identifier"] = Application.Current.GetIdentifier();
            ThreadContext.Properties["UserId"] = "[Unknown]";
            ThreadContext.Properties["Session"] = Application.Current.GetSessionId();
           
            var hiearchy = (Hierarchy)LogManager.GetRepository();

            var patternLayout = new PatternLayout { ConversionPattern = LogPattern };
            patternLayout.ActivateOptions();

            var tracer = new TraceAppender {Layout = patternLayout};
            tracer.ActivateOptions();
            hiearchy.Root.AddAppender(tracer);

            var roller = new RollingFileAppender
                {
                    Layout = patternLayout,
                    AppendToFile = true,
                    RollingStyle = RollingFileAppender.RollingMode.Once,
                    MaxSizeRollBackups = -1,
                    MaximumFileSize = "100KB",
                    StaticLogFileName = false,
                    File = PathUtilities.LogFile,
                    Threshold = Level.Debug,
                    LockingModel = new FileAppender.MinimalLock()
                };

            roller.ActivateOptions();
            roller.RollingStyle = RollingFileAppender.RollingMode.Once;
            roller.LockingModel = new FileAppender.MinimalLock();
            hiearchy.Root.AddAppender(roller);

            hiearchy.Root.Level = Level.Debug;
            hiearchy.Configured = true;
        }

        public static string SessionId
        {
            get
            {
                var hierarchy = (Hierarchy)LogManager.GetRepository();
                var appender = hierarchy.Root.Appenders.OfType<WebServiceAppender>()
                                        .FirstOrDefault();
                if (appender == null)
                    return "[unknown]";

                return appender.SessionId <= 0
                    ? "[unknown]"
                    : appender.SessionId.ToString(CultureInfo.InvariantCulture);
            }   
        }

        public static  string SessionGuid
        {
            get
            {
                var hierarchy = (Hierarchy)LogManager.GetRepository();
                var appender = hierarchy.Root.Appenders.OfType<WebServiceAppender>()
                                        .FirstOrDefault();
                if (appender == null)
                    return "[unknown]";

                return appender.SessionId <= 0
                    ? "[unknown]"
                    : appender.SessionId.ToString(CultureInfo.InvariantCulture);
            }
        }
        

        public static ILog Create()
        {
            return LogManager.GetLogger("engine");
        }

        
    }
}
