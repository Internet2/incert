using System.Linq;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using log4net;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Org.InCommon.InCert.Engine.Tasks.Logging
{
    abstract class AbstractRemoteLoggingTask : AbstractTask
    {
       
        protected AbstractRemoteLoggingTask(IEngine engine)
            : base (engine)
        {
        }

        protected WebServiceAppender GetWebserviceAppender()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            return hierarchy.Root.Appenders.OfType<WebServiceAppender>()
                .FirstOrDefault() ?? AddAppender(hierarchy);
        }

        private WebServiceAppender AddAppender(Hierarchy hierarchy)
        {
            var webserviceLayout = new PatternLayout { ConversionPattern = "%P{UserId} %P{Identifier} %P{Session} %-5p %m" };
            webserviceLayout.ActivateOptions();

            var appender = new WebServiceAppender(EndpointManager) { Layout = webserviceLayout, Threshold = Level.Info };
            appender.ActivateOptions();

            hierarchy.Root.AddAppender(appender);
            return appender;
        }
    }
}
