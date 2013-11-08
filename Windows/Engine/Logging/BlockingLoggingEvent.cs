using System;
using System.Runtime.Serialization;
using log4net.Core;
using log4net.Repository;

namespace Org.InCommon.InCert.Engine.Logging
{
    // this type of logging event will cause the web-service appender to block
    // until the logging request completes
    class BlockingLoggingEvent:LoggingEvent
    {
        public BlockingLoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, string loggerName, Level level, object message, Exception exception) : base(callerStackBoundaryDeclaringType, repository, loggerName, level, message, exception)
        {
        }

        public BlockingLoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data, FixFlags fixedData) : base(callerStackBoundaryDeclaringType, repository, data, fixedData)
        {
        }

        public BlockingLoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data) : base(callerStackBoundaryDeclaringType, repository, data)
        {
        }

        public BlockingLoggingEvent(LoggingEventData data) : base(data)
        {
        }

        protected BlockingLoggingEvent(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
