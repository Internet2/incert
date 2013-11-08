using System.Runtime.InteropServices;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace Org.InCommon.InCert.Engine.NativeCode.InternetSecurityManager
{

    public enum OperationFlags
    {
        Create = 0,
        Delete = 1
    }

    public enum TargetZone
    {
        LocalMachine = 0,
        Intranet = 1,
        Trusted = 2,
        Internet = 3,
        Untrusted = 4
    }

    [ComImport, Guid("79EAC9EE-BAF9-11CE-8C82-00AA004BA90B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IInternetSecurityManager
    {
        [PreserveSig]
        int SetSecuritySite([In] IntPtr site);

        [PreserveSig]
        int GetSecuritySite([Out] IntPtr site);

        [PreserveSig]
        int MapUrlToZone(
            [In, MarshalAs(UnmanagedType.LPWStr)] string url,
            ref TargetZone zone,
            OperationFlags flags);

        [PreserveSig]
        int GetSecurityId(
            [MarshalAs(UnmanagedType.LPWStr)] string url,
            [MarshalAs(UnmanagedType.LPArray)] byte[] securityIdentifier,
            ref UInt32 securityIdentifierSize,
            uint reserved);

        [PreserveSig]
        int ProcessUrlAction([
            In,
            MarshalAs(UnmanagedType.LPWStr)] string url,
            UInt32 action,
            ref byte policy,
            UInt32 policySize,
            byte context,
            UInt32 contextSize,
            UInt32 flags,
            UInt32 reserved);

        [PreserveSig]
        int QueryCustomPolicy(
            [In, MarshalAs(UnmanagedType.LPWStr)] string url,
            ref Guid guid,
            ref byte policy,
            ref UInt32 policySize,
            ref byte context,
            UInt32 contextSize,
            UInt32 reserved);

        [PreserveSig]
        int SetZoneMapping(
            TargetZone zone,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pattern,
            OperationFlags flags);

        [PreserveSig]
        int GetZoneMappings(
            TargetZone zone,
            ref IEnumString enumString,
            OperationFlags flags);
    }
}