using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Tasks;
using log4net;

namespace Org.InCommon.InCert.Engine.TaskBranches.BranchStrategies
{
    public class MoveToKeyedTask: IBranchStrategy
    {
        private static readonly ILog Log = Logger.Create();
        public string Key { get; set; }

        public AbstractTask GetNextTask(TaskBranch context, AbstractTask currentTask)
        {
            var result = context.GetKeyedTask(Key);
            if (result == null)
            {
                Log.WarnFormat("Could not locate task for key {0}", Key);
                result = context.GetNextTask(currentTask);    
            }
            
            return result;
        }
    }
}
