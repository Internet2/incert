using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE96-306E-11D1-AACF-00805FC1270E"),InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface INetCfgBindingPath
    {
        int IsSamePathAs([In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgBindingPath*/ object pPath);
        int IsSubPathOf([In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgBindingPath*/ object pPath);
        int IsEnabled();
        int Enable([MarshalAs(UnmanagedType.Bool)]bool fEnable);
        int GetPathToken([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszwPathToken);
        int GetOwner([Out, MarshalAs(UnmanagedType.IUnknown)] out /*INetCfgBindingPath*/ object ppComponent);
        int GetDepth([Out, MarshalAs(UnmanagedType.U4)] out int pcInterfaces);
        int EnumBindingInterfaces([Out, MarshalAs(UnmanagedType.IUnknown)] out /*IEnumNetCfgBindingInterface*/object ppenumInterface);
    };
}