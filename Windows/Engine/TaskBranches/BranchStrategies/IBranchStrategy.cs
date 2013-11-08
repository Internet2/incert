using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies
{
    public interface IBranchStrategy
    {
        AbstractTask GetNextTask(TaskBranch context, AbstractTask currentTask);
    }
}
