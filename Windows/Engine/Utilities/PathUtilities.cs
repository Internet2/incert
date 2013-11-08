using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.ShellLink;
using Org.InCommon.InCert.Engine.Results.Misc;
using log4net;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class PathUtilities
    {
        private static readonly ILog Log = Logger.Create();

        /// <summary>
        /// Returns the path to the "Xml Resources" folder
        /// </summary>
        public static string XmlFolder
        {
            get { return Path.Combine(ApplicationFolder, "Xml"); }
        }
        
        /// <summary>
        /// Returns the path to the utility's containing folder
        /// </summary>
        public static string ApplicationFolder
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string AppDataFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); }
        }

        public static string UtilityAppDataFolder
        {
            get { return Path.Combine(new[] { AppDataFolder, Application.Current.GetCompanyName(), Application.Current.GetProductName() }); }
        }

        public static string ApplicationExecutable
        {
            get { return Assembly.GetExecutingAssembly().Location; }
        }

        public static string IconFolder
        {
            get { return Path.Combine(ApplicationFolder, "Icons"); }
        }

        public static string DesktopFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); }
        }

        public static string DesktopFolderAllUsers
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory); }
        }

        /// <summary>
        /// Returns the path to the utility's log folder
        /// </summary>
        public static string LogFolder
        {
            get
            {
                var targetPath = BaseDocumentsFolder;
                targetPath = Path.Combine(targetPath, "Logs");

                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                return Directory.Exists(targetPath) ? targetPath : "";
            }
        }


        public static string DownloadFolder
        {
            get
            {
                var targetPath = BaseDocumentsFolder;
                targetPath = Path.Combine(targetPath, "Downloads");
                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                return Directory.Exists(targetPath) ? targetPath : "";
            }
        }

        public static string Timestamp
        {
            get { return DateTime.UtcNow.ToString("g"); }
        }

        public static string BaseDocumentsFolder
        {
            get
            {
                var relativePath = Assembly.GetExecutingAssembly().GetName().Name;
                if (!string.IsNullOrWhiteSpace(Application.Current.GetCompanyName()) && !string.IsNullOrWhiteSpace(Application.Current.GetProductName()))
                    relativePath = Path.Combine(Application.Current.GetCompanyName(),
                                                Application.Current.GetProductName());

                return Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        relativePath);
            }
        }

        /// <summary>
        /// Returns the path to the utility's log file
        /// </summary>

        public static string LogFile
        {
            get { return Path.Combine(LogFolder, "Client.Log"); }
        }

        public static void EnsureDirectoryExists(string targetPath)
        {
            var targetFolder = Path.GetDirectoryName(targetPath);
            if (string.IsNullOrWhiteSpace(targetFolder))
                throw new Exception("Folder path cannot by empty");

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);
        }

        public static string TemporaryDirectory
        {
            get { return Path.GetTempPath(); }
        }

        public static string WindowsDirectory
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Windows); }
        }

        public static string UserStartupDirectory
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Startup); }
        }

        public static string HashFile(string targetPath)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
                return "";

            using (var inputstream = File.OpenRead(targetPath))
            {
                var hasher = new SHA1CryptoServiceProvider();
                var value = hasher.ComputeHash(inputstream);
                return value.Length == 0 ? "" : Convert.ToBase64String(value);
            }
        }

        public static string NormalizePath(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return "";

                value = value.Replace(
                    new string(Path.DirectorySeparatorChar, 2),
                    Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture));

                return value.TrimEnd(new[] { Path.DirectorySeparatorChar });
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while trying to normalize the path {0}: {1}", value, e.Message);
                return value;
            }
        }

        public static void OpenFolderPath(string value)
        {
            try
            {
                if (!Directory.Exists(value))
                {
                    Log.WarnFormat("Cannot open folder ({0}); folder does not exist.", value);
                    return;
                }

                Process.Start(value);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to open a folder ({0}): {1}", value, e.Message);
            }
        }

        public static DriveInfo GetSystemDrive()
        {
            try
            {
                var systemPath = Environment.SystemDirectory;
                var dirInfo = new DirectoryInfo(systemPath);
                var rootInfo = dirInfo.Root;

                return DriveInfo.GetDrives()
                                .FirstOrDefault(instance => instance.RootDirectory.FullName.Equals(
                                    rootInfo.FullName, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to determine the root drive: {0}", e.Message);
                return null;
            }

        }

        public static double BytesToGigabytes(long value)
        {
            return ((value / 1024.0F) / 1024.0F) / 1024.0F;
        }

        /// <summary>
        /// Adds a shortcut
        /// </summary>
        /// <param name="shortcutPath">The path where the shortcut will be created</param>
        /// <param name="sourcePath">The path to the shortcut's target</param>
        /// <param name="arguments">The command-line arguments to be associated with target</param>
        /// <param name="description">The description to be associated with the target</param>
        /// <param name="iconPath">The path to the icon to be associated with the target.  The default (0 index) icon will be used.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static BooleanReason AddShortcut(string shortcutPath, string sourcePath, string arguments, string description, string iconPath)
        {
            return AddShortcut(shortcutPath, shortcutPath, arguments, description, iconPath, 0);
        }

        /// <summary>
        /// Adds a shortcut
        /// </summary>
        /// <param name="shortcutPath">The path where the shortcut will be created</param>
        /// <param name="sourcePath">The path to the shortcut's target</param>
        /// <param name="arguments">The command-line arguments to be associated with target</param>
        /// <param name="description">The description to be associated with the target</param>
        /// <param name="iconPath">The path to the icon to be associated with the target.  The default (0 index) icon will be used.</param>
        /// <param name="iconIndex">The index of the icon to use, if the icon specified in iconpath contains more than one icon</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static BooleanReason AddShortcut(string shortcutPath, string sourcePath, string arguments, string description, string iconPath, int iconIndex)
        {
            IShellLink linker = null;
            try
            {
                var interfaceGuid = new Guid("00021401-0000-0000-C000-000000000046");
                var interfaceType = Type.GetTypeFromCLSID(interfaceGuid);
                linker = (IShellLink)Activator.CreateInstance(interfaceType);

                linker.SetPath(sourcePath);
                linker.SetDescription(description);
                linker.SetIconLocation(iconPath, iconIndex);
                linker.SetArguments(arguments);

                var saver = (IPersistFile)linker;
                saver.Save(shortcutPath, true);

                return new BooleanReason(true, "");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while trying to create a shortcut: {0}", e.Message);
            }
            finally
            {
                if ((linker != null))
                {
                    Marshal.ReleaseComObject(linker);
                }
            }
        }

        public static BooleanReason VerifyFile(this FileInfoWrapper wrapper, string targetDir)
        {
            var targetPath = Path.Combine(targetDir, wrapper.FileName);

            if (!File.Exists(targetPath))
                return new BooleanReason(false, "The file {0} does not exist", targetPath);

            if (string.IsNullOrWhiteSpace(wrapper.FileSha1))
                return new BooleanReason(true, "No hash value specified in wrapper; assuming file is valid.");

            using (var task = Task<String>.Factory.StartNew(() => HashFile(targetPath)))
            {
                task.WaitUntilExited();

                if (task.IsFaulted || task.IsCanceled)
                    return new BooleanReason(false, "Could not generate hash for file {0}: {1}", wrapper.FileName, task.GetIssueMessage("[unknown issue occurred]"));

                if (!wrapper.FileSha1.Equals(task.Result, StringComparison.InvariantCulture))
                    return new BooleanReason(false, "Hash value for file {0} ({1}) not equal to wrapper value ({2})", wrapper.FileName, task.Result, wrapper.FileSha1);
            }

            if (wrapper.FileSize == 0)
                return new BooleanReason(true, "{0} verified via hash only", wrapper.FileName);

            var info = new FileInfo(targetPath);
            if (info.Length != wrapper.FileSize)
                return new BooleanReason(true, "Size of {0} ({1}) not equal to expected size ({2})", wrapper.FileName, info.Length, wrapper.FileSize);

            return new BooleanReason(true, "Size and hash value verified for {0}", wrapper.FileName);
        }

        public static string GetSystemUtilityPath(string utilityName)
        {
            var result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), utilityName);
            return File.Exists(result) ? result : utilityName;
        }
        
    }

}
