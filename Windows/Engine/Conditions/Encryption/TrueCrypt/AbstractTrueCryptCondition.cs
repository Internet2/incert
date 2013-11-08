using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.TrueCrypt
{
    abstract class AbstractTrueCryptCondition : AbstractEncryptionCondition
    {
        private static readonly ILog Log = Logger.Create();

        static internal class NativeMethods
        {
            internal enum TrueCryptControlCodes
            {
                GetMountedVoumes = 2236440,
                GetStatus = 2236488
            }
            
            internal const uint FileShareRead = 0x00000001;
            internal const uint FileShareWrite = 0x00000002;
            internal const uint OpenExisting = 3;

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            static internal extern SafeFileHandle CreateFile(
                string lpFileName,
                [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess,
                [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
                IntPtr lpSecuritys,
                [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
                [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAnds,
                IntPtr hTemplateFile);

            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool DeviceIoControl(
                SafeFileHandle hDevice,
                TrueCryptControlCodes controlCode,
                [MarshalAs(UnmanagedType.AsAny)]
                [In] object input,
                int inputSize,
                [MarshalAs(UnmanagedType.AsAny)]
                [Out] object output,
                int outputSize,
                ref int bytesReturned,
                IntPtr overlapped);
        }

        private const string DriverIdentifier = "\\\\.\\TrueCrypt";

        protected AbstractTrueCryptCondition(IEngine engine)
            : base (engine)
        {
        }

        protected enum BootEncryptionSetupMode
        {
            SetupNone = 0,
            SetupEncryption,
            SetupDecryption
        }

        [StructLayout(LayoutKind.Explicit, Size = 8)]
        internal struct LargeInteger
        {
            [FieldOffset(0)]
            public readonly Int64 QuadPart;
            [FieldOffset(0)]
            private readonly UInt32 LowPart;
            [FieldOffset(4)]
            private readonly Int32 HighPart;
        }

        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6760)]

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct VolumeName
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string Value;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct MountedVolumesList
        {
            public uint MountedDrives;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26, ArraySubType = UnmanagedType.Struct)]
            public VolumeName[] Volumes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26, ArraySubType = UnmanagedType.U8)]
            public ulong[] DiskLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26, ArraySubType = UnmanagedType.I4)]
            public int[] ea;
            /// int[26]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26, ArraySubType = UnmanagedType.I4)]
            public int[] volumeType;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct DriverStatus
        {
            // New fields must be added at the end of the structure to maintain compatibility with previous versions
            public int DeviceFilterActive;
            public Int16 BootLoaderVersion;
            public int DriveMounted;
            public int VolumeHeaderPresent;
            public int DriveEncrypted;
            public readonly LargeInteger BootDriveLength;
            public Int64 ConfiguredEncryptedAreaStart;
            public Int64 ConfiguredEncryptedAreaEnd;
            public Int64 EncryptedAreaStart;
            public Int64 EncryptedAreaEnd;
            public uint VolumeHeaderSaltCrc32;
            public int SetupInProgress;
            public BootEncryptionSetupMode SetupMode;
            public int TransformWaitingForIdle;
            public uint HibernationPreventionCount;
            public int HiddenSystem;
            public Int64 HiddenSystemPartitionStart;
            public uint HiddenSysLeakProtectionCount;
        }
        
        internal const int BootLoaderAreaSize = 32256;

        protected SafeFileHandle GetDriverHandle()
        {
            try
            {
                var result = NativeMethods.CreateFile(
                    DriverIdentifier,
                    0,
                    FileShare.ReadWrite,
                    IntPtr.Zero,
                    FileMode.Open,
                    0,
                    IntPtr.Zero);

                if (result.IsInvalid)
                    throw new Win32Exception();

                return result;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to get a handle to the TrueCrypt driver: {0}", e.Message);
                return null;
            }
        }

        private static T ByteArrayToStruct<T>(byte[] value) where T:new()
        {
            var memoryPointer = IntPtr.Zero;
            try
            {
                var size = Marshal.SizeOf(typeof(T));
                if (value.Length != size)
                    throw new Exception("input array is incorrect size");

                memoryPointer = Marshal.AllocHGlobal(size);
                Marshal.Copy(value, 0, memoryPointer, size);
                return (T)Marshal.PtrToStructure(memoryPointer, typeof(T));

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue has occurred while attempting to copy the contents of a byte array to a structure of type {0}: {1}", typeof(T), e.Message);
                return default(T);
            }
            finally
            {
                if (memoryPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(memoryPointer);
            }
        }

        protected DriverStatus? GetStatus()
        {
            SafeFileHandle handle = null;
            try
            {
                handle = GetDriverHandle();
                if (handle == null || handle.IsInvalid)
                    throw new Exception("Could not retrieve valid handle.");

                var size = Marshal.SizeOf(typeof(DriverStatus));
                var bytes = new byte[size];

                var bytesReturned= 0;
                if (
                    !NativeMethods.DeviceIoControl(
                        handle, 
                        NativeMethods.TrueCryptControlCodes.GetStatus, 
                        null, 
                        0, 
                        bytes, 
                        size, 
                        ref bytesReturned,
                        IntPtr.Zero))
                {
                    throw new Win32Exception();
                }

                return ByteArrayToStruct<DriverStatus>(bytes);
            }
            catch (Exception e)
            {
                Log.WarnFormat(
                    "An exception occurred while attempting to get a handle to the TrueCrypt driver status: {0}",
                    e.Message);
                return null;
            }
            finally
            {
                if (handle !=null && !handle.IsInvalid)
                    handle.Close();
            }
        }
    }
}
