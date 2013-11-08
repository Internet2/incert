using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Utility
{
    class GenerateFileWrapper:AbstractTask
    {
        internal static class NativeMethods
        {
            [DllImport("msi.dll", CharSet = CharSet.Unicode)]
            internal static extern int MsiOpenDatabase(string databasePath, IntPtr openFlags, out int dataBaseHandle);

            [DllImport("msi.dll", CharSet = CharSet.Unicode)]
            internal static extern int MsiDatabaseOpenViewW(int databaseHandle, [MarshalAs(UnmanagedType.LPWStr)] string query, out int viewHandle);

            [DllImport("msi.dll", CharSet = CharSet.Unicode)]
            internal static extern int MsiViewExecute(int viewHandle, int recordHandle);

            [DllImport("msi.dll", CharSet = CharSet.Unicode)]
            internal static extern int MsiViewFetch(int viewHandle, out int recordHandle);

            [DllImport("msi.dll", CharSet = CharSet.Unicode)]
            internal static extern int MsiRecordGetString(int recordHandle, int field,
               [Out] StringBuilder buffer, ref int bufferSize);

            [DllImport("msi.dll", ExactSpelling = true)]
            internal static extern int MsiCreateRecord(int parameters);
            
            [DllImport("msi.dll", ExactSpelling = true)]
            internal static extern int MsiCloseHandle(int handle);

            
        }
        
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string Target
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string OutputKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string OutputFolder
        {
            get { return GetDynamicValue();}
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string OutputFileName
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string Version
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public string Vital
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value);}
        }
        
        public GenerateFileWrapper(IEngine engine) : base(engine)
        {
           
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Target))
                    throw new Exception("Target path is null or empty");

                if (!File.Exists(Target))
                    return new FileNotFound {Target = Target};

                if (!string.IsNullOrWhiteSpace(OutputFolder))
                {
                    if (!Directory.Exists(OutputFolder))
                        return new FileNotFound {Target = OutputFolder};
                }

                bool vital;
                if (!bool.TryParse(Vital, out vital))
                    Log.Warn("Cannot convert vital value to valid boolean.  Assuming vital = false.");

                var version = GetVersionForFile(Target, Version);
                var info = new FileInfo(Target);

                var task = Task<string>.Factory.StartNew(() => PathUtilities.HashFile(Target));
                task.WaitUntilExited();
                if (task.IsFaulted)
                    Log.Warn("An issue occurred while attempting to generate the file hash: {0}", task.Exception);

                var wrapper = new FileInfoWrapper
                    {
                        FileName = info.Name,
                        FileUrl = info.Name,
                        Version = version.ToString(),
                        Vital = vital,
                        FileSize = info.Length,
                        FileSha1 = task.Result,
                        MsiProductCode = GetMsiPropertyFromFile(Target, "ProductCode"),
                        MsiUpgradeCode = GetMsiPropertyFromFile(Target, "UpgradeCode"),
                    };

                var serializedWrapper = wrapper.ToXElement<FileInfoWrapper>();
                // remove attributes from root
                serializedWrapper.Attributes().Remove();
                var result = new XElement("InCommonProvision", new XAttribute("version", "1.0"));
                result.Add(serializedWrapper);
                
                if (!string.IsNullOrWhiteSpace(OutputKey))
                {
                    var settingsWrapper = new StringSettingWrapper(OutputKey, result.ToString(), null);
                    SettingsManager.BindingProxy.SettingProperty = settingsWrapper;    
                }

                if (!string.IsNullOrWhiteSpace(OutputFolder))
                {
                    var outputFileName = OutputFileName;
                    if (string.IsNullOrWhiteSpace(OutputFileName))
                        outputFileName = GetInfoFilename(Target);

                    File.WriteAllText(Path.Combine(OutputFolder, outputFileName), result.ToString(), new ASCIIEncoding());
                }
                    
                return new NextResult();

            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static string GetInfoFilename(string target)
        {
            var fileName = Path.GetFileName(target);
            
            return string.Format("{0}.info.xml", fileName);
        }
        
        private static Version GetVersionForFile(string path, string version)
        {
            var result = new Version(1,0,0,0);
            if (string.IsNullOrWhiteSpace(version))
            {
                var info = FileVersionInfo.GetVersionInfo(path);
                version = info.FileVersion;
            }

            if (string.IsNullOrWhiteSpace(version))
                version = GetMsiPropertyFromFile(path, "ProductVersion");
            
            if (string.IsNullOrWhiteSpace(version))
                return result;

            if (!System.Version.TryParse(version, out result))
                Log.Warn("Cannot convert version to valid value. Will use 1.0.0.0");

            return result;
        }

        /// <summary>
        /// Gets a property value from a .msi file
        /// </summary>
        /// <param name="path">Path to .msi</param>
        /// <param name="propertyName">Name of property in question</param>
        /// <returns></returns>
        /// <remarks>Adapted from http://stackoverflow.com/questions/4347325/checking-productversion-of-an-msi-programatically</remarks>
        private static string GetMsiPropertyFromFile(string path, string propertyName)
        {
            var databaseHandle = 0;
            var viewHandle =0;
            var recordHandle = 0;
            try
            {
                if (!path.ToLowerInvariant().EndsWith(".msi"))
                    return "";

                var result = NativeMethods.MsiOpenDatabase(path, IntPtr.Zero, out databaseHandle);
                if (0 != result)
                    return "";

                var query = string.Format("SELECT * FROM Property WHERE Property = '{0}'", propertyName);
                recordHandle = NativeMethods.MsiCreateRecord(0);

                result = NativeMethods.MsiDatabaseOpenViewW(databaseHandle, query, out viewHandle);
                if (0 != result)
                    return "";

                result = NativeMethods.MsiViewExecute(viewHandle, recordHandle);
                if (0 != result)
                    return "";

                result = NativeMethods.MsiViewFetch(viewHandle, out recordHandle);
                if (0 != result)
                    return "";

                var length = 512;
                var buffer = new StringBuilder(length);
                result = NativeMethods.MsiRecordGetString(recordHandle, 2, buffer, ref length);
                return 0 != result ? "" : buffer.ToString();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to extract the product code from a file: {0}", e.Message);
                return "";
            }
            finally
            {
                if (recordHandle != 0)
                    NativeMethods.MsiCloseHandle(recordHandle);

                if (viewHandle != 0)
                    NativeMethods.MsiCloseHandle(viewHandle);
                
                if (databaseHandle !=0)
                    NativeMethods.MsiCloseHandle(databaseHandle);
            }
        }

        public override string GetFriendlyName()
        {
            var fileName = "[unknown]";
            if (!string.IsNullOrWhiteSpace(Target))
                fileName = Path.GetFileName(Target);

            return string.Format("Generate file info wrapper ({0})", fileName);
        }
    }
}
