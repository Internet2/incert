using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Logging;
using Org.InCommon.InCert.BootstrapperEngine.WebServices;

namespace Org.InCommon.InCert.BootstrapperEngine.Models
{
    public class BaseModel
    {
        private const string BurnBundleInstallDirectoryVariable = "InstallFolder";
        private const string BurnBundleLayoutDirectoryVariable = "WixBundleLayoutDirectory";
        private const string BurnProductVariable = "ProductDisplayName";
        private const string BackgroundColorVariable = "BackgroundColor";
        private const string TextColorVariable = "TextColor";
        private const string HelpUrlVariable = "HelpUrl";
        private const string FontVariable = "Font";

        public InstallActions PlannedAction { get; set; }

        private readonly BootstrapperApplication _bootstrapper;
        //private readonly Dispatcher _dispatcher;

        public BaseModel(BootstrapperApplication bootstrapper)
        {
            _bootstrapper = bootstrapper;
        }

        /// <summary>
        /// Returns the current bootstrapper
        /// </summary>
        public BootstrapperApplication Bootstrapper
        {
            get { return _bootstrapper; }
        }

        /// <summary>
        /// Returns the command-line arguments object associated with the current bootstrapper
        /// </summary>
        public Command Command
        {
            get { return _bootstrapper.Command; }
        }

        /// <summary>
        /// Returns the engine associated with the current bootstrapper
        /// </summary>
        private Engine Engine
        {
            get { return _bootstrapper.Engine; }
        }

        /// <summary>
        /// Gets or sets the install result
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Returns the version of the bootstrapper engine
        /// </summary>
        public Version Version
        {
            get
            {
                var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                return new Version(fileVersion.FileVersion);
            }
        }

        /// <summary>
        /// Returns the folder where the installers will install their payloads
        /// </summary>
        public string InstallFolder
        {
            get
            {
                if (!StringVariables.Contains(BurnBundleInstallDirectoryVariable))
                    return "";

                var value = StringVariables[BurnBundleInstallDirectoryVariable];
                if (string.IsNullOrWhiteSpace(value))
                    return "";

                return FormatString(value);
            }
            set
            {
                StringVariables[BurnBundleInstallDirectoryVariable] = value;
            }
        }

        /// <summary>
        /// Returns the folder where the installation files will be extracted to
        /// </summary>
        public string LayoutFolder
        {
            get
            {
                return !StringVariables.Contains(BurnBundleLayoutDirectoryVariable) ? null :
                    StringVariables[BurnBundleLayoutDirectoryVariable];
            }
            set
            {
                StringVariables[BurnBundleLayoutDirectoryVariable] = value;
            }
        }

        public string ProductName
        {
            get
            {
                return !StringVariables.Contains(BurnProductVariable) ? "[Unknown]" : StringVariables[BurnProductVariable];
            }
        }

        public string BackgroundColor
        {
            get
            {
                return !StringVariables.Contains(BackgroundColorVariable)
                    ? "White"
                    : StringVariables[BackgroundColorVariable];
            }
        }

        public string TextColor
        {
            get
            {
                return !StringVariables.Contains(TextColorVariable)
                    ? "Orange"
                    : StringVariables[TextColorVariable];
            }
        }
        
        /// <summary>
        /// Starts planning the appropriate action.
        /// </summary>
        /// <param name="action">Action to plan.</param>
        public void Plan(InstallActions action)
        {
            PlannedAction = action;
            Engine.Plan(PlannedAction.Value);
        }

        public void PlanLayout()
        {
            // Either default or set the layout directory
            LayoutFolder = !String.IsNullOrEmpty(Command.LayoutDirectory) ?
                Command.LayoutDirectory : Path.GetTempPath();

            Plan(Command.Action.ToInstallAction());
        }

        public string HelpUrl
        {
            get
            {
                return !StringVariables.Contains(HelpUrlVariable)
                    ? ""
                    : StringVariables[HelpUrlVariable];
            }
        }

        public void SetLocalSource(string packageOrContainerId, string payloadId, string source)
        {
            Engine.SetLocalSource(packageOrContainerId, payloadId, source);
        }

        public void Apply(IntPtr handle)
        {
            Engine.Apply(handle);
        }

        public string FormatString(string value)
        {
            return Engine.FormatString(value);
        }

        public string FontName
        {
            get
            {
                return !StringVariables.Contains(FontVariable)
                    ? ""
                    : StringVariables[FontVariable];
            }
        }

        public Engine.Variables<string> StringVariables
        {
            get { return Engine.StringVariables; }
        }

        public string GetPackageAlias(string value, string defaultValue)
        {
            var aliasName = string.Format("{0}Alias", value);
            return !StringVariables.Contains(aliasName)
                ? defaultValue
                : StringVariables[aliasName];
        }

        public void UploadLogEntry(string eventId, string message, params object[] parameters)
        {
            try
            {
                var uploader = LogUploaderFactory.GetLogUploader(this);
                if (uploader == null)
                    return;

                uploader.UploadLog(eventId, string.Format(message, parameters));
            }
            catch (Exception e)
            {
                Logger.Error("An issue has occurred while attempting to upload a log entry: {0}", e.Message);
            }
        }
        
        public void SetInstallEntry(DetectRelatedMsiPackageEventArgs e)
        {
            if (e.Operation != RelatedOperation.MajorUpgrade)
                return;

            Logger.Standard("setting install entries for {0}", e.PackageId);
            SetInstallEntries(e.PackageId, "Updating {0}",
                "It may take a minute or two to finish updating {0}");
        }

        public void SetInstallEntry(DetectRelatedBundleEventArgs e)
        {
            if (e.RelationType != RelationType.Upgrade)
                return;

            if (e.Operation != RelatedOperation.MajorUpgrade)
                return;

            Logger.Standard("setting install entries for {0}", e.ProductCode);
            SetInstallEntries(e.ProductCode, "Completing Update",
                "It may take a minute or two to finish updating {0}");
        }

        private void SetInstallEntries(string key, string messageText, string noteText)
        {
            try
            {
                var messageVariable = GetMessageVariableName(key);
                var noteVariable = GetNoteVariableName(key);
                var alias = GetPackageAlias(key, ProductName);
                StringVariables[messageVariable] = string.Format(messageText, alias);
                StringVariables[noteVariable] = string.Format(noteText, alias);

            }
            catch (Exception e)
            {
                Logger.Error("An issue occurred trying to set install entries for key {0}: {1}", key, e.Message);
            }
        }

        private static string GetMessageVariableName(string key)
        {

            return string.Format("{0}Message", key);
        }

        private static string GetNoteVariableName(string key)
        {
            return string.Format("{0}Note", key);
        }

        public void ParseCommandLine()
        {
            // Get array of arguments based on the system parsing algorithm.
            var args = Command.GetCommandLineArgs();
            foreach (var t in args)
            {
                if (!t.StartsWith("InstallFolder=", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                // Allow relative directory paths. Also validates.
                var param = t.Split(new[] { '=' }, 2);
                InstallFolder = Path.Combine(Environment.CurrentDirectory, param[1]);
            }
        }

    }
}
