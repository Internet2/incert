using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies
{
    class MoveToNextTask: IBranchStrategy
    {
        public AbstractTask GetNextTask(TaskBranch context, AbstractTask currentTask)
        {
            return context.GetNextTask(currentTask);
        }
    }
}
