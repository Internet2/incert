using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.Models;
using Org.InCommon.InCert.BootstrapperEngine.Models.Pages;
using Org.InCommon.InCert.BootstrapperEngine.Utility;

namespace Org.InCommon.InCert.BootstrapperEngine.External
{
    public class ExternalUtilityEngine
    {
        private readonly PagedViewModel _model;
        private string _installPath;

        public OptionPageModel UninstallOptions { get; private set; }
        public string IssueText { get; set; }

        public ExternalUtilityEngine(PagedViewModel model, string utilityPath)
        {
            _model = model;
            UtilityPath = utilityPath;
            PipeName = Guid.NewGuid().ToString();
        }

        public string PipeName { get; private set; }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_installPath))
                    return false;

                if (UninstallOptions == null)
                    return false;

                return UninstallOptions.IsValid;
            }
        }

        public void Initialize(string installPath)
        {
            try
            {
                _installPath = installPath;

                var cabPath = Path.Combine(installPath, "settings.cab");
                if (!File.Exists(cabPath))
                {
                    Logger.Error("Could not retrieve uninstall entries: settings.cab does not exist in folder {0}", installPath);
                    return;
                }

                var bytes = CabArchiveUtilities.ExtractFile(cabPath, "uninstall.xml");
                if (bytes == null)
                {
                    Logger.Error("Could not retrieve uninstall entries: uninstall.xml does not exist");
                    return;
                }

                using (var stream = new MemoryStream(bytes, false))
                {
                    var node = XDocument.Load(stream).Element("UninstallConfiguration");
                    if (node == null)
                    {
                        Logger.Error("Could not retrieve uninstall entries: 'UninstallConfiguration' node not present");
                        return;
                    }

                    UninstallOptions = new OptionPageModel(_model, node);
                }
            }
            catch (Exception e)
            {
                Logger.Error("An exception occurred while attempting to retrieve uninstall options: {0}", e.Message);
            }

        }

        private string UtilityPath { get; set; }
        public bool AllowBack { get; set; }

        public ExecuteResult Execute()
        {
            try
            {
                if (!IsValid)
                    return ExecuteResult.Continue;

                var selectedBranches = UninstallOptions.ChildModels.Select(e => e.SelectedValue).ToList();
                selectedBranches.RemoveAll(string.IsNullOrWhiteSpace);

                if (!selectedBranches.Any())
                    return ExecuteResult.Continue;

                if (!File.Exists(UtilityPath))
                    return ExecuteResult.Continue;

                var arguments = string.Format("-unconfigure -echo=\"initializing {0} engine\" -pipe=\"{1}\" -options=\"{2}\"",
                    _model.ProductName,
                    PipeName,
                    string.Join(",", selectedBranches));

                var exitCode = ExecuteUtilityAndWait(arguments);
                Logger.Standard("external utility returned {0}", exitCode);
                switch (exitCode)
                {
                    case 0:
                        return ExecuteResult.Continue;
                    case 2:
                        return ExecuteResult.Restart;
                    default:
                        return ExecuteResult.Prompt;
                }
                
            }
            catch (Exception e)
            {
                Logger.Error("An issue occurred while running the unconfigure engine: {0} {1}", e.Message, e.StackTrace);
                return ExecuteResult.Continue;
            }
        }

        private int ExecuteUtilityAndWait(string args)
        {
            try
            {
                Logger.Standard("Running {0} with arguments {1}", UtilityPath, args);

                _model.InstallationState = InstallationState.RunningExternal;

                var info = new ProcessStartInfo(UtilityPath)
                    {
                        UseShellExecute = true,
                        Verb = "RunAs",
                        Arguments = args
                    };
                using (var process = Process.Start(info))
                {
                    do
                    {
                        process.WaitForExit(5);
                        _model.Dispatcher.DoEvents();
                    } while (!process.HasExited);

                    Logger.Standard("{0} returned {1}", UtilityPath, process.ExitCode);
                    return process.ExitCode;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                IssueText = e.Message;
                return -1;
            }
        }




    }
}
