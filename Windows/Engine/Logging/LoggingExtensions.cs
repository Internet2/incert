using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.Tasks;
using log4net;

namespace Org.InCommon.InCert.Engine.Logging
{
    public static class LoggingExtensions
    {
       
        /// <summary>
        /// Adds the custom auth levels to the logManager
        /// </summary>
        /// <param name="log"></param>
        public static void InitializeCustomLevels(this ILog log)
        {
            var levelMap = LogManager.GetRepository().LevelMap;
            levelMap.Add(SupportedLoggingLevels.Task);
            levelMap.Add(SupportedLoggingLevels.Branch);
            levelMap.Add(SupportedLoggingLevels.Start);
            levelMap.Add(SupportedLoggingLevels.Exit);
            levelMap.Add(SupportedLoggingLevels.Register);
            levelMap.Add(SupportedLoggingLevels.Track);
            levelMap.Add(SupportedLoggingLevels.Finish);
            levelMap.Add(SupportedLoggingLevels.Monitor);
            levelMap.Add(SupportedLoggingLevels.Waypoint);
        }

        /// <summary>
        /// Writes a task event to the log. 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="task"> </param>
        public static void Task(this ILog log, ITask task)
        {
            var level = SupportedLoggingLevels.Task;
            if (task.SuppressLogging)
                level = SupportedLoggingLevels.Debug;
            
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
            level, task.GetFriendlyName(), null);
        }

        /// <summary>
        /// Writes a branch event to the log.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="branch"></param>
        public static void Branch(this ILog log, ITaskBranch branch)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
            SupportedLoggingLevels.Branch, branch.Name, null);
        }

     
    }
}
