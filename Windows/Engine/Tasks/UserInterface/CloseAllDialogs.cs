using System;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class CloseAllDialogs : AbstractTask
    {
        public CloseAllDialogs(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                DialogsManager.CloseAllDialogs();
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Close all dialogs";
        }
    }
}
