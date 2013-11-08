using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE9E-306E-11D1-AACF-00805FC1270E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface INetCfgComponentBindings
    {
        int BindTo(
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgComponent**/object pnccItem);

        int UnbindFrom(
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgComponent**/object pnccItem);

        int SupportsBindingInterface(
            [In, MarshalAs(UnmanagedType.U4)] int dwFlags,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszwInterfaceName);

        int IsBoundTo(
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgComponent**/object pnccItem);

        int IsBindableTo(
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgComponent**/object pnccItem);

        int EnumBindingPaths(
            [In, MarshalAs(UnmanagedType.U4)] int dwFlags,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out /*IEnumNetCfgBindingPath***/object ppIEnum);

        int MoveBefore(
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgBindingPath**/ object pncbItemSrc,
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgBindingPath**/ object pncbItemDest);

        int MoveAfter(
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgBindingPath**/ object pncbItemSrc,
            [In, MarshalAs(UnmanagedType.IUnknown)] /*INetCfgBindingPath**/ object pncbItemDest);
    };
}