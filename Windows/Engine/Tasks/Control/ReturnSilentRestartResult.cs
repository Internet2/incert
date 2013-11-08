using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnSilentRestartResult:AbstractTask
    {
        public ReturnSilentRestartResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new SilentRestartComputerResult();
        }

        public override string GetFriendlyName()
        {
            return "Return silent restart computer result";
        }
    }
}
