using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnRepeatCurrentBranchResult:AbstractTask
    {
        public ReturnRepeatCurrentBranchResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new RepeatCurrentBranchResult();
        }

        public override string GetFriendlyName()
        {
            return "Return repeat current branch result";
        }
    }
}
