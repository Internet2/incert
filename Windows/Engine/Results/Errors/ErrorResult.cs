using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;


namespace Org.InCommon.InCert.Engine.Results.Errors
{
    public abstract class ErrorResult: AbstractTaskResult
    {
        protected ErrorResult()
        {
            IssueCode = -1;
        }
        
        public bool Logged { get; set; }

        public override IBranchStrategy GetBranchStrategy()
        {
            return new LeaveBranch();
        }

        public virtual string ErrorName
        {
            get { return GetType().Name; }
        }

        [ResultExtensions.IncludeInErrorDetails]
        public string Issue { get; set; }

        public int IssueCode { get; set; }

        public override bool IsOk()
        {
            return false;
        }

        public override int ResultCode
        {
            get { return IssueCode; }
        }
            

     
    }
}
