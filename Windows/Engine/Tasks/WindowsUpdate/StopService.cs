using System;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class StopService : AbstractTask
    {
        private const string WindowsUpdateServiceName = "wuauserv";
        private static readonly ILog Log = Logger.Create();

        public StopService(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {

            try
            {
                using (var disableTask = Task<BooleanReason>.Factory.StartNew(DisableService))
                {
                    disableTask.WaitUntilExited();

                    if (!disableTask.Result.Result)
                        Log.WarnFormat("Could not disable windows update service: {0}", disableTask.Result.Reason);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Stop windows update service";
        }

        private static BooleanReason DisableService()
        {
            using (var instance = ServiceUtilities.GetServiceInstance(WindowsUpdateServiceName))
                return ServiceUtilities.DisableService(instance);
        }
    }
}
