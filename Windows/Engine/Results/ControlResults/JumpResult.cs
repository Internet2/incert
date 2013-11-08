using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class JumpResult: AbstractTaskResult
    {
        public string Key { get; set; }
        
        public override IBranchStrategy GetBranchStrategy()
        {
            return new MoveToKeyedTask {Key = Key};
        }

        public override bool IsOk()
        {
            return false;
        }
    }
}
