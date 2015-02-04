using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Conditions;
using Org.InCommon.InCert.Engine.Conditions.Grouping;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks
{
    public abstract class AbstractTask : AbstractDynamicPropertyContainer, ITask
    {

        private ICondition _rootCondition;

        protected AbstractTask(IEngine engine)
            : base(engine)
        {
        }

        public abstract IResult Execute(IResult previousResults);

        [PropertyAllowedFromXml]
        public string ErrorBranch
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string NavigationPoint
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public bool ForwardOnly { get; set; }

        [PropertyAllowedFromXml]
        public bool LogIfSkipped { get; set; }

        public virtual bool SuppressLogging { get { return false; } }

        /// <summary>
        /// Gets or sets the amount of time (in milliseconds) to wait before preceeding to the next task.
        /// </summary>
        [PropertyAllowedFromXml]
        public int Delay { get; set; }

        /// <summary>
        /// Gets or sets the minimum task time (in milliseconds) that the task should be displayed. If the task completes in less time than is specified, the program will wait for the specified time to elapse before continuing.
        /// </summary>
        /// 
        [PropertyAllowedFromXml]
        public int MinimumTaskTime { get; set; }

        public abstract string GetFriendlyName();

        /// <summary>
        /// For logging purposes, gets the name of the task.
        /// </summary>
        /// <returns></returns>
        public string GetLogName()
        {
            return !String.IsNullOrWhiteSpace(GetFriendlyName()) ? GetFriendlyName() : GetType().Name;
        }

        public string Id { get; set; }

        public string UiMessage { get; set; }

        public BooleanReason EvaluateRootCondition()
        {
            if (_rootCondition == null)
                return new BooleanReason(true, "");

            return _rootCondition.Evaluate();
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);

            ErrorBranch = XmlUtilities.GetTextFromAttribute(node, "errorBranch");
            NavigationPoint = XmlUtilities.GetTextFromAttribute(node, "navPoint");
            ForwardOnly = XmlUtilities.GetBooleanFromAttribute(node, "forwardOnly", false);
            LogIfSkipped = XmlUtilities.GetBooleanFromAttribute(node, "logIfSkipped", false);
            Delay = XmlUtilities.GetIntegerFromAttribute(node, "delay", 0);
            MinimumTaskTime = XmlUtilities.GetIntegerFromAttribute(node, "minimumTaskTime", 0);
            Id = XmlUtilities.GetTextFromAttribute(node, "id");
            UiMessage = XmlUtilities.GetTextFromAttribute(node, "message");

            SetRootCondition(GetInstanceFromNode<AllTrue>(node.Element("Conditions.All")));
            SetRootCondition(GetInstanceFromNode<AnyTrue>(node.Element("Conditions.Any")));
        }

        private void SetRootCondition(ICondition value)
        {
            // only replace value if value !=null
            if (value == null)
                return;

            _rootCondition = value;
        }

        public int GetIndex()
        {
            return Parent.GetTaskIndex(this);
        }

        public ITaskBranch Parent { get; set; }


    }


}
