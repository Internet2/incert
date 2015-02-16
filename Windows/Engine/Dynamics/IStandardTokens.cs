namespace Org.InCommon.InCert.Engine.Dynamics
{
    public interface IStandardTokens
    {
        /// <summary>
        /// Replaces tokens with fvalues
        /// </summary>
        /// <param name="value">Tokenized string</param>
        /// <returns></returns>
        string ResolveTokens(string value);

        string BaseContentUrl { get; }
        string BaseDownloadUrl { get; }

        /// <summary>
        /// Returns Environment.OSVersion.VersionString for the current operating system
        /// </summary>
        string OperatingSystem { get; }

        string OperatingSystemName { get; }
        string ServicePackVersion { get; }
        string ChassisType { get; }
        string OperatingSystemCulture { get; }
        string OperatingSystemCultureSimple { get; }
        string OperatingSystemCultureIso2Letter { get; }

        /// <summary>
        /// Returns the path to the "Xml Resources" folder
        /// </summary>
        string XmlFolder { get; }

        /// <summary>
        /// Returns the path to the utility's containing folder
        /// </summary>
        string ApplicationFolder { get; }

        string ProgramFilesFolder { get; }
        string AppDataFolder { get; }
        string UtilityAppDataFolder { get; }
        string ApplicationExecutable { get; }
        string IconFolder { get; }
        string DesktopFolder { get; }
        string DesktopFolderAllUsers { get; }
        string StartupFolderAllUsers { get; }
        string StartupFolder { get; }
        string SystemFolder { get; }

        /// <summary>
        /// Returns the path to the utility's log folder
        /// </summary>
        string LogFolder { get; }

        string DownloadFolder { get; }
        string Timestamp { get; }
        string BaseDocumentsFolder { get; }

        /// <summary>
        /// Returns the path to the utility's log file
        /// </summary>
        string LogFile { get; }

        string TemporaryDirectory { get; }
        string WindowsDirectory { get; }
        string UserStartupDirectory { get; }
        string ApplicationCompany { get; }
        string StartMenuFolder { get; }
        string StartMenuFolderAllUsers { get; }
        string ProgramsFolder { get; }
        string ProgramsFolderAllUsers { get; }
        string MyDocuments { get; }
        string ApplicationTitle { get; }
        string EffectiveEngineVersion { get; }
        string ActualEngineVersion { get; }
        string LoggingSessionId { get; }
        string LoggingMachineId { get; }
        string LoggingUserId { get; }
        string ActiveAdapterDescription { get; }
        string ActiveIpAddress { get; }
        string ActiveMacAddress { get; }
        string ActiveWirelessNetwork { get; }
        string ConnectedWirelessNetworks { get; }
    }
}