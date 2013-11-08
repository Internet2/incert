using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Control
{
    class BranchExists : AbstractCondition
    {
        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public BranchExists(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            var branch = BranchManager.GetBranch(Key);
            return branch == null ? new BooleanReason(false, "The branch " + Key + " does not exist") :
                new BooleanReason(true, "The branch " + Key + " exists");
        }

        public override bool IsInitialized()
        {
            return !string.IsNullOrWhiteSpace(Key);
        }

        public override void ConfigureFromNode(XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
        }
    }
}
