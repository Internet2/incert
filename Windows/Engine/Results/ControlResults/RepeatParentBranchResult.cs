using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class RepeatParentBranchResult:AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            return new LeaveBranch();
        }

        public override bool IsOk()
        {
            return false;
        }

        public override IResult AdjustResultByBranchContext(TaskBranches.TaskBranch tasks)
        {
            return new RepeatCurrentBranchResult();
        }
    }
}
