using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.TaskBranches
{
    public interface ITaskBranch
    {
        string Name { get; }
        TaskBranch Parent { get; set; }

        IResult Execute(IResult previousResults);
        List<AbstractTask> Tasks { get; }

        bool Initialized();
        int GetTaskIndex(AbstractTask task);
        AbstractTask GetNextTask(AbstractTask task);
        AbstractTask GetPreviousTask(AbstractTask task);
        AbstractTask GetKeyedTask(string key);

        int MinimumBranchTime { get; set; }

        void ClearCancelFlags();
    }
}
