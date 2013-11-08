using System;
using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE9D-306E-11D1-AACF-00805FC1270E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface INetCfgClassSetup
    {
        int SelectAndInstall(
            [In] IntPtr hwndParent,
            [In] /*OBO_TOKEN*/IntPtr pOboToken,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out /*INetCfgComponent***/object ppnccItem);

        int Install(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszwInfId,
            [In] /*OBO_TOKEN*/IntPtr pOboToken,
            [In, MarshalAs(UnmanagedType.U4)] int dwSetupFlags,
            [In, MarshalAs(UnmanagedType.U4)] int dwUpgradeFromBuildNo,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszwAnswerFile,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszwAnswerSections,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out /*INetCfgComponent***/ object ppnccItem);

        int DeInstall(
            [Out, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgComponent**/object pComponent,
            [In] /*OBO_TOKEN*/IntPtr pOboToken,
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string pmszwRefs);
    };
}