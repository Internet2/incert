using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.TaskBranches;

namespace Org.InCommon.InCert.Engine.Tasks
{
    public interface ITask:IImportable
    {
        // functionality actions
        IResult Execute(IResult previousResults);

        // control properties
        //string Branch { get; set; }
        string ErrorBranch { get; set; }
        string NavigationPoint { get; set; }

        bool ForwardOnly { get; set; }
        int GetIndex();
        ITaskBranch Parent { get; set; }

        bool LogIfSkipped { get; set; }
        bool SuppressLogging { get; }

        // ui properties
        int Delay { get; set; }
        int MinimumTaskTime { get; set; }
        string GetFriendlyName();
        string GetLogName();

        string Id { get; set; }
        string UiMessage { get; set; }
        BooleanReason EvaluateRootCondition();



    }

}
