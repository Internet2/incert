using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.TaskBranches
{
    public class RoleBranch:TaskBranch
    {
        public RoleBranch(IEngine engine) : base(engine)
        {
        }

        public BranchRoles BranchRole { get; set; }
        public EngineModes RoleMode { get; set; }
        
        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            BranchRole = XmlUtilities.GetEnumValueFromAttribute(node, "role", BranchRoles.None);
            RoleMode = XmlUtilities.GetEnumValueFromAttribute(node, "roleMode", EngineModes.All);
        }
    }
}
