using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class SilentRestartComputerResult : AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            return new LeaveBranch();
        }

        public override bool IsOk()
        {
            return false;
        }

        public override int ResultCode
        {
            get { return 2; }
        }
    }
}
