using System;
using System.Threading;
using System.Windows;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Tasks;
using log4net;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers;

namespace Org.InCommon.InCert.Engine.TaskBranches
{
    public class TaskBranch : AbstractBranch
    {
        private static readonly ILog Log = Logger.Create();
       
    
        public TaskBranch(IEngine engine):base(engine)
        {
          
        }

        public override IResult Execute(IResult previousResult)
        {
            {
                Log.Branch(this);
                EngineEvents.OnBranchStarted(this, new BranchEventData(this));

                // if somehow the branch gets into a state where it has no members,
                // log and return a skip-result
                if (Tasks.Count <= 0)
                {
                    Log.WarnFormat("Current branch {0} contains no elements!", Name);
                    return new SkipResult();
                }

                // always start with the first task
                var currentTask = Tasks[0];
                var lastTask = currentTask;
                var movingForward = true;
                IResult result;
                var startTime = DateTime.UtcNow;
                do
                {
                    SettingsManager.PreviousTaskResult = previousResult;
                    ClearCancelFlags();
                    result = ExecuteCurrentTask(currentTask, previousResult, movingForward);
                    result = CheckForCloseRequest(result);

                    result.LogIfError();

                    if (result is BranchResult)
                        result = ExecuteBranch(result);

                    if (result is ErrorResult)
                    {
                        EngineEvents.OnIssueOccurred(this, new IssueEventData(result));
                        result = ExecuteErrorBranch(currentTask.ErrorBranch, result);
                    }
                        
                    currentTask = result.GetBranchStrategy().GetNextTask(this, currentTask);
                    movingForward = IsForwardProgress(currentTask, lastTask);
                    lastTask = currentTask;
                    previousResult = result;

                } while (currentTask != null);

                // wait for minimum branch time, but not if there's an error
                if (!(result is ErrorResult))
                    DialogsManager.WaitForDurationOrCancel(startTime, new TimeSpan(0, 0, 0, MinimumBranchTime));
                
                // need to check for close request after waiting for minimum branch time
                // in case user clicked close button during this time
                result = CheckForCloseRequest(result);

                EngineEvents.OnBranchCompleted(this, new BranchEventData(this));

                return result.AdjustResultByBranchContext(this);
            }
        }

        private IResult ExecuteCurrentTask(ITask currentTask, IResult previousResults, bool movingForward)
        {
            var performTask = PerformTask(currentTask, movingForward);
            if (!performTask.Result)
            {
                Log.DebugFormat("Skipping task ({0}): {1}", currentTask.GetFriendlyName(), performTask.Reason);
                return new SkipResult { SkipForward = movingForward };
            }

            Log.Task(currentTask);
            EngineEvents.OnTaskStarted(this, new TaskEventData(currentTask));
            
            var startTime = DateTime.UtcNow;

            var result = currentTask.Execute(previousResults);
            result.FromTask = currentTask;

            DialogsManager.WaitForDurationOrCancel(
                startTime, 
                new TimeSpan(0,0,0,currentTask.MinimumTaskTime));

            EngineEvents.OnTaskCompleted(this, new TaskEventData(currentTask));
            return result;
        }

        private bool IsForwardProgress(AbstractTask currentTask, AbstractTask lastTask)
        {
            var currentIndex = Tasks.IndexOf(currentTask);
            var lastIndex = Tasks.IndexOf(lastTask);

            return currentIndex >= lastIndex;
        }

        private static BooleanReason PerformTask(ITask task, bool movingForward)
        {
            if (task.ForwardOnly && !movingForward)
                return new BooleanReason(false, "task is flagged as forward-only");

            return task.EvaluateRootCondition();
        }

        private IResult CheckForCloseRequest(IResult previousResults)
        {
            WaitForPendingCancel();
            return DialogsManager.CancelRequested ? new CloseResult() : previousResults;
        }

        private void WaitForPendingCancel()
        {
            while (DialogsManager.CancelPending)
            {
                Application.Current.DoEvents(250);
            }
        }

        private IResult ExecuteBranch(IResult previousResult)
        {
            var branchResult = previousResult as BranchResult;
            if (branchResult == null)
            {
                Log.Warn("Invalid result passed to Execute Branch");
                return previousResult;
            }

            if (String.IsNullOrWhiteSpace(branchResult.Branch))
                return new BranchNotFound("[invalid branch]");

            var taskBranch = BranchManager.GetBranch(branchResult.Branch);
            if (taskBranch == null)
                return new BranchNotFound(branchResult.Branch);

            taskBranch.Parent = this;
            return taskBranch.Execute(branchResult);

        }

        private IResult ExecuteErrorBranch(string errorBranch, IResult previousResults)
        {
            if (String.IsNullOrWhiteSpace(errorBranch))
                return previousResults;

            var taskBranch = BranchManager.GetBranch(errorBranch);
            if (taskBranch == null)
                return new BranchNotFound(errorBranch);

            taskBranch.Parent = this;
            return taskBranch.Execute(previousResults);
        }
    }
}
