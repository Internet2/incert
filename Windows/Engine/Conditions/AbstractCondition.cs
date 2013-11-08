using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions
{
    public abstract class AbstractCondition : AbstractDynamicPropertyContainer, ICondition
    {
        public abstract BooleanReason Evaluate();
        public abstract bool IsInitialized();
        public string SkipText { get; set; }
        public bool CachePreviousResults { get; set; }
        public bool Cumulative { get; set; }

        protected AbstractCondition(IEngine engine) : base(engine)
        {
            SkipText = "";
        }
        


        public override void ConfigureFromNode(XElement node)
        {
            SkipText = XmlUtilities.GetTextFromChildNode(node, "text");
            CachePreviousResults = XmlUtilities.GetBooleanFromAttribute(node, "cache",false);
        }
    }
}
