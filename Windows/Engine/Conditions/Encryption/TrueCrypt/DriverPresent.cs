using System;
using Microsoft.Win32.SafeHandles;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.TrueCrypt
{
    class DriverPresent : AbstractTrueCryptCondition
    {
        private static readonly ILog Log = Logger.Create();

        public DriverPresent(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            SafeFileHandle handle = null;
            try
            {
                handle = GetDriverHandle();
                if (handle == null || handle.IsInvalid)
                    return new BooleanReason(false, "Could not retrieve valid driver handle");

                return new BooleanReason(true, "TrueCrypt driver detected.");
            }
            catch (Exception e)
            {
                Log.WarnFormat(
                    "An exception occurred while trying to determine whether the TrueCrypt driver is installed: {0}", e);
                return new BooleanReason(e);
            }
            finally
            {
                if (handle != null && !handle.IsInvalid)
                    handle.Close();
            }
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
