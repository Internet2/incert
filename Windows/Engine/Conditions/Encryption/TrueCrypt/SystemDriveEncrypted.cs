using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.TrueCrypt
{
    class SystemDriveEncrypted : AbstractTrueCryptCondition
    {
        private static readonly ILog Log = Logger.Create();

        public SystemDriveEncrypted(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var status = GetStatus();
                if (!status.HasValue)
                    return new BooleanReason(false, "Could not get driver status");
    
                var wholeDriveEncrypted = (
                    status.Value.ConfiguredEncryptedAreaStart == BootLoaderAreaSize
                    && status.Value.ConfiguredEncryptedAreaEnd >= status.Value.BootDriveLength.QuadPart - 1);

                if (!wholeDriveEncrypted)
                {
                    Log.Warn("This computer's system drive is not whole drive encrypted");
                    return new BooleanReason(false, "This computer's system drive is not whole drive encrypted");
                }

                var systemDriveOrPartitionFullyEncrypted = (
                    status.Value.SetupInProgress != 1
                    && status.Value.ConfiguredEncryptedAreaEnd != 0
                    && status.Value.ConfiguredEncryptedAreaEnd != -1
                    && status.Value.ConfiguredEncryptedAreaStart == status.Value.EncryptedAreaStart
                    && status.Value.ConfiguredEncryptedAreaEnd == status.Value.EncryptedAreaEnd);

                if (!systemDriveOrPartitionFullyEncrypted)
                {
                    Log.Warn("This computer's system drive or partition is not fully encrypted");
                    return new BooleanReason(false, "This computer's system drive or partition is not fully encrypted");
                }
                return new BooleanReason(true, "");
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine whether this computer's system drive is encrypted with TrueCrypt: {0}", e.Message);
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
