using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Ninject;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;
using log4net.Repository.Hierarchy;
using Logger = Org.InCommon.InCert.Engine.Logging.Logger;

namespace Org.InCommon.InCert.Engine.Dynamics
{
    public class StandardTokens
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IEndpointManager _endpointManager;
        private readonly IAppearanceManager _appearanceManager;
        private static readonly ILog Log = Logger.Create();

        public StandardTokens(ISettingsManager settingsManager, IEndpointManager endpointManager, IAppearanceManager appearanceManager)
        {
            _settingsManager = settingsManager;
            _endpointManager = endpointManager;
            _appearanceManager = appearanceManager;
        }


        public static StandardTokens GetInstance()
        {
            var kernel = Application.Current.CurrentKernel();
            return kernel == null ? null : Application.Current.CurrentKernel().Get<StandardTokens>();
        }

        /// <summary>
        /// Replaces tokens with fvalues
        /// </summary>
        /// <param name="value">Tokenized string</param>
        /// <returns></returns>
        public string ResolveTokens(string value)
        {
            //@"\[(.*?)\]

            foreach (Match match in Regex.Matches(value, "(!.*?)!"))
            {
                var tokenValue = GetValueForToken(match.Value);
                value = value.Replace(match.Value, tokenValue);
            }

            return value;
        }

        private string GetValueForToken(string token)
        {
            try
            {
                var propertyName = GetPropertyForToken(token);
                if (string.IsNullOrWhiteSpace(propertyName))
                    throw new Exception(string.Format("No property found for token {0}", token));

                var property = GetType().GetProperty(propertyName);
                if (property == null)
                    throw new Exception(string.Format("Could not locate resolver for token {0}", token));

                if (!ReflectionUtilities.IsPropertyAllowedFromXml(property))
                    throw new Exception(string.Format("The token {0} references an invalid resolver", token));

                return property.GetValue(this, null).ToString();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to resolve the token {0}:{1}", token, e.Message);
                return "";
            }
        }

        private static string GetPropertyForToken(string token)
        {
            return token.Replace("!", "");
        }


        [PropertyAllowedFromXml]
        public string BaseContentUrl
        {
            get { return _endpointManager.GetEndpointForFunction(EndPointFunctions.GetContent); }
        }

        [PropertyAllowedFromXml]
        public string BaseDownloadUrl
        {
            get { return _endpointManager.GetEndpointForFunction(EndPointFunctions.GetFileInfo); }
        }

        /// <summary>
        /// Returns Environment.OSVersion.VersionString for the current operating system
        /// </summary>
        [PropertyAllowedFromXml]
        public string OperatingSystem
        {
            get { return Environment.OSVersion.VersionString; }
        }

        [PropertyAllowedFromXml]
        public string OperatingSystemName
        {
            get { return SystemUtilities.GetOperatingSystemVersionName(); }
        }

        [PropertyAllowedFromXml]
        public string ServicePackVersion
        {
            get { return SystemUtilities.GetServicePackVersion().ToString(CultureInfo.InvariantCulture); }
        }

        [PropertyAllowedFromXml]
        public string ChassisType
        {
            get { return SystemUtilities.GetChassisType(); }
        }

        [PropertyAllowedFromXml]
        public string OperatingSystemCulture
        {
            get
            {
                return string.Format("{0} ({1})",
                    CultureInfo.InstalledUICulture.EnglishName,
                    CultureInfo.InstalledUICulture.LCID.ToString(CultureInfo.InvariantCulture));
            }
        }

        [PropertyAllowedFromXml]
        public string OperatingSystemCultureSimple
        {
            get
            {
                var englishName = CultureInfo.InstalledUICulture.EnglishName;
                if (string.IsNullOrWhiteSpace(englishName))
                    return "unknown";

                var parts = englishName.Split(' ');
                return !parts.Any() ? englishName : parts[0];
            }
        }

        [PropertyAllowedFromXml]
        public string OperatingSystemCultureIso2Letter
        {
            get { return CultureInfo.InstalledUICulture.TwoLetterISOLanguageName; }
        }

        /// <summary>
        /// Returns the path to the "Xml Resources" folder
        /// </summary>
        [PropertyAllowedFromXml]
        public string XmlFolder
        {
            get { return PathUtilities.XmlFolder; }
        }

        /// <summary>
        /// Returns the path to the utility's containing folder
        /// </summary>
        [PropertyAllowedFromXml]
        public string ApplicationFolder
        {
            get { return PathUtilities.ApplicationFolder; }
        }

        [PropertyAllowedFromXml]
        public string ProgramFilesFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86); }
        }

        [PropertyAllowedFromXml]
        public string AppDataFolder
        {
            get { return PathUtilities.AppDataFolder; }
        }

        [PropertyAllowedFromXml]
        public string UtilityAppDataFolder
        {
            get { return PathUtilities.UtilityAppDataFolder; }
        }

        [PropertyAllowedFromXml]
        public string ApplicationExecutable
        {
            get { return PathUtilities.ApplicationExecutable; }
        }

        [PropertyAllowedFromXml]
        public string IconFolder
        {
            get { return PathUtilities.IconFolder; }
        }

        [PropertyAllowedFromXml]
        public string DesktopFolder
        {
            get { return PathUtilities.DesktopFolder; }
        }

        [PropertyAllowedFromXml]
        public string DesktopFolderAllUsers
        {
            get { return PathUtilities.DesktopFolderAllUsers; }
        }

        [PropertyAllowedFromXml]
        public string StartupFolderAllUsers
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup); }
        }

        [PropertyAllowedFromXml]
        public string StartupFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Startup); }
        }

        [PropertyAllowedFromXml]
        public string SystemFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.System); }
        }

        /// <summary>
        /// Returns the path to the utility's log folder
        /// </summary>
        [PropertyAllowedFromXml]
        public string LogFolder
        {
            get { return PathUtilities.LogFolder; }
        }

        [PropertyAllowedFromXml]
        public string DownloadFolder
        {
            get { return PathUtilities.DownloadFolder; }
        }

        [PropertyAllowedFromXml]
        public string Timestamp
        {
            get { return DateTime.UtcNow.ToString("g"); }
        }

        [PropertyAllowedFromXml]
        public string BaseDocumentsFolder
        {
            get { return PathUtilities.BaseDocumentsFolder; }
        }

        /// <summary>
        /// Returns the path to the utility's log file
        /// </summary>
        [PropertyAllowedFromXml]
        public string LogFile
        {
            get { return PathUtilities.LogFile; }
        }

        [PropertyAllowedFromXml]
        public string TemporaryDirectory
        {
            get { return PathUtilities.TemporaryDirectory; }
        }

        [PropertyAllowedFromXml]
        public string WindowsDirectory
        {
            get { return PathUtilities.WindowsDirectory; }
        }

        [PropertyAllowedFromXml]
        public string UserStartupDirectory
        {
            get { return PathUtilities.UserStartupDirectory; }
        }

        [PropertyAllowedFromXml]
        public string ApplicationCompany
        {
            get
            {
                return _appearanceManager.ApplicationCompany;
            }
        }

        [PropertyAllowedFromXml]
        public string StartMenuFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.StartMenu); }
        }

        [PropertyAllowedFromXml]
        public string StartMenuFolderAllUsers
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu); }
        }

        [PropertyAllowedFromXml]
        public string ProgramsFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Programs); }
        }

        [PropertyAllowedFromXml]
        public string ProgramsFolderAllUsers
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms); }
        }


        [PropertyAllowedFromXml]
        public string ApplicationTitle
        {
            get
            {
                return _appearanceManager.ApplicationTitle;
            }
        }

        [PropertyAllowedFromXml]
        public string EffectiveEngineVersion
        {
            get { return _settingsManager.EffectiveEngineVersion.ToString(); }
        }

        [PropertyAllowedFromXml]
        public string ActualEngineVersion
        {
            get { return Application.Current.GetVersion().ToString(); }
        }

        [PropertyAllowedFromXml]
        public string LoggingSessionId
        {
            get { return Logger.SessionId; }
        }

        [PropertyAllowedFromXml]
        public string LoggingMachineId
        {
            get
            {
                var hierarchy = (Hierarchy)LogManager.GetRepository();
                var appender = hierarchy.Root.Appenders.OfType<WebServiceAppender>()
                                        .FirstOrDefault();
                if (appender == null)
                    return "[unknown]";

                return appender.MachineId <= 0
                    ? "[unknown]"
                    : appender.MachineId.ToString(CultureInfo.InvariantCulture);
            }
        }

        [PropertyAllowedFromXml]
        public string LoggingUserId
        {
            get { return ThreadContext.Properties["UserId"].ToStringOrDefault("[unknown]").ToLowerInvariant(); }
        }

        [PropertyAllowedFromXml]
        public string ActiveAdapterDescription
        {
            get
            {
                var adapter = NetworkUtilities.GetPrimaryAdapter(_endpointManager);
                return adapter == null ? "[unknown]" : adapter.Description;
            }
        }

        [PropertyAllowedFromXml]
        public string ActiveIpAddress
        {
            get { return NetworkUtilities.GetActiveIpAddress(_endpointManager).ToStringOrDefault(""); }
        }

        [PropertyAllowedFromXml]
        public string ActiveMacAddress
        {
            get
            {
                var address = NetworkUtilities.GetActiveMacAddress(_endpointManager);
                return address.ToDelimitedText(":").ToUpperInvariant();
            }
        }

        [PropertyAllowedFromXml]
        public string ActiveWirelessNetwork
        {
            get
            {
                var adapter = NetworkUtilities.GetPrimaryAdapter(_endpointManager);
                if (adapter == null)
                    return "";

                return !NetworkUtilities.IsAdapterWireless(adapter) ? ""
                    : adapter.GetConnectedWirelessNetwork().ToStringOrDefault("");
            }
        }

        [PropertyAllowedFromXml]
        public string ConnectedWirelessNetworks
        {
            get
            {
                var results = new List<string>();
                foreach (var network in NetworkUtilities.GetWirelessAdapters()
                    .Select(instance => instance.GetConnectedWirelessNetwork())
                    .Where(network => !string.IsNullOrWhiteSpace(network))
                    .Where(network => !results.Contains(network)))
                {
                    results.Add(network);
                }

                return !results.Any() ? "" : string.Join(",", results);
            }
        }
    }
}

