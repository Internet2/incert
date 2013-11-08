using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE97-306E-11D1-AACF-00805FC1270E"),InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface INetCfgClass
    {
        int FindComponent([In, MarshalAs(UnmanagedType.LPWStr)] string pszwInfId,
                          [Out, MarshalAs(UnmanagedType.IUnknown)] out /*INetCfgComponent*/object ppnccItem);
        int EnumComponents([Out, MarshalAs(UnmanagedType.IUnknown)] out /*IEnumNetCfgComponent*/object ppenumComponent);
    };
}