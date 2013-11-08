using System;
using System.Diagnostics;
using System.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class CloseProcessByName:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        public CloseProcessByName(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string ProcessName
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ProcessName))
                    throw new Exception("Process name cannot be empty");

                var processList = Process.GetProcessesByName(ProcessName);
                if (!processList.Any())
                    return new NextResult();
                
                foreach (var process in Process.GetProcessesByName(ProcessName))
                {
                    Log.DebugFormat("Attempting to close process {0}", process.ProcessName);
                    process.Kill();
                    UserInterfaceUtilities.WaitForSeconds(DateTime.UtcNow, 3);
                    if (!process.HasExited)
                        Log.WarnFormat("Instance of {0} may still be active", process.ProcessName);
                    else
                        Log.InfoFormat("Instance of {0} closed", process.ProcessName);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Close external process ({0})", ProcessName);
        }
    }
}
