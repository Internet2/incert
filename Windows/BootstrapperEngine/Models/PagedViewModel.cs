using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.External;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.Models.Panels;
using Org.InCommon.InCert.BootstrapperEngine.PropertyNotifiers;
using Org.InCommon.InCert.BootstrapperEngine.Utility;
using Org.InCommon.InCert.BootstrapperEngine.Views;
using ErrorEventArgs = Microsoft.Tools.WindowsInstallerXml.Bootstrapper.ErrorEventArgs;

namespace Org.InCommon.InCert.BootstrapperEngine.Models
{
    public class PagedViewModel : PropertyNotifyBase
    {
        private readonly ButtonPanelModel _bottomButtonsModel;
        private readonly ContentPanelModel _contentPanelModel;

        private bool _resolvePrompt;

        public IntPtr ViewWindowHandle { get; private set; }
        public PagedView View { get; private set; }

        public BaseModel BaseModel { get; private set; }

        public string ProductName { get { return BaseModel.ProductName; } }

        public Dispatcher Dispatcher { get { return View.Dispatcher; } }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        private bool _upgrade;
        private bool _downgrade;

        public ExternalUtilityEngine ExternalEngine { get; private set; }

        public PagedViewModel(BaseModel baseModel)
        {
            BaseModel = baseModel;

            Logger.Verbose("Initializing engine window");
            View = new PagedView { DataContext = this };
            ViewWindowHandle = new WindowInteropHelper(View).EnsureHandle();

            _bottomButtonsModel = new ButtonPanelModel(this);
            _contentPanelModel = new ContentPanelModel(this);

            Logger.Verbose("Initializing engine events");
            BaseModel.Bootstrapper.DetectBegin += DetectBegin;
            BaseModel.Bootstrapper.DetectRelatedBundle += DetectedRelatedBundle;
            BaseModel.Bootstrapper.DetectComplete += DetectComplete;
            BaseModel.Bootstrapper.PlanPackageBegin += PlanPackageBegin;
            BaseModel.Bootstrapper.PlanComplete += PlanComplete;
            BaseModel.Bootstrapper.ApplyBegin += ApplyBegin;
            BaseModel.Bootstrapper.ExecuteProgress += ExecuteProgressHandler;
            BaseModel.Bootstrapper.ExecutePackageBegin += ExecutePackageBegin;
            BaseModel.Bootstrapper.ExecutePackageComplete += ExecutePackageComplete;
            BaseModel.Bootstrapper.Error += ExecuteError;
            BaseModel.Bootstrapper.ResolveSource += ResolveSource;
            BaseModel.Bootstrapper.ApplyComplete += ApplyComplete;
            BaseModel.Bootstrapper.Progress += ProgressHandler;
            BaseModel.Bootstrapper.DetectUpdateComplete += DetectUpdateCompleteHandler;
            BaseModel.Bootstrapper.DetectPriorBundle += DetectPriorBundleHandler;
            BaseModel.Bootstrapper.DetectRelatedMsiPackage += DetectRelatedMsiPackageHandler;
        }

        private InstallationState _state;

        /// <summary>
        /// Gets and sets the state of the view's model.
        /// </summary>
        public InstallationState InstallationState
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged("InstallationState"); }
        }

        public InstallationState PreApplyState { get; set; }

        /// <summary>
        /// Gets and sets whether a cancel request has been received
        /// </summary>
        public bool Cancelled { get; set; }

        public ContentPanelModel PageModel
        {
            get { return _contentPanelModel; }
        }

        public ButtonPanelModel BottomButtonsModel
        {
            get { return _bottomButtonsModel; }
        }

        public Brush Foreground
        {
            get { return BaseModel.TextColor.ToSolidColorBrush(Colors.Orange); }
        }

        public Brush Background
        {
            get { return BaseModel.BackgroundColor.ToSolidColorBrush(Colors.Teal); }
        }

        private Cursor _cursor;
        public Cursor Cursor
        {
            get { return _cursor; }
            set { _cursor = value; OnPropertyChanged("Cursor"); }
        }


        public string WindowTitle
        {
            get { return string.Format("{0} Installer", ProductName); }
        }

        public void Activate()
        {
            try
            {
                if (View == null)
                    return;

                if (View.Visibility != Visibility.Visible)
                    return;

                View.Activate();
            }
            catch (Exception e)
            {
                Logger.Error("An exception occurred while attempting to activate the view: {0}", e.Message);
            }
        }
        #region event handlers

        void DetectRelatedMsiPackageHandler(object sender, DetectRelatedMsiPackageEventArgs e)
        {
            BaseModel.SetInstallEntry(e);
            Logger.Standard("Detect related package {0} {1}", e.PackageId, e.Operation);
        }

        void DetectPriorBundleHandler(object sender, DetectPriorBundleEventArgs e)
        {
            Logger.Standard("Detect prior bundle: {0}", e.BundleId);
        }

        void DetectUpdateCompleteHandler(object sender, DetectUpdateCompleteEventArgs e)
        {
            Logger.Standard("Detect update complete handler: {0}", e.Status, e.UpdateLocation);
        }

        private void DetectBegin(object sender, DetectBeginEventArgs e)
        {
            BaseModel.PlannedAction = InstallActions.Unknown;
            if (!e.Installed)
            {
                InstallationState = InstallationState.DetectedAbsent;
                return;
            }
            
            if (BaseModel.Command.Action != LaunchAction.Uninstall)
                InstallationState = InstallationState.DetectedPresent;
        }

        private void DetectedRelatedBundle(object sender, DetectRelatedBundleEventArgs e)
        {
            Logger.Standard("Detected related bundle: {0} {1} {2} {3}", e.ProductCode, e.RelationType, e.Operation, e.BundleTag);
            if (e.Operation == RelatedOperation.Downgrade)
            {
                _downgrade = true;
                return;
            }

            if (e.Operation == RelatedOperation.MajorUpgrade)
            {
                _upgrade = true;
            }

            BaseModel.SetInstallEntry(e);
        }

        public void Close(int exitCode)
        {
            BaseModel.Result = exitCode;
            View.Close();
        }

        private void DetectComplete(object sender, DetectCompleteEventArgs e)
        {
            // Parse the command line string before any planning.
            BaseModel.ParseCommandLine();

            if (LaunchAction.Uninstall == BaseModel.Command.Action)
            {
                ConfigureUninstall();
            }
            else if (Hresult.Succeeded(e.Status))
            {

                if (_downgrade)
                {
                    Logger.Standard("downgrade detected");
                    InstallationState = InstallationState.DetectedNewer;
                }

                if (_upgrade)
                {
                    Logger.Standard("upgrade detected");
                    InstallationState = InstallationState.DetectedOlder;
                }

                if (InstallationState == InstallationState.DetectedPresent)
                {
                    ExternalEngine = new ExternalUtilityEngine(
                        this,
                        Path.Combine(BaseModel.InstallFolder, "engine.exe"))
                        {
                            AllowBack = true
                        };
                    Dispatcher.Invoke(new Action(() => ExternalEngine.Initialize(BaseModel.InstallFolder)));

                }

                if (LaunchAction.Layout == BaseModel.Command.Action)
                {
                    BaseModel.PlanLayout();
                }
                else if (BaseModel.Command.Display != Display.Full)
                {
                    // If we're not waiting for the user to click install, dispatch plan with the default action.
                    Logger.Verbose("Invoking automatic plan for non-interactive mode.");
                    BaseModel.Plan(BaseModel.Command.Action.ToInstallAction());
                }
            }
            else
            {
                InstallationState = InstallationState.Failed;
            }
        }

        private void ConfigureUninstall()
        {
            if (BaseModel.Command.Display != Display.Full)
            {
                Logger.Standard("Uninstall invoked in {0} mode; suppressing options mode (if available)", BaseModel.Command.Display);
                BaseModel.Plan(InstallActions.Remove);
                return;
            }

            ExternalEngine = new ExternalUtilityEngine(this, Path.Combine(BaseModel.InstallFolder, "engine.exe")) {AllowBack = false};
            Dispatcher.Invoke(new Action(() => ExternalEngine.Initialize(BaseModel.InstallFolder)));
            if (ExternalEngine == null || !ExternalEngine.IsValid)
            {
                Logger.Standard("No uninstall options available; proceeding with uninstall");
                BaseModel.Plan(InstallActions.Remove);
                return;
            }

            InstallationState = InstallationState.ShowingRemoveOptions;
        }

        private void ProgressHandler(object sender, ProgressEventArgs e)
        {
            if (!Cancelled) return;

            Logger.Standard("Apply Process cancelled handler - {0}", InstallationState);
            if (!AskCancel())
            {
                Cancelled = false;
                return;
            }

            RaiseCancelledIssue();
            e.Result = Result.Cancel;
        }

        private bool AskCancel()
        {
            lock (this)
            {
                if (InstallationState == InstallationState.Cancelling)
                    return true;
                
                InstallationState = InstallationState.AskingCancel;
                do
                {
                    Dispatcher.DoEvents();
                    Thread.Sleep(5);
                } while (InstallationState == InstallationState.AskingCancel);

                Logger.Standard("Installation state = {0}", InstallationState);
                return InstallationState == InstallationState.Cancelling;
            }
        }

        void ExecuteProgressHandler(object sender, ExecuteProgressEventArgs e)
        {
            if (!Cancelled) return;

            Logger.Standard("Execute Process cancelled handler - {0}", InstallationState);
            if (!AskCancel())
            {
                Cancelled = false;
                return;
            }
                
            RaiseCancelledIssue();
            e.Result = Result.Cancel;
        }

        private void RaiseCancelledIssue()
        {
            ErrorCode = 1602;
            ErrorMessage = string.Format("User cancelled {0}", BaseModel.PlannedAction.Verb.Gerundive);
            InstallationState = InstallationState.Cancelling;
        }

        private void PlanPackageBegin(object sender, PlanPackageBeginEventArgs e)
        {
            Logger.Standard("Plan package begin: {0}, {1}, {2}", e.PackageId, e.State, e.Result);

            if (BaseModel.StringVariables.Contains("MbaNetfxPackageId") && e.PackageId.Equals(BaseModel.StringVariables["MbaNetfxPackageId"], StringComparison.Ordinal))
                e.State = RequestState.None;


        }

        private void PlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (!Hresult.Succeeded(e.Status))
                InstallationState = InstallationState.Failed;

            PreApplyState = InstallationState;
            InstallationState = InstallationState.Applying;
            BaseModel.Apply(ViewWindowHandle);
        }

        private void ApplyBegin(object sender, ApplyBeginEventArgs e)
        {
            BaseModel.UploadLogEntry("Install", "Starting action {0}", BaseModel.PlannedAction);

            Cursor = Cursors.Wait;
            InstallationState = InstallationState.Applying;
        }

        private void ExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            Logger.Standard("Executing package {0}", e.PackageId);

            if (!BaseModel.PlannedAction.IsInstallAction())
                return;

            BaseModel.UploadLogEntry("Install", "Executing package {0}", e.PackageId);

        }

        private void ExecutePackageComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            if (!BaseModel.PlannedAction.IsInstallAction())
                return;

            BaseModel.UploadLogEntry("Install", "Installed package {0}", e.PackageId);

        }

        private void ExecuteError(object sender, ErrorEventArgs e)
        {
            lock (this)
            {
                try
                {
                    ErrorCode = e.ErrorCode;
                    ErrorMessage = e.ErrorMessage;

                    // if user cancelled error, return that
                    if (Cancelled)
                    {
                        e.Result = Result.Cancel;
                        ErrorCode = 1602;
                        ErrorMessage = "User cancelled operation";
                        return;
                    }

                    // If the error is a cancel coming from the engine during apply we want to go back to the preapply state.
                    if (InstallationState.Applying == InstallationState && (int)EngineErrors.UserCancelled == e.ErrorCode)
                    {
                        InstallationState = PreApplyState;
                        return;
                    }

                    // if we're not in full display mode, just return
                    if (Display.Full != BaseModel.Command.Display) return;

                    // On HTTP authentication errors, have the engine try to do authentication for us.
                    if (ErrorType.HttpServerAuthentication == e.ErrorType || ErrorType.HttpProxyAuthentication == e.ErrorType)
                    {
                        e.Result = Result.TryAgain;
                        return;
                    }

                    // otherwise, raise an error dialog
                    var msgbox = MessageBoxButton.OK;
                    switch (e.UIHint & 0xF)
                    {
                        case 0:
                            msgbox = MessageBoxButton.OK;
                            break;
                        case 1:
                            msgbox = MessageBoxButton.OKCancel;
                            break;
                        // There is no 2! That would have been MB_ABORTRETRYIGNORE.
                        case 3:
                            msgbox = MessageBoxButton.YesNoCancel;
                            break;
                        case 4:
                            msgbox = MessageBoxButton.YesNo;
                            break;
                        // default: stay with MBOK since an exact match is not available.
                    }

                    var result = MessageBoxResult.None;
                    Dispatcher.Invoke((Action)delegate
                    {
                        result = MessageBox.Show(View, e.ErrorMessage, WindowTitle, msgbox, MessageBoxImage.Error);
                    });

                    // If there was a match from the UI hint to the msgbox value, use the result from the
                    // message box. Otherwise, we'll ignore it and return the default to Burn.
                    if ((e.UIHint & 0xF) == (int)msgbox)
                    {
                        e.Result = (Result)result;
                    }
                }
                finally
                {
                    BaseModel.UploadLogEntry("InstallError", "{0} {1}", ErrorCode, ErrorMessage);
                }

            }
        }

        private void ResolveSource(object sender, ResolveSourceEventArgs e)
        {

            Logger.Standard("Resolve source request received");
            Logger.Standard("download source: {0}", e.DownloadSource);
            Logger.Standard("local source: {0}", e.LocalSource);
            Logger.Standard("package id: {0}", e.PackageOrContainerId);
            Logger.Standard("payload id: {0}", e.PayloadId);

            if (!_resolvePrompt)
            {
                Logger.Standard("Attempting to find local source");
                BaseModel.SetLocalSource(e.PackageOrContainerId, e.PayloadId, FindSource(e.LocalSource));
                _resolvePrompt = true;
                e.Result = Result.Retry;
            }
            else
            {
                e.Result = Result.Error;
            }
        }

        private string FindSource(string previousSource)
        {
            try
            {
                if (File.Exists(previousSource))
                    return previousSource;

                var directory = Path.GetDirectoryName(previousSource);
                if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                {
                    Logger.Error("Could not get directory from string {0}", previousSource);
                    return previousSource;
                }

                var originalName = Path.GetFileName(previousSource);

                foreach (var file in Directory.EnumerateFiles(directory))
                {
                    var info = FileVersionInfo.GetVersionInfo(file);
                    if (string.IsNullOrWhiteSpace(info.OriginalFilename))
                        continue;

                    if (info.OriginalFilename.Equals(originalName, StringComparison.InvariantCultureIgnoreCase))
                        return file;
                }

                Logger.Error("Could not find source for {0} in directory {1}", previousSource, directory);
                return previousSource;
            }
            catch (Exception e)
            {
                Logger.Error("An issue occurred while attempting to find the installation source: {0}", e.Message);
                return previousSource;
            }
        }

        private void ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            Cursor = Cursors.Arrow;
            BaseModel.Result = e.Status; // remember the final result of the apply.
            BaseModel.UploadLogEntry("Install", "Action {0} completed, result = {1}", BaseModel.PlannedAction, e.Status);

            // If we're not in Full UI mode, we need to alert the dispatcher to stop and close the window for passive.
            if (Display.Full != BaseModel.Command.Display)
            {
                // If its passive, send a message to the window to close.
                if (Display.Passive == BaseModel.Command.Display)
                {
                    Logger.Verbose("Automatically closing the window for non-interactive install");
                    Dispatcher.BeginInvoke((Action)(() => View.Close()));
                }
                else
                {
                    Dispatcher.InvokeShutdown();
                }
            }

            else if (Hresult.Succeeded(e.Status) && InstallActions.UpdateReplace == BaseModel.PlannedAction) // if we successfully applied an update close the window since the new Bundle should be running now.
            {
                Logger.Standard("Automatically closing the window since update successful.");
                Dispatcher.BeginInvoke((Action)(() => Close(e.Status)));
            }

            // Set the state to applied or failed unless the state has already been set back to the preapply state
            // which means we need to show the UI as it was before the apply started.
            if (InstallationState == PreApplyState) return;

            Logger.Error("Installation state ({0}) != pre-apply state ({1})", InstallationState, PreApplyState);
            if (!Hresult.Succeeded(e.Status))
            {
                InstallationState = InstallationState.Failed;
                return;
            }

            Logger.Error("Planned action = {0}", BaseModel.PlannedAction);
            InstallationState = BaseModel.PlannedAction.SuccessState;

        }
        #endregion



    }
}
