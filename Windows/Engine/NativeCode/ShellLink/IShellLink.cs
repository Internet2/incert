using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Org.InCommon.InCert.Engine.NativeCode.ShellLink
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F9-0000-0000-C000-000000000046")]
    public interface IShellLink
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder filePath,
            int maxPath, ref Win32FindDataW findData, GetPathFlags flags);

        void GetIDList(ref IntPtr listPointer);
        void SetIDList(IntPtr listPointer);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int maxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder directory, int maxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string directory);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder arguments, int maxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string arguments);
        void GetHotkey(ref short hotkey);

        void SetHotkey(short hotkey);
        void GetShowCmd(ref int showCommand);

        void SetShowCmd(int showCommand);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder iconPath,
            int iconPathLength, ref int iconIndex);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string iconPath, int iconIndex);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string path, int reserved);
        void Resolve(IntPtr windowHandle, LinkResolveFlags flags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string path);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Win32FindDataW
    {
        public int dwFileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
        public int FileSizeHigh;
        public int FileSizeLow;
        public int Reserved0;
        public int Reserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string FileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string AlternateFileName;
    }

    [Flags]
    public enum GetPathFlags
    {
        ShortPath = 1,
        Uncpriority = 2,
        Rawpath = 4,
    }

    [Flags]
    public enum LinkResolveFlags
    {
        NoUi = 0x1,
        AnyMatch = 0x2,
        Update = 0x4,
        Noupdate = 0x8,
        Nosearch = 0x10,
        Notrack = 0x20,
        Nolinkinfo = 0x40,
        InvokeMsi = 0x80
    }
}