using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;
using log4net.Core;

namespace Org.InCommon.InCert.Engine.Conditions.Grouping
{
    public abstract class AbstractGroupCondition:AbstractCondition
    {
        private static readonly ILog Log = Logger.Create();
        
        protected readonly List<AbstractCondition> Conditions = new List<AbstractCondition>();

        protected AbstractGroupCondition(IEngine engine):base(engine)
        {
        }

        public List<AbstractCondition> Children { get { return Conditions; } }
        public Level LogLevel { get; set; }

        public override bool IsInitialized()
        {
            return (Conditions.Count >=1);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);

            var levelValue = XmlUtilities.GetEnumValueFromAttribute(node, "logLevel", SupportedLoggingLevels.Values.None);
            LogLevel = levelValue.GetAssociatedLevel();

            foreach (var child in node.Elements())
            {
                var condition = GetInstanceFromNode<AbstractCondition>(child);
                if (condition == null)
                    continue;

                if (!condition.Initialized())
                    continue;

                Conditions.Add(condition);
            }
        }

        internal void LogMessage(string message)
        {
            if (LogLevel == null)
                return;
            
            Log.Logger.Log(null, LogLevel, message, null);
        }

        internal void LogFormat(string message, params object[] parameters)
        {
            var fullMessage = string.Format(message, parameters);
            LogMessage(fullMessage);
        }
    }
}
