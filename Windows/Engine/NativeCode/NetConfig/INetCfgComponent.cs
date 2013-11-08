using System;
using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE99-306E-11D1-AACF-00805FC1270E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface INetCfgComponent
    {

        int GetDisplayName([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszwDisplayName);
        int SetDisplayName([In, MarshalAs(UnmanagedType.LPWStr)] string pszwDisplayName);
        int GetHelpText([Out, MarshalAs(UnmanagedType.LPWStr)] out string pszwHelpText);
        int GetId([Out, MarshalAs(UnmanagedType.LPWStr)]out string ppszwId);
        int GetCharacteristics([Out, MarshalAs(UnmanagedType.U4)] out int pdwCharacteristics);
        int GetInstanceGuid([Out] Guid pGuid);
        int GetPnpDevNodeId([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszwDevNodeId);
        int GetClassGuid([Out]  Guid pGuid);
        int GetBindName([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszwBindName);
        int GetDeviceStatus([Out, MarshalAs(UnmanagedType.U4)] out int pulStatus);
        int OpenParamKey([Out, MarshalAs(UnmanagedType.U4)] IntPtr phkey);
        int RaisePropertyUi([In] IntPtr hwndParent,
                            [In, MarshalAs(UnmanagedType.U4)] int dwFlags, /* NCRP_FLAGS */
                            [In, MarshalAs(UnmanagedType.IUnknown)] object punkContext);
    };
}