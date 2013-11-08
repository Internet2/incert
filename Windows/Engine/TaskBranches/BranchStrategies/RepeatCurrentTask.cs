using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies
{
    class RepeatCurrentTask:IBranchStrategy
    {
        public AbstractTask GetNextTask(TaskBranch context, AbstractTask currentTask)
        {
            return currentTask;
        }
    }
}
