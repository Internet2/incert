using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.TaskBranches;

namespace Org.InCommon.InCert.Engine.Results.Errors.Control
{
    class BranchNotFound : ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Branch { get; set; }

        public BranchNotFound()
        {
            Branch = "[unknown]";
        }
        
        public BranchNotFound(string branch)
        {
            Branch = branch;
        }

        public BranchNotFound(BranchRoles role, EngineModes mode)
        {
            Issue = string.Format("No branch defined for role {0} and mode {1}", role, mode);
        }


    }

}
