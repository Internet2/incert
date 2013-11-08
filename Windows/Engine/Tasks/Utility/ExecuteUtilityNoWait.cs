using System;
using System.Diagnostics;
using System.IO;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class ExecuteUtilityNoWait:AbstractExecuteUtilityTask
    {
        public ExecuteUtilityNoWait(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TargetPath))
                    throw new Exception("Target cannot be null or empty");

                if (!File.Exists(TargetPath))
                    return new FileNotFound { Target = TargetPath };

                InitializeArguments();

                if (!string.IsNullOrWhiteSpace(Verb))
                    UseShellExecute = true;

                var info = new ProcessStartInfo(TargetPath)
                {
                    Arguments = Arguments,
                    UseShellExecute = UseShellExecute,
                    WorkingDirectory = WorkingDirectory,
                    CreateNoWindow = HideWindow,
                    Verb = Verb
                };

                using (Process.Start(info))
                {
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
            return "Execute utility no wait";
        }
    }
}
