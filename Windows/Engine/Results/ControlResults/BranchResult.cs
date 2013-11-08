using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class BranchResult:AbstractTaskResult
    {

        public string Branch { get; private set; }
        
        public BranchResult(string branch)
        {
            Branch = branch;
        }

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
