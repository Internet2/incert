using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnExitResult:AbstractTask
    {
        public ReturnExitResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new ExitUtilityResult();
        }

        public override string GetFriendlyName()
        {
            return "Return exit utility result";
        }
    }
}
