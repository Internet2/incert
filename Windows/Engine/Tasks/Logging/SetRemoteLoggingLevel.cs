using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Logging
{
    class SetRemoteLoggingLevel : AbstractRemoteLoggingTask
    {
        public SetRemoteLoggingLevel(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public SupportedLoggingLevels.Values Level { get; set; }
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var levelInstance = Level.GetAssociatedLevel();
                if (levelInstance == null)
                    throw new Exception("Log level cannot be null/none. If you want to disable remote logging, use the task for that!");

                var appender = GetWebserviceAppender();
                appender.Threshold = levelInstance;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set remote logging level";
        }
    }
}
