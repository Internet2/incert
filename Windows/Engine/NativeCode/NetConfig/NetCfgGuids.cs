using System;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    public static class NetCfgGuids
    {
        public static Guid ClsidCNetCfg = new Guid("5B035261-40F9-11D1-AAEC-00805FC1270E");
        public static Guid IidINetCfg = new Guid("C0E8AE93-306E-11D1-AACF-00805FC1270E");
        public static Guid IidDevClassNet = new Guid(0x4d36e972, 0xe325, 0x11ce, 0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18);
        public static Guid IidINetCfgClass = new Guid("C0E8AE97-306E-11D1-AACF-00805FC1270E");
    }
}