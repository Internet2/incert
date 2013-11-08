using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;
using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.Results
{
    public interface IResult
    {
        IBranchStrategy GetBranchStrategy();
        IResult AdjustResultByBranchContext(TaskBranch tasks);
        ITask FromTask { get; set; }

        object PrivateData { get; set; }
        bool IsOk();
        int ResultCode { get; }
    }
}