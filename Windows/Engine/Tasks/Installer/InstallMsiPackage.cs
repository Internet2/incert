using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Installer
{
    class InstallMsiPackage : AbstractTask
    {
        private readonly List<string> _arguments = new List<string>();

        public InstallMsiPackage(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Argument
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                _arguments.Add(value);
            }
        }

        public string Arguments
        {
            get { return GetDynamicValue(); }
            private set { SetDynamicValue(value); }
        }
        
        [PropertyAllowedFromXml]
        public string MsiPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ResultObjectKey {get; set;}

        // convert the list of arguments into a string and assign it to the arguments property
        internal void InitializeArguments()
        {
            Arguments = string.Join(" ", _arguments);
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!File.Exists(MsiPath))
                    return new FileNotFound {Target = MsiPath};

                InitializeArguments();

                var info = new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("msiexec.exe"))
                    {
                        Arguments = string.Format("/i \"{0}\" {1}",
                        MsiPath, Arguments)
                    };
                int processResult;
                using (var process = Process.Start(info))
                {
                    process.WaitUntilExited();
                    processResult = process.ExitCode;
                }

                if (!string.IsNullOrWhiteSpace(ResultObjectKey))
                    SettingsManager.SetTemporaryObject(ResultObjectKey, processResult);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Install msi package";
        }
    }
}
