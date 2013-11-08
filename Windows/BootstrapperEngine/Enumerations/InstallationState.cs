namespace Org.InCommon.InCert.BootstrapperEngine.Enumerations
{
    public enum InstallationState
    {
        Initializing,
        DetectedAbsent,
        DetectedPresent,
        DetectedNewer,
        DetectedOlder,
        Applying,
        Cancelling,
        InstallApplied,
        RepairApplied,
        RemoveApplied,
        LayoutApplied,
        HelpApplied,
        Failed,
        LaunchingNewInstall,
        LaunchingAlreadyInstalled,
        ShowingRemoveOptions,
        RunningExternal,
        AskingExternalIssueRetry,
        AskingCancel,
        AskingIssueRety,
        Unknown
    }
}