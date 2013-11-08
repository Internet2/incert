using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnLeaveBranchBackResult:AbstractTask
    {
        public ReturnLeaveBranchBackResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new LeaveBranchBackResult();
        }

        public override string GetFriendlyName()
        {
            return "return leave-branch back result";
        }
    }
}
