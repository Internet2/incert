using System;
using System.Diagnostics;
using System.IO;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Results.Errors.SecurityPolicies;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.SecurityPolicies
{
    class ApplySecurityPolicy : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        public ApplySecurityPolicy(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string PolicyPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string PolicyDatabase
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!File.Exists(PolicyPath))
                    return new FileNotFound { Target = PolicyPath };

                var databasePath = Path.Combine(PathUtilities.WindowsDirectory, PolicyDatabase);
                var logPath = Path.Combine(PathUtilities.LogFolder, "policy.log");

                var info = new ProcessStartInfo(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.System)
                        , "secedit.exe"))
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = string.Format("/configure /db {0} /cfg {1} /overwrite /quiet /log {2}",
                            databasePath.ToQuoted(), 
                            PolicyPath.ToQuoted(), 
                            logPath.ToQuoted())
                    };

                using (var process = Process.Start(info))
                {
                    process.WaitUntilExited();

                    var result = EvaluateResult(process.ExitCode, logPath);
                    if (!result.Result)
                    {
                        Log.WarnFormat("Could not apply security policy: {0}", result.Reason);
                        return new CouldNotSetSecurityPolicy { Issue = result.Reason };
                    }    
                }
                
                Log.Info("Security policies applied.");
                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static BooleanReason EvaluateResult(int exitCode, string logPath)
        {
            try
            {
                if (exitCode == 0)
                    return new BooleanReason(true, "security policies applied successfully");

                if (exitCode == 3)
                    return new BooleanReason(true, "security policies applied successfully, but with warnings");

                Log.WarnFormat("secedit returned {0}", exitCode);

                if (!File.Exists(logPath))
                    return new BooleanReason(false, "unknown issue");

                var buffer = File.ReadAllLines(logPath);
                if (buffer.LongLength<4)
                    return new BooleanReason(false, "unknown issue");

                var reason = buffer[buffer.LongLength - 3];
                if (string.IsNullOrWhiteSpace(reason))
                    reason = string.Format("unknown issue (return code = {0})", exitCode);

                return new BooleanReason(false, reason);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to parse the secedit log file: {0}", e.Message);
                return new BooleanReason(false, "unknown issue");
            }
            
        }

        public override string GetFriendlyName()
        {
            return "Apply security policy";
        }
    }
}
