using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;
using log4net.Core;

namespace Org.InCommon.InCert.Engine.Tasks.Logging
{
    class LogEvent : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public bool Asynchronous { get; set; }

        [PropertyAllowedFromXml]
        public SupportedLoggingLevels.Values Event { get; set; }

        [PropertyAllowedFromXml]
        public string Message
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public LogEvent(IEngine engine)
            : base(engine)
        {
        }

        public override bool SuppressLogging
        {
            get { return true; }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var eventData = new LoggingEventData
                    {
                        Level = Event.GetAssociatedLevel(),
                        Message = Message,
                        TimeStamp = DateTime.Now
                    };

                if (eventData.Level == null)
                {
                    Log.Warn("Cannot upload log event; Level is null");
                    return new NextResult();
                }

                if (string.IsNullOrWhiteSpace(eventData.Message))
                    eventData.Message = "[no content]";

                var eventEntry = Asynchronous 
                    ? new LoggingEvent(eventData) 
                    : new BlockingLoggingEvent(eventData);

                Log.Logger.Log(eventEntry);

                return new NextResult();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to record a log event: {0}", e.Message);
                return new NextResult();
            }
        }

        public override string GetFriendlyName()
        {
            return "Log event";
        }
    }
}
