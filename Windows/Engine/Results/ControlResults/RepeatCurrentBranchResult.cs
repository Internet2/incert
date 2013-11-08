using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class RepeatCurrentBranchResult:AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            return new MoveToFirstTask();
        }

        public override bool IsOk()
        {
            return false;
        }
    }
}
