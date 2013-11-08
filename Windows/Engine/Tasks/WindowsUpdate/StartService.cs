using System;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class StartService:AbstractTask
    {
        private const string WindowsUpdateServiceName = "wuauserv";

        public StartService(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var task = Task<IResult>.Factory.StartNew(() => ServiceUtilities.EnableService(WindowsUpdateServiceName));
                task.WaitUntilExited();

                return task.Result;
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Start Windows Update service";
        }
    }
}
