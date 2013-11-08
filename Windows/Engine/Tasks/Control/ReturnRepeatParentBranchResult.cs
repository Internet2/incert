using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnRepeatParentBranchResult:AbstractTask
    {
        public ReturnRepeatParentBranchResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new RepeatParentBranchResult();
        }

        public override string GetFriendlyName()
        {
            return "Return repeat branch result";
        }
    }
}
