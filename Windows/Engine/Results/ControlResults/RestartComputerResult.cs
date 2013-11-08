using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class RestartComputerResult:AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            return new LeaveBranch();
        }

        public override bool IsOk()
        {
            return false;
        }
    }
}
