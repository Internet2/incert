using System;
using System.Diagnostics;
using System.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.SystemInfo
{
    class IsProcessRunning : AbstractCondition
    {
        public IsProcessRunning(IEngine engine)
            : base (engine)
        {
        }

        public string ProcessName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(ProcessName))
                    return new BooleanReason(false, "No process name specified");

                return !Process.GetProcessesByName(ProcessName).Any() 
                    ? new BooleanReason(false, "No instances of {0} are active", ProcessName) 
                    : new BooleanReason(true, "At least one instance of {0} is active", ProcessName);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("ProcessName");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            ProcessName = XmlUtilities.GetTextFromAttribute(node, "processName");
        }
    }
}
