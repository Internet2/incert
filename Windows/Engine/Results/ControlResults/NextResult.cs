using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    public class NextResult : AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            return new MoveToNextTask();
        }

        public override bool IsOk()
        {
            return true;
        }
    }
}
