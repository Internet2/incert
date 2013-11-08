using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies
{
    class MoveToPreviousTask:IBranchStrategy
    {
        public AbstractTask GetNextTask(TaskBranch context, AbstractTask currentTask)
        {
            return context.GetPreviousTask(currentTask);
        }
    }
}
