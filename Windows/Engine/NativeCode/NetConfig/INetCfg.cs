using System;
using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE93-306E-11D1-AACF-00805FC1270E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface INetCfg
    {
        int Initialize(IntPtr pvReserved);
        int Uninitialize();
        int Apply();
        int Cancel();
        int EnumComponents(IntPtr pguidClass, [Out, MarshalAs(UnmanagedType.IUnknown)] out /*IEnumNetCfgComponent*/object ppenumComponent);
        int FindComponent([In, MarshalAs(UnmanagedType.LPWStr)]  string pszwInfId,
                          [Out, MarshalAs(UnmanagedType.IUnknown)] out /*INetCfgComponent*/ object pComponent);
        int QueryNetCfgClass([In]ref Guid pguidClass,
                             ref  Guid riid,
                             [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
    };
}