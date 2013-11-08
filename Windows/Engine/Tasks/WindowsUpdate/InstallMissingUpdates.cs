using System;
using System.Collections;
using System.Windows;
using System.Windows.Threading;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.WindowsUpdate;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.DataWrappers;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using WUApiLib;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class InstallMissingUpdates : AbstractWuApiTask
    {
        private static readonly ILog Log = Logger.Create();

        private DispatcherTimer _timer;
        private UpdateDisplayManager _currentDisplayManager;

        public InstallMissingUpdates(IEngine engine)
            : base(engine)
        {
            
        }

        [PropertyAllowedFromXml]
        public string ToInstallObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string InstalledObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string InstallProgressText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string InstallProgressTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DownloadProgressText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string DownloadProgressTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string PreInstallProgressText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string PreInstallProgressTitle
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ProgressTextOutputKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ProgressTitleOutputKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public bool UseCreepTimer { get; set; }

        [PropertyAllowedFromXml]
        public int CreepInterval { get; set; }

        [PropertyAllowedFromXml]
        public int CreepTotalPercentIncrement { get; set; }

        [PropertyAllowedFromXml]
        public int CreepUpdatePercentIncrement { get; set; }

        [PropertyAllowedFromXml]
        public int CreepLimit { get; set; }

        [PropertyAllowedFromXml]
        public bool DeferTextUpdates { get; set; }

        [PropertyAllowedFromXml]
        public bool DeferTitleUpdates { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ToInstallObjectKey))
                    throw new Exception("Results object key cannot be null");

                var updates = SettingsManager.GetTemporaryObject(ToInstallObjectKey) as UpdateCollection;
                if (updates == null)
                    throw new Exception("Could not retrieve updates collection from settings");

                if (updates.Count == 0)
                {
                    Log.Info("No updates to install");
                    return new NextResult();
                }

                PauseAutomaticUpdates();

                var session = GetUpdateSession();
                var downloadResult = DownloadUpdates(session, updates);
                if (!downloadResult.IsOk())
                    return downloadResult;

                return InstallUpdates(session, updates);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                ResumeAutomaticUpdates();
            }
        }


        private IResult DownloadUpdates(IUpdateSession3 session, UpdateCollection updates)
        {
            IDownloadJob downloadJob = null;
            try
            {
                var downloader = session.CreateUpdateDownloader();
                downloader.Updates = updates;
                downloader.Priority = DownloadPriority.dpHigh;
                downloader.IsForced = true;
                downloader.ClientApplicationID = AppearanceManager.ApplicationTitle;

                var displayManager = new UpdateDisplayManager(
                    SettingsManager, 
                    AppearanceManager, 
                    ProgressTextOutputKey, 
                    DownloadProgressText, 
                    ProgressTitleOutputKey, 
                    DownloadProgressTitle,
                    DeferTitleUpdates,
                    DeferTextUpdates);

                if (string.IsNullOrWhiteSpace(ProgressTextOutputKey))
                    displayManager = null;

                if (string.IsNullOrWhiteSpace(InstallProgressText))
                    displayManager = null;

                downloadJob = downloader.BeginDownload(displayManager, displayManager, null);
                while (!downloadJob.IsCompleted)
                {
                    System.Threading.Thread.Sleep(5);
                    Application.Current.DoEvents();

                    if (DialogsManager.CancelRequested)
                    {
                        downloadJob.RequestAbort();
                    }
                }

                var result = downloader.EndDownload(downloadJob);
                LogDownloadResults(updates, result);
                return ConvertToResult(result);

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (downloadJob != null)
                    downloadJob.CleanUp();
            }
        }

        private void SetPreInstallText()
        {
            if (string.IsNullOrWhiteSpace(ProgressTextOutputKey))
                return;

            if (string.IsNullOrWhiteSpace(PreInstallProgressText))
                return;

            SettingsManager.BindingProxy.SettingProperty = new StringSettingWrapper(ProgressTextOutputKey, PreInstallProgressText, null);
            AppearanceManager.ChangeTimedMessage(ProgressTextOutputKey, PreInstallProgressText);
        }

        private void SetPreInstallTitle()
        {
            if (string.IsNullOrWhiteSpace(ProgressTitleOutputKey))
                return;

            if (string.IsNullOrWhiteSpace(PreInstallProgressTitle))
                return;

            SettingsManager.BindingProxy.SettingProperty = new StringSettingWrapper(ProgressTitleOutputKey, PreInstallProgressTitle, null);
            AppearanceManager.ChangeTimedMessage(ProgressTitleOutputKey, PreInstallProgressTitle);
        }

        private IResult InstallUpdates(IUpdateSession3 session, UpdateCollection updates)
        {
            IInstallationJob installJob = null;
            try
            {
                SetPreInstallTitle();
                SetPreInstallText();

                var installer = session.CreateUpdateInstaller();
                installer.Updates = updates;
                installer.AllowSourcePrompts = false;
                installer.IsForced = true;
                installer.ClientApplicationID = AppearanceManager.ApplicationTitle;

                var displayManager = new UpdateDisplayManager(
                    SettingsManager, 
                    AppearanceManager, 
                    ProgressTextOutputKey,
                    InstallProgressText, 
                    ProgressTitleOutputKey, 
                    InstallProgressTitle,
                    DeferTitleUpdates,
                    DeferTextUpdates);

                if (string.IsNullOrWhiteSpace(ProgressTextOutputKey))
                    displayManager = null;

                if (string.IsNullOrWhiteSpace(InstallProgressText))
                    displayManager = null;

                installJob = installer.BeginInstall(displayManager, displayManager, null);
                SetCreepTimer(displayManager);
                while (!installJob.IsCompleted)
                {
                    System.Threading.Thread.Sleep(5);
                    Application.Current.DoEvents();

                    if (DialogsManager.CancelRequested)
                    {
                        installJob.RequestAbort();
                    }
                }

                var result = installer.EndInstall(installJob);
                SetInstalledUpdatesObject(result, updates);
                LogInstallResult(updates, result);
                return ConvertToResult(result, updates);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (installJob != null)
                    installJob.CleanUp();

                ClearCreepTimer();
            }
        }

        private void SetInstalledUpdatesObject(IInstallationResult result,UpdateCollection updates)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InstalledObjectKey))
                    return;

                var installed = new UpdateCollection();
                for (var index = 0; index < updates.Count; index++ )
                {
                    var updateResult = result.GetUpdateResult(index);
                    if (updateResult.ResultCode != OperationResultCode.orcSucceeded)
                        continue;

                    installed.Add(updates[index]);
                }

                SettingsManager.SetTemporaryObject(InstalledObjectKey, installed);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to set the installed updates object: {0}", e.Message);
            }
        }

        private void SetCreepTimer(UpdateDisplayManager updateDisplayManager)
        {

            if (!UseCreepTimer)
                return;

            if (CreepInterval < 1)
                return;

            if (CreepLimit == 0)
                CreepLimit = 100;

            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, CreepInterval)};

            _currentDisplayManager = updateDisplayManager;
            _timer.Tick += _timer_Tick;
            _timer.IsEnabled = true;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_currentDisplayManager == null)
                    return;

                _currentDisplayManager.Creep(CreepTotalPercentIncrement, CreepUpdatePercentIncrement, CreepLimit);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("An issue occurred while attempting to handler a creep timer event: {0}", ex.Message);
            }
        }

        
        private void ClearCreepTimer()
        {
            try
            {
                if (_timer == null)
                    return;

                _timer.Stop();
                _timer = null;

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while trying to clear the creep timer: {0}", e.Message);
            }

        }

        private static IResult ConvertToResult(IDownloadResult result)
        {
            if (result.ResultCode == OperationResultCode.orcAborted)
                return new UpdateOperationCancelled();

            if (result.ResultCode == OperationResultCode.orcSucceeded)
                return new NextResult();

            if (result.ResultCode == OperationResultCode.orcSucceededWithErrors)
                return new NextResult();

            return new UpdateDownloadFailed(result);
        }

        private static void LogDownloadResults(IEnumerable updates, IDownloadResult results)
        {
            try
            {
                var count = 0;
                var succeeded = 0;

                foreach (IUpdate update in updates)
                {
                    var result = results.GetUpdateResult(count);
                    if (result.ResultCode == OperationResultCode.orcSucceeded)
                    {
                        Log.DebugFormat("{0} downloaded successfully!", update.Title);
                        count++;
                        succeeded++;
                        continue;
                    }

                    var issue = new UpdateDownloadFailed(string.Format("Unable to download {0}", update.Title), result);
                    Log.Warn(issue.Issue);
                    count++;
                }

                if (count == succeeded)
                    Log.InfoFormat("{0} updates downloaded successfully", count);
                else
                    Log.WarnFormat("{0} of {1} updates downloaded successfully", succeeded, count);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while logging download results: {0}", e.Message);
            }
        }

        private void LogInstallResult(IEnumerable updates, IInstallationResult results)
        {
            try
            {
                var count = 0;
                var succeeded = 0;

                foreach (IUpdate update in updates)
                {
                    var result = results.GetUpdateResult(count);
                    if (result.ResultCode == OperationResultCode.orcSucceeded)
                    {
                        Log.DebugFormat("{0} installed successfully!", update.Title);
                        ReportInstallResult(update, "Update Installed");
                        count++;
                        succeeded++;
                        continue;
                    }

                    var issue = new UpdateInstallFailed(string.Format("Unable to download {0}", update.Title), result);
                    Log.Warn(issue.Issue);
                    count++;
                }

                if (count == succeeded)
                    Log.InfoFormat("{0} updates installed successfully", count);
                else
                    Log.WarnFormat("{0} of {1} updates installed successfully", succeeded, count);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while logging update results: {0}", e.Message);
            }
        }

        private void ReportInstallResult(IUpdate update, string reportKey)
        {
            try
            {
                var request =
                    EndpointManager.GetContract<AbstractReportingContract>(EndPointFunctions.UploadAsyncReport);
                request.Name = reportKey;
                request.Value = update.Title;
                request.MakeRequest<NoContentWrapper>();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while reporting update results: {0}", e.Message);
            }
        }

        private static IResult ConvertToResult(IInstallationResult result, IUpdateCollection updates)
        {
            if (result.ResultCode == OperationResultCode.orcAborted)
                return new UpdateOperationCancelled();

            if (result.ResultCode == OperationResultCode.orcSucceeded)
                return new NextResult();

            if (result.ResultCode == OperationResultCode.orcSucceededWithErrors)
                return new NextResult();

            if (updates.Count != 1)
                return new UpdateInstallFailed(result);

            var update = updates[0];
            return new UpdateInstallFailed(string.Format("{0} installation failed", update.Title), result.GetUpdateResult(0));
        }

        private static void PauseAutomaticUpdates()
        {
            try
            {
                var updater = new AutomaticUpdates();
                updater.Pause();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to pause automatic updates: {0}", e.Message);
            }
        }

        private static void ResumeAutomaticUpdates()
        {
            try
            {
                var updater = new AutomaticUpdates();
                updater.Resume();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to resume automatic updates: {0}", e.Message);
            }
        }

        public override string GetFriendlyName()
        {
            return "Install missing updates";
        }

        private class UpdateDisplayManager : IInstallationProgressChangedCallback, IInstallationCompletedCallback, IDownloadProgressChangedCallback, IDownloadCompletedCallback
        {
            private readonly ISettingsManager _settingsManager;
            private readonly IAppearanceManager _appearanceManager;
            private readonly string _progressTextKey;
            private readonly string _progressText;
            private readonly string _progressTitleKey;
            private readonly string _progressTitle;
            private readonly bool _deferTitleUpdates;
            private readonly bool _deferTextUpdates;

            private UpdateWrapper _lastWrapper;
            private bool _updateInProgress;

            public UpdateDisplayManager(ISettingsManager settingsManager, IAppearanceManager appearanceManager, string progressTextKey, string progressText, string progressTitleKey, string progressTitle, bool deferTitleUpdates, bool deferTextUpdates)
            {
                _settingsManager = settingsManager;
                _appearanceManager = appearanceManager;
                _progressTextKey = progressTextKey;
                _progressText = progressText;
                _progressTitleKey = progressTitleKey;
                _progressTitle = progressTitle;
                _deferTitleUpdates = deferTitleUpdates;
                _deferTextUpdates = deferTextUpdates;
            }

            public void Invoke(IInstallationJob installationJob, IInstallationProgressChangedCallbackArgs callbackArgs)
            {
                var clearProgressFlag = false;
                try
                {
                    if (_updateInProgress) return;

                    _updateInProgress = true;
                    clearProgressFlag = true;
                    var wrapper = new UpdateWrapper(installationJob, callbackArgs);
                    wrapper.AdjustValues(_lastWrapper);
                    _lastWrapper = wrapper;
                    SetTitle(wrapper);
                    SetText(wrapper);
                }
                catch (Exception e)
                {
                    Log.WarnFormat("An issue occurred while handling an install update progress changed event: {0}", e);
                }
                finally
                {
                    if (clearProgressFlag) _updateInProgress = false;
                }
            }

            public void Invoke(IInstallationJob installationJob, IInstallationCompletedCallbackArgs callbackArgs)
            {
                var clearProgressFlag = false;
                try
                {
                    if (_updateInProgress) return;

                    _updateInProgress = true;
                    clearProgressFlag = true;
                    var wrapper = new UpdateWrapper(installationJob) { TotalPercent = 100, UpdatePercent = 100, CurrentIndex = installationJob.Updates.Count};
                    wrapper.AdjustValues(_lastWrapper);
                    _lastWrapper = wrapper;
                    SetTitle(wrapper);
                    SetText(wrapper);
                }
                catch (Exception e)
                {
                    Log.WarnFormat("An issue occurred while handling an install updates completed event: {0}", e);
                }
                finally
                {
                    if (clearProgressFlag) _updateInProgress = false;
                }
            }


            public void Invoke(IDownloadJob downloadJob, IDownloadProgressChangedCallbackArgs callbackArgs)
            {
                var clearProgressFlag = false;
                try
                {
                    if (_updateInProgress) return;

                    _updateInProgress = true;
                    clearProgressFlag = true;
                    var wrapper = new UpdateWrapper(downloadJob, callbackArgs);
                    wrapper.AdjustValues(_lastWrapper);
                    _lastWrapper = wrapper;
                    SetTitle(wrapper);
                    SetText(wrapper);
                }
                catch (Exception e)
                {
                    Log.WarnFormat("An issue occurred while handling a download updates progress changed event: {0}", e);
                }
                finally
                {
                    if (clearProgressFlag) _updateInProgress = false;
                }
            }

            public void Invoke(IDownloadJob downloadJob, IDownloadCompletedCallbackArgs callbackArgs)
            {
                var clearProgressFlag = false;
                try
                {
                    if (_updateInProgress) return;

                    _updateInProgress = true;
                    clearProgressFlag = true;
                    var wrapper = new UpdateWrapper(downloadJob) { TotalPercent = 100, UpdatePercent = 100, CurrentIndex = downloadJob.Updates.Count};
                    wrapper.AdjustValues(_lastWrapper);
                    _lastWrapper = wrapper;
                    SetTitle(wrapper);
                    SetText(wrapper);
                }
                catch (Exception e)
                {
                    Log.WarnFormat("An issue occurred while An issue occurred while handling a download updates progress completed event: {0}", e);
                }
                finally
                {
                    if (clearProgressFlag) _updateInProgress = false;
                }
            }

            public void Creep(int totalPercentIncrement, int updatePercentIncrement, int creepLimit)
            {
                var clearProgressFlag = false;
                try
                {
                    if (_updateInProgress) return;

                    if (_lastWrapper == null)
                        return;

                    _updateInProgress = true;
                    clearProgressFlag = true;
                    _lastWrapper.TotalPercent = _lastWrapper.TotalPercent + totalPercentIncrement;
                    _lastWrapper.UpdatePercent = _lastWrapper.UpdatePercent + updatePercentIncrement;

                    if (_lastWrapper.TotalPercent > creepLimit)
                        _lastWrapper.TotalPercent = creepLimit;

                    if (_lastWrapper.UpdatePercent > creepLimit)
                        _lastWrapper.UpdatePercent = creepLimit;

                    SetTitle(_lastWrapper);
                    SetText(_lastWrapper);
                }
                catch (Exception e)
                {
                    Log.WarnFormat("An issue occurred while An issue occurred while handling a creep event: {0}", e);
                }
                finally
                {
                    if (clearProgressFlag) _updateInProgress = false;
                }

            }

            private void SetTitle(UpdateWrapper wrapper)
            {
                if (string.IsNullOrWhiteSpace(_progressTitle))
                    return;

                if (string.IsNullOrWhiteSpace(_progressTitleKey))
                    return;

                var updateText = ReflectionUtilities.GetObjectPropertyText(wrapper, _progressTitle);
                
                if (!_deferTitleUpdates)
                    _settingsManager.BindingProxy.SettingProperty = new StringSettingWrapper(_progressTitleKey, updateText, null);

                _appearanceManager.ChangeTimedMessage(_progressTitleKey, updateText);
            }

            private void SetText(UpdateWrapper wrapper)
            {
                if (string.IsNullOrWhiteSpace(_progressText))
                    return;

                if (string.IsNullOrWhiteSpace(_progressTextKey))
                    return;

                var updateText = ReflectionUtilities.GetObjectPropertyText(wrapper, _progressText);
                
                if (!_deferTextUpdates)
                    _settingsManager.BindingProxy.SettingProperty = new StringSettingWrapper(_progressTextKey, updateText, null);
                
                _appearanceManager.ChangeTimedMessage(_progressTextKey, updateText, false);
            }
        }

        internal class UpdateWrapper
        {
            public UpdateWrapper(IDownloadJob job, IDownloadProgressChangedCallbackArgs args)
            {
                TotalUpdates = job.Updates.Count;
                TotalPercent = args.Progress.PercentComplete;
                CurrentIndex = args.Progress.CurrentUpdateIndex+1;
                Title = job.Updates[args.Progress.CurrentUpdateIndex].Title;
                UpdatePercent = args.Progress.CurrentUpdatePercentComplete;
            }

            public UpdateWrapper(IInstallationJob job, IInstallationProgressChangedCallbackArgs args)
            {
                TotalUpdates = job.Updates.Count;
                TotalPercent = args.Progress.PercentComplete;
                CurrentIndex = args.Progress.CurrentUpdateIndex+1;
                Title = job.Updates[args.Progress.CurrentUpdateIndex].Title;
                UpdatePercent = args.Progress.CurrentUpdatePercentComplete;
            }

            public UpdateWrapper(IInstallationJob job)
            {
                TotalUpdates = job.Updates.Count;
            }

            public UpdateWrapper(IDownloadJob job)
            {
                TotalUpdates = job.Updates.Count;
            }

            public int TotalUpdates { get; set; }
            public int CurrentIndex { get; set; }
            public string Title { get; set; }
            public int TotalPercent { get; set; }
            public int UpdatePercent { get; set; }

            public void AdjustValues(UpdateWrapper lastWrapper)
            {
                if (lastWrapper == null)
                    return;

                if (lastWrapper.TotalPercent > TotalPercent)
                    TotalPercent = lastWrapper.TotalPercent;
                
                if (TotalPercent > 100)
                    TotalPercent = 100;

                if (UpdatePercent > 100)
                    UpdatePercent = 100;
            }
        }
    }
}
