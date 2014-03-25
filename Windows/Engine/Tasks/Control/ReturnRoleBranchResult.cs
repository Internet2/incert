using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.TaskBranches;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    public class ReturnRoleBranchResult:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public BranchRoles Role { get; set; }

        public ReturnRoleBranchResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var branch = BranchManager.GetBranchForRole(Role, Engine.Mode, true);

                if (branch == null)
                {
                    Log.DebugFormat("No branch found for Role {0} and mode {1}; skipping branch", Role, Engine.Mode);
                    return new NextResult();
                }

                return new BranchResult(branch.Name);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Return branch result for branch with role {0} and mode {1}", Role, Engine.Mode);
        }
    }
}
