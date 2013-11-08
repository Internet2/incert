using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE94-306E-11D1-AACF-00805FC1270E"),InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface INetCfgBindingInterface
    {
        int GetName([Out, MarshalAs(UnmanagedType.LPWStr)] string ppszwInterfaceName);
        int GetUpperComponent([Out, MarshalAs(UnmanagedType.IUnknown)] out /*INetCfgComponent*/object ppnccItem);
        int GetLowerComponent([Out, MarshalAs(UnmanagedType.IUnknown)] out /*INetCfgComponent*/object ppnccItem);
    };
}