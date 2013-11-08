using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE9F-306E-11D1-AACF-00805FC1270E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    public interface INetCfgLock
    {
        int AcquireWriteLock([In, MarshalAs(UnmanagedType.I8)] ulong cmsTimeout,
                             [In, MarshalAs(UnmanagedType.LPWStr)] string pszwClientDescription,
                             [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszwClientDescription);
        int ReleaseWriteLock();
        int IsWriteLocked([Out, MarshalAs(UnmanagedType.LPWStr)] string ppszwClientDescription);
    };
}