using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class RepeatCurrentTaskResult:AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            return new RepeatCurrentTask();
        }

        public override bool IsOk()
        {
           return false;
        }

    }
}
