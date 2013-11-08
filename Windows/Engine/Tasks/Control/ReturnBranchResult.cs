using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.Control;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnBranchResult:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string Branch
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ReturnBranchResult(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            if (MinimumTaskTime>0)
               Log.Warn("Consider using minimum branch time instead of using minimum task time here.");
            
            if (string.IsNullOrWhiteSpace(Branch))
                return new BranchNotFound {Branch = "[branch not specified]"};

            return new BranchResult(Branch);
        }

        public override string GetFriendlyName()
        {
            return string.Format("Return branch result ({0})",Branch);
        }
    }
}
