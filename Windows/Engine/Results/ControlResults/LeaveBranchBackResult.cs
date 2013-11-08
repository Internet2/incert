using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class LeaveBranchBackResult:AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            return new LeaveBranch();
        }

        public override bool IsOk()
        {
            return true;
        }

        public override IResult AdjustResultByBranchContext(TaskBranches.TaskBranch tasks)
        {
            return new BackResult();
        }
    }
}
