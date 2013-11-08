using System.Collections.Generic;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.TaskBranches
{
    public interface IBranchManager
    {
        bool IsInitialized();
        void Initialize();
        bool ImportBranchesFromXml(XElement node);
        bool ImportBranches(List<ITaskBranch> branches);
        ITaskBranch GetBranch(string value);
        ITaskBranch GetBranchForRole(BranchRoles role, EngineModes mode, bool fallbackToAll);
        void AddBranch(ITaskBranch branch);
    }
}