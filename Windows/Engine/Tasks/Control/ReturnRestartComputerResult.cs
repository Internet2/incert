using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnRestartComputerResult:AbstractTask
    {
        public ReturnRestartComputerResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new RestartComputerResult();
        }

        public override string GetFriendlyName()
        {
            return "Return restart computer result";
        }
    }
}
