using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net.Layout;

namespace Org.InCommon.InCert.Engine.Tasks.Logging
{
    class SetRemoteLoggingMessageFormat:AbstractRemoteLoggingTask
    {
        [PropertyAllowedFromXml]
        public string MessageFormat { get; set; }
        
        public SetRemoteLoggingMessageFormat(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(MessageFormat))
                    throw  new Exception("Message format cannot be null or empty");
                
                var webserviceLayout = new PatternLayout { ConversionPattern = MessageFormat };
                webserviceLayout.ActivateOptions();

                var appender = GetWebserviceAppender();
                appender.Layout = webserviceLayout;
                appender.ActivateOptions();


                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
           return "Set remote logging message format";
        }
    }
}
