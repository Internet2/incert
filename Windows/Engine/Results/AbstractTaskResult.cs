using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies;
using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.Results
{
    public abstract class AbstractTaskResult : AbstractImportable, IResult
    {
        private ITask _fromTask;

        protected AbstractTaskResult() : base(null)
        {
        }

        protected AbstractTaskResult(IEngine engine) : base(engine)
        {
        }

        public abstract IBranchStrategy GetBranchStrategy();

        public virtual IResult AdjustResultByBranchContext(TaskBranch tasks)
        {
            return this;
        }

        [ResultExtensions.IncludeInErrorDetails]
        public ITask FromTask
        {
            get { return _fromTask; }
            set
            {
                if (_fromTask != null)
                    return;
                _fromTask = value;
            }
        }

        public object PrivateData { get; set; }

        public abstract bool IsOk();

        public virtual int ResultCode { get { return 0; } }
    }

}
