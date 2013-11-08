using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Org.InCommon.InCert.Utilities.GenerateFileWrapper
{
    static class NativeMethods
    {
        internal struct WinTrustFileInfo : IDisposable
        {
            public int StructSize;
            private readonly IntPtr _filePathPointer;
            // required, file name to be verified
            public IntPtr FileHandle;
            // optional, open handle to FilePath
            internal IntPtr KnownSubjectPointer;
            // optional, subject type if it is known
            public WinTrustFileInfo(String filePath)
            {
                StructSize = Marshal.SizeOf(typeof(WinTrustFileInfo));
                FileHandle = IntPtr.Zero;
                KnownSubjectPointer = IntPtr.Zero;
                _filePathPointer = Marshal.StringToCoTaskMemAuto(filePath);
            }

            public void Dispose()
            {
                Marshal.FreeCoTaskMem(_filePathPointer);
            }
        }
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal class WinTrustData : IDisposable
        {
            internal int StructSize = Marshal.SizeOf(typeof(WinTrustData));
            internal IntPtr PolicyCallbackData = IntPtr.Zero;
            internal IntPtr SIPClientData = IntPtr.Zero;

            // required: UI choice
            internal WinTrustDataUiChoice UIChoice = WinTrustDataUiChoice.None;
            // required: certificate revocation check options
            internal WinTrustDataRevocationChecks RevocationChecks = WinTrustDataRevocationChecks.None;
            // required: which structure is being passed in?
            internal WinTrustDataChoice UnionChoice = WinTrustDataChoice.File;
            // individual file
            internal IntPtr FileInfoPtr;
            internal WinTrustDataStateAction StateAction = WinTrustDataStateAction.Ignore;
            internal IntPtr StateData = IntPtr.Zero;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal String URLReference = null;
            internal WinTrustDataProvFlags ProvFlags = WinTrustDataProvFlags.SaferFlag;

            internal WinTrustDataUiContext UIContext = WinTrustDataUiContext.Execute;

            // constructor for silent WinTrustDataChoice.File check
            internal WinTrustData(String fileName, WinTrustDataRevocationChecks checks)
            {
                var wtfiData = new WinTrustFileInfo(fileName);
                FileInfoPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(WinTrustFileInfo)));
                Marshal.StructureToPtr(wtfiData, FileInfoPtr, false);
                RevocationChecks = checks;
            }

            ~WinTrustData()
            {
                Dispose();
            }

            public void Dispose()
            {
                Marshal.FreeCoTaskMem(FileInfoPtr);
            }
        }

        internal enum WinTrustDataUiChoice : uint
        {
            All = 1,
            None = 2,
            NoBad = 3,
            NoGood = 4,
        }

        internal enum WinTrustDataUiContext : uint
        {
            Execute = 0,
            Install = 1
        }

        internal enum WinTrustDataRevocationChecks : uint
        {
            None = 0,
            WholeChain = 1
        }

        internal enum WinTrustDataChoice : uint
        {
            File = 1,
            Catalog = 2,
            Blob = 3,
            Signer = 4,
            Certificate = 5
        }

        internal enum WinTrustDataStateAction : uint
        {
            Ignore = 0,
            Verify = 1,
            Close = 2,
            AutoCache = 3,
            AutoCacheFlush = 4
        }

        [Flags]
        internal enum WinTrustDataProvFlags : uint
        {
            UseIe4TrustFlag = 0x1,
            NoIe4ChainFlag = 0x2,
            NoPolicyUsageFlag = 0x4,
            RevocationCheckNone = 0x10,
            RevocationCheckEndCert = 0x20,
            RevocationCheckChain = 0x40,
            RevocationCheckChainExcludeRoot = 0x80,
            SaferFlag = 0x100,
            HashOnlyFlag = 0x200,
            UseDefaultOsverCheck = 0x400,
            LifetimeSigningFlag = 0x800,
            CacheOnlyUrlRetrieval = 0x1000
        }

        internal enum WinVerifyTrustResult : uint
        {
            Success = 0,
            ProviderUnknown = 0x800b0001, // The trust provider is not recognized on this system
            ActionUnknown = 0x800b0002, // The trust provider does not support the specified action
            SubjectFormUnknown = 0x800b0003, // The trust provider does not support the form specified for the subject
            SubjectNotTrusted = 0x800b0004 // The subject failed the specified verification action
        }

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

        [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        internal static extern WinVerifyTrustResult WinVerifyTrust(
            [In] IntPtr handle,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid actionIdentifier,
            [In] WinTrustData data);
    }
}
