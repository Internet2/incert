using System;
using System.IO;
using System.Runtime.InteropServices;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.FileAndPath
{
    class SetFolderIcon : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public SetFolderIcon(IEngine engine)
            : base(engine)
        {
        }

        internal static class NativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool WritePrivateProfileString(string appName,
               string keyName, string text, string fileName);

    
        }

        [PropertyAllowedFromXml]
        public string TargetFolder
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string IconPath
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Description
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!Directory.Exists(TargetFolder))
                    return new DirectoryNotFound { Target = TargetFolder };

                if (!File.Exists(IconPath))
                    return new FileNotFound { Target = IconPath };

                var iniPath = Path.Combine(TargetFolder, "desktop.ini");
                NativeMethods.WritePrivateProfileString(".ShellClassInfo", "IconFile", IconPath, iniPath);
                NativeMethods.WritePrivateProfileString(".ShellClassInfo", "IconIndex", "0", iniPath);
                NativeMethods.WritePrivateProfileString(".ShellClassInfo", "InfoTip", Description, iniPath);
                NativeMethods.WritePrivateProfileString(".ShellClassInfo", "IconResource",
                                                        string.Format("{0},0", IconPath), iniPath);
                if (!File.Exists(iniPath))
                {
                    Log.WarnFormat("Could add icon to folder {0}", TargetFolder);
                    return new NextResult();
                }

                var folderInfo = new DirectoryInfo(TargetFolder);
                folderInfo.Attributes = folderInfo.Attributes | FileAttributes.System;

                var iniInfo = new FileInfo(iniPath);
                iniInfo.Attributes = iniInfo.Attributes | FileAttributes.Hidden | FileAttributes.System;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set folder icon";
        }
    }
}
