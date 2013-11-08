using System;
using System.Runtime.InteropServices;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.ScreenSaver;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.ScreenSaver
{
    internal class ConfigureScreenSaver : AbstractTask
    {
        internal static class NativeMethods
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
            internal static extern bool SystemParametersInfoSet(uint action, uint param, IntPtr vparam, uint init);

            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
            internal static extern bool SystemParametersInfoGet(uint action, uint param, ref IntPtr vparam, uint init);

            internal const uint SpiSetScreenSaveActive = 0x0011;
            internal const uint SpiGetScreenSaveActive = 0x0010;
            internal const uint SpiSetScreenSaveTimeout = 0x000F;
            internal const uint SpiGetScreenSaveTimeout = 0x000E;
            internal const uint SpiSetScreenSaveSecure = 0x0077;
            internal const uint SpiGetScreenSaveSecure = 0x0076;
        }

        public ConfigureScreenSaver(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public bool RequirePassword { get; set; }

        [PropertyAllowedFromXml]
        public int Timeout { get; set; }

        [PropertyAllowedFromXml]
        public bool Active { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (!SetScreenSaverSecurity(RequirePassword))
                    return new CouldNotConfigureScreenSaver();

                if (!SetScreenSaverActive(Active))
                    return new CouldNotConfigureScreenSaver();

                if (!SetScreenSaverTimeout(Timeout))
                    return new CouldNotConfigureScreenSaver();

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static bool SetScreenSaverSecurity(bool requirePassword)
        {
            return NativeMethods.SystemParametersInfoSet(NativeMethods.SpiSetScreenSaveSecure, Convert.ToUInt32(requirePassword), IntPtr.Zero, 0);
        }

        private static bool SetScreenSaverActive(bool active)
        {
            return NativeMethods.SystemParametersInfoSet(NativeMethods.SpiSetScreenSaveActive, Convert.ToUInt32(active), IntPtr.Zero, 0);
        }

        private static bool SetScreenSaverTimeout(int timeout)
        {
            return NativeMethods.SystemParametersInfoSet(NativeMethods.SpiSetScreenSaveTimeout, Convert.ToUInt32(timeout), IntPtr.Zero, 0);
        }

        public override string GetFriendlyName()
        {
            return "Configure screen saver";
        }
    }
}
