using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnLeaveBranchNextResult:AbstractTask
    {
        public ReturnLeaveBranchNextResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new LeaveBranchNextResult();
        }

        public override string GetFriendlyName()
        {
            return "Return leave-branch next result";
        }
    }
}
