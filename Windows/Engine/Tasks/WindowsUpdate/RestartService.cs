using System;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class RestartService : AbstractTask
    {
        private const string WindowsUpdateServiceName = "wuauserv";

        public RestartService(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                using (var disableTask = Task<IResult>.Factory.StartNew(()=>ServiceUtilities.DisableService(WindowsUpdateServiceName)))
                    disableTask.WaitUntilExited();

                UserInterfaceUtilities.WaitForSeconds(DateTime.UtcNow, 1);

                using (var enableTask = Task<IResult>.Factory.StartNew(()=>ServiceUtilities.EnableService(WindowsUpdateServiceName)))
                    enableTask.WaitUntilExited();
                
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Restart windows update service";
        }
    }
}
