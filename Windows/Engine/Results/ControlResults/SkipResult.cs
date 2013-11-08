using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;

namespace Org.InCommon.InCert.Engine.Results.ControlResults
{
    class SkipResult : AbstractTaskResult
    {
        public override IBranchStrategy GetBranchStrategy()
        {
            if (!SkipForward)
                return new MoveToPreviousTask();

            return new MoveToNextTask();
        }

        public bool SkipForward { get; set; }

        public override bool IsOk()
        {
            return false;
        }
    }
}
