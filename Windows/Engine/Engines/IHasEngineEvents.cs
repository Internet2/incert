using System;

namespace Org.InCommon.InCert.Engine.Engines
{
    public interface IHasEngineEvents
    {
        event EventHandler TaskStarted;
        event EventHandler TaskCompleted;
        event EventHandler BranchStarted;
        event EventHandler BranchCompleted;
        event EventHandler IssueOccurred;

        void OnTaskStarted(object sender, EventArgs e);
        void OnTaskCompleted(object sender, EventArgs e);

        void OnBranchStarted(object sender, EventArgs e);
        void OnBranchCompleted(object sender, EventArgs e);

        void OnIssueOccurred(object sender, EventArgs e);
    }
}
