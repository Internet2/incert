using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class ProcessExtensions
    {
        private static readonly ILog Log = Logger.Create();

        [StructLayout(LayoutKind.Sequential)]
        public struct Luid
        {
            public uint LowPart;
            public uint HighPart;

            public override int GetHashCode()
            {
                return Marshal.SizeOf(this);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Luid))
                    return false;


                return (Luid)obj == this;
            }

            public static bool operator ==(Luid left, Luid right)
            {
                return (left.LowPart == right.LowPart) && (left.HighPart == right.HighPart);
            }

            public static bool operator !=(Luid left, Luid right)
            {
                return (left.LowPart != right.LowPart) || (left.HighPart != right.HighPart);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TokenPrivileges
        {
            public bool Equals(TokenPrivileges other)
            {
                return PrivilegeCount == other.PrivilegeCount && Attributes == other.Attributes;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Marshal.SizeOf(this));
                }
            }

            public uint PrivilegeCount;
            public Luid Luid;
            public uint Attributes;

            public override bool Equals(object obj)
            {
                if (!(obj is TokenPrivileges))
                    return false;

                return (TokenPrivileges)obj == this;
            }

            public static bool operator ==(TokenPrivileges left, TokenPrivileges right)
            {
                if (left.PrivilegeCount != right.PrivilegeCount)
                    return false;

                if (left.Attributes != right.Attributes)
                    return false;

                if (left.Luid != right.Luid)
                    return false;

                return true;
            }

            public static bool operator !=(TokenPrivileges left, TokenPrivileges right)
            {
                if (left.PrivilegeCount != right.PrivilegeCount)
                    return true;

                if (left.Attributes != right.Attributes)
                    return true;

                if (left.Luid != right.Luid)
                    return true;

                return false;
            }
        }

        internal static class NativeMethods
        {
            internal const uint TokenAdjustPrivileges = 0x20;
            internal const uint TokenQuery = 0x8;
            internal const uint PrivilegeEnabled = 0x2;

            [DllImport("advapi32.dll", EntryPoint = "OpenProcessToken", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool OpenProcessToken([In] IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

            [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValueW", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool LookupPrivilegeValueW(
                [In] [MarshalAs(UnmanagedType.LPWStr)] string systemName,
                [In] [MarshalAs(UnmanagedType.LPWStr)] string privilegeName,
                [Out] out Luid luid);
            
            [DllImport("advapi32.dll", EntryPoint = "AdjustTokenPrivileges")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AdjustTokenPrivileges(
                [In] IntPtr tokenHandle,
                [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges,
                ref TokenPrivileges newState,
                uint bufferLength,
                ref TokenPrivileges previousState,
                ref uint returnLength);

        }

        public static bool SetPrivilegeTokens(this Process process, string privilegeName, ref TokenPrivileges previousState)
        {
            try
            {
                IntPtr tokenHandle;
                if (!NativeMethods.OpenProcessToken(process.Handle,
                        NativeMethods.TokenAdjustPrivileges | NativeMethods.TokenQuery,
                        out tokenHandle))
                    throw new Win32Exception();

                Luid luid;
                if (!NativeMethods.LookupPrivilegeValueW("", privilegeName, out luid))
                    throw new Win32Exception();

                var token = new TokenPrivileges
                    {
                        PrivilegeCount = 1,
                        Luid = luid,
                        Attributes = NativeMethods.PrivilegeEnabled
                    };

                uint requiredSize = 0;
                if (!NativeMethods.AdjustTokenPrivileges(
                    tokenHandle, false, ref token, (uint)Marshal.SizeOf(token), ref previousState, ref requiredSize))
                    throw new Win32Exception();

                return true;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to set privilege tokens for a process: {0}", e);
                return false;
            }
        }

        public static bool SetPrivilegeTokens(this Process process, TokenPrivileges token)
        {
            try
            {
                IntPtr tokenHandle;
                if (!NativeMethods.OpenProcessToken(process.Handle,
                        NativeMethods.TokenAdjustPrivileges | NativeMethods.TokenQuery,
                        out tokenHandle))
                    throw new Win32Exception();

                var previousState = new TokenPrivileges();
                uint requiredSize = 0;
                if (!NativeMethods.AdjustTokenPrivileges(
                    tokenHandle, false, ref token, (uint)Marshal.SizeOf(token), ref previousState, ref requiredSize))
                    throw new Win32Exception();

                return true;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to set privilege tokens for a process: {0}", e);
                return false;
            }
        }

        public static void WaitUntilExited(this Process process)
        {
            if (process == null)
                return;

            do
            {
                process.WaitForExit(25);
                Application.Current.DoEvents();
            } while (!process.HasExited);
        }


    }
}
