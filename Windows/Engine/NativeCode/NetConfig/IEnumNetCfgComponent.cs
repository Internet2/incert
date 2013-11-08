using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE92-306E-11D1-AACF-00805FC1270E"),InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface IEnumNetCfgComponent
    {
        int Next([In, MarshalAs(UnmanagedType.U4)] int celt,
                 [Out, MarshalAs(UnmanagedType.IUnknown)] out object rgelt,
                 [Out, MarshalAs(UnmanagedType.U4)] out int pceltFetched);
        int Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
        int Reset();
        int Clone([Out, MarshalAs(UnmanagedType.IUnknown)] out /*IEnumNetCfgComponent*/object ppenum);
    };
}