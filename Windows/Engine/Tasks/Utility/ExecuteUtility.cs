using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class ExecuteUtility:AbstractExecuteUtilityTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string ResultKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string OutputKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public ExecuteUtility(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TargetPath))
                    throw new Exception("Target cannot be null or empty");

                if (!File.Exists(TargetPath))
                    return new FileNotFound{Target = TargetPath};

                InitializeArguments();
                
                var info = new ProcessStartInfo(TargetPath)
                    {
                        Arguments = Arguments,
                        UseShellExecute = false,
                        WorkingDirectory = WorkingDirectory,
                        CreateNoWindow = HideWindow
                    };

                if (!string.IsNullOrWhiteSpace(OutputKey))
                    info.RedirectStandardOutput = true;
                
                using (var process = Process.Start(info))
                {
                    process.WaitUntilExited();

                    Log.DebugFormat("Process exit code = {0}", process.ExitCode);
                    if (!string.IsNullOrWhiteSpace(OutputKey))
                    {
                        var outputText = process.StandardOutput.ReadToEnd();
                        Log.DebugFormat(
                            string.IsNullOrWhiteSpace(outputText)
                            ? "Process output: [empty]"
                            : "Process output:\n{0}", outputText);    
                        SettingsManager.SetTemporarySettingString(OutputKey, outputText);
                    }
                    
                    
                    if (!string.IsNullOrWhiteSpace(ResultKey))
                        SettingsManager.SetTemporarySettingString(ResultKey, process.ExitCode.ToString(CultureInfo.InvariantCulture));
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
            var fileName = "[unknown]";
            if (!string.IsNullOrWhiteSpace(TargetPath))
                fileName = Path.GetFileName(TargetPath);

            return string.Format("Execute utility ({0})", fileName);
        }
    }
}
