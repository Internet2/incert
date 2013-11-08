using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Tasks;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.TaskBranches
{
    public abstract class AbstractBranch : AbstractImportable, ITaskBranch
    {
        private static readonly ILog Log = Logger.Create();
        private readonly List<AbstractTask> _tasks = new List<AbstractTask>();

        protected AbstractBranch(IEngine engine) : base(engine)
        {
            
        }

        public string Name { get; private set; }
        public TaskBranch Parent { get; set; }
        public abstract IResult Execute(IResult previousResults);
        
        public List<AbstractTask> Tasks
        {
            get { return _tasks; }
        }

        
        public override bool Initialized()
        {
            if (!Tasks.Any())
                return false;

            return !string.IsNullOrWhiteSpace(Name);
        }

        public int GetTaskIndex(AbstractTask task)
        {
            return Tasks.IndexOf(task);
        }

        public AbstractTask GetNextTask(AbstractTask task)
        {
            var index = GetTaskIndex(task);
            if (index == -1)
                return null;

            index++;

            return index > (Tasks.Count - 1) ? null : Tasks[index];
        }

        public AbstractTask GetPreviousTask(AbstractTask task)
        {
            var index = GetTaskIndex(task);
            if (index == -1)
                return null;

            index--;

            return index < 0 ? null : Tasks[index];
        }

        public AbstractTask GetKeyedTask(string key)
        {
            return Tasks.FirstOrDefault(task => task.NavigationPoint.Equals(key, StringComparison.InvariantCulture));
        }

        public int MinimumBranchTime { get; set; }
        public void ClearCancelFlags()
        {
            DialogsManager.CancelPending = false;
            DialogsManager.CancelRequested = false;
        }

        public override void ConfigureFromNode(XElement node)
        {

            Name = XmlUtilities.GetTextFromAttribute(node, "name");
            MinimumBranchTime = XmlUtilities.GetIntegerFromAttribute(node, "minimumBranchTime", 0);

            var taskNodes = node.Elements().ToList();
            if (!taskNodes.Any())
            {
                Log.Warn("Branch contains no task nodes");
                return;
            }

            foreach (var task in taskNodes.Select(GetInstanceFromNode<AbstractTask>).Where(task => task != null))
            {
                task.Parent = this;
                _tasks.Add(task);
            }
        }
    }
}
