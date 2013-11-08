using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    [Guid("C0E8AE98-306E-11D1-AACF-00805FC1270E"),InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    interface INetCfgSysPrep
    {
        int HrSetupSetFirstDword(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszSection,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszKey,
            [In, MarshalAs(UnmanagedType.U4)] int dwValue);

        int HrSetupSetFirstString(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszSection,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszKey,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszValue);

        int HrSetupSetFirstStringAsBool(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszSection,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszKey,
            [MarshalAs(UnmanagedType.Bool)] bool fValue);

        int HrSetupSetFirstMultiSzField(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszSection,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwszKey,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pmszValue);
    };
}