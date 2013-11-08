using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnLeaveBranchRepeatTaskResult: AbstractTask
    {
        public ReturnLeaveBranchRepeatTaskResult(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            return new RepeatBranchingTaskResult();
        }

        public override string GetFriendlyName()
        {
            return "Return leave-branch, repeat-task result";
        }
    }
}
