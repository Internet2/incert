using System;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers;

namespace Org.InCommon.InCert.Engine.Engines
{
    public interface IHasEngineEvents
    {
        event EventHandler<TaskEventData> TaskStarted;
        event EventHandler<TaskEventData> TaskCompleted;
        event EventHandler<BranchEventData> BranchStarted;
        event EventHandler<BranchEventData> BranchCompleted;
        event EventHandler<IssueEventData> IssueOccurred;

        void OnTaskStarted(object sender, TaskEventData e);
        void OnTaskCompleted(object sender, TaskEventData e);

        void OnBranchStarted(object sender, BranchEventData e);
        void OnBranchCompleted(object sender, BranchEventData e);

        void OnIssueOccurred(object sender, IssueEventData e);
    }
}
