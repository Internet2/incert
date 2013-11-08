using System;
using System.Collections.Generic;
using System.ComponentModel;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.Handlers;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.PropertyNotifiers;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Panels
{
    public abstract class AbstractPanelModel : PropertyNotifyBase
    {
        private readonly InstallationStateHandler _stateHandler;

        protected PagedViewModel Model { get; private set; }

        protected AbstractPanelModel(PagedViewModel model)
        {
            Model = model;

            _stateHandler = new InstallationStateHandler(
                Model.Dispatcher,
                new Dictionary<InstallationState, Action>
                    {
                        {InstallationState.Applying, ConfigureApplying},
                        {InstallationState.InstallApplied, ConfigureInstallApplied},
                        {InstallationState.RemoveApplied, ConfigureRemoveApplied},
                        {InstallationState.RepairApplied, ConfigureRepairApplied},
                        {InstallationState.DetectedPresent, ConfigureStart},
                        {InstallationState.DetectedAbsent, ConfigureInstall},
                        {InstallationState.DetectedNewer,  ConfigureNewer},
                        {InstallationState.DetectedOlder, ConfigureOlder},
                        {InstallationState.Initializing, ConfigureInitializing},
                        {InstallationState.Failed, ConfigureFailed},
                        {InstallationState.LaunchingNewInstall, ConfigureLaunchingNew},
                        {InstallationState.LaunchingAlreadyInstalled, ConfigureLaunchingCurrent},
                        {InstallationState.Cancelling, Cancelling},
                        {InstallationState.ShowingRemoveOptions, ConfigureShowRemoveOptions},
                        {InstallationState.RunningExternal, ConfigureRunningExternal},
                        {InstallationState.AskingExternalIssueRetry, ConfigureAskExternalRetry},
                        {InstallationState.AskingCancel, ConfigureAskCancel}
                     });

            Model.PropertyChanged += RootPropertyChanged;
        }

        private void RootPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (!e.PropertyName.Equals("InstallationState", StringComparison.InvariantCulture))
                    return;

                _stateHandler.HandleState(Model.InstallationState);
            }
            catch (Exception ex)
            {
                Logger.Error("An exception occurred in an installation state handler: {0}", ex.Message);
            }
        }

        protected abstract void ConfigureInstall();
        protected abstract void ConfigureStart();
        protected abstract void ConfigureApplying();
        protected abstract void ConfigureInstallApplied();
        protected abstract void ConfigureRepairApplied();
        protected abstract void ConfigureRemoveApplied();
        protected abstract void ConfigureInitializing();
        protected abstract void ConfigureFailed();
        protected abstract void ConfigureNewer();
        protected abstract void ConfigureOlder();
        protected abstract void ConfigureLaunchingNew();
        protected abstract void ConfigureLaunchingCurrent();
        protected abstract void Cancelling();
        protected abstract void ConfigureShowRemoveOptions();
        protected abstract void ConfigureRunningExternal();
        protected abstract void ConfigureAskExternalRetry();
        protected abstract void ConfigureAskCancel();
    }
}
