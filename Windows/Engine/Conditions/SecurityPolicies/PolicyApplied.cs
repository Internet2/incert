using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.SecurityPolicies
{
    class PolicyApplied:AbstractCondition
    {
        private static readonly ILog Log = Logger.Create();
        
        public string PolicyPath
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value );}
        }

        public string IgnoreTokens
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public PolicyApplied(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var logPath = GenerateAnalysisLog(PolicyPath);
                if (string.IsNullOrWhiteSpace(logPath))
                    return new BooleanReason(false, "analysis log path is empty or invalid");

                if (!File.Exists(logPath))
                    return new BooleanReason(false, "analysis log does not exist");

                var buffer = File.ReadAllLines(logPath);
                if (!buffer.Any())
                    return new BooleanReason(false, "analysis log contains no content");

                var ignoreList = new List<string>();
                if (!string.IsNullOrWhiteSpace(IgnoreTokens))
                    ignoreList = IgnoreTokens.Split(',').ToList();

                return buffer
                           .Where(entry => !string.IsNullOrWhiteSpace(entry))
                           .Where(entry => entry.StartsWith("mismatch", StringComparison.InvariantCultureIgnoreCase))
                           .Any(entry => !IgnoreMismatch(ignoreList, entry)) 
                        ? new BooleanReason(false, "At least one policy mismatch found") 
                        : new BooleanReason(true, "");
            }
            catch (Exception e)
            {
                return new BooleanReason(
                    false, 
                    "An issue occurred while evaluating the condition: {0}", 
                    e.Message);
            }
        }

        private static bool IgnoreMismatch(List<string> ignoreList, string mismatch)
        {
            if (!ignoreList.Any())
                return false;

            if (string.IsNullOrWhiteSpace(mismatch))
                return true;

            return ignoreList.Any(token => mismatch.ToLowerInvariant().Contains(token.ToLowerInvariant()));
        }

        private static string GenerateAnalysisLog(string policyPath)
        {
            var logPath = Path.GetTempFileName();
            var databasePath = Path.GetTempFileName();

            File.Delete(logPath);
            File.Delete(databasePath);

            var info = new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("secedit.exe"))
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = string.Format("/analyze /db {0} /cfg {1} /log {2}",
                    databasePath.ToQuoted(),
                    policyPath.ToQuoted(),
                    logPath.ToQuoted())
            };

            using (var process = Process.Start(info))
            {
                process.WaitUntilExited();
                if (process.ExitCode != 0)
                {
                    Log.WarnFormat("secedit.exe returned result {0}; assuming issue running utility.", process.ExitCode);
                    return "";
                }
            }
                
            File.Delete(databasePath);

            return logPath;
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("PolicyPath");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            PolicyPath = XmlUtilities.GetTextFromAttribute(node, "policyPath");
        }
    }
}
