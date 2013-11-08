using System;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.BitLocker
{
    class DrivesEncrypted:AbstractEncryptionCondition
    {
        private static readonly ILog Log = Logger.Create();

        public DrivesEncrypted(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                using (var task = Task<bool>.Factory.StartNew(AreDrivesEncrypted))
                {
                    task.WaitUntilExited();

                    if (task.IsFaulted)
                        if (task.Exception !=null)
                            throw task.Exception;
                        else
                            throw new Exception("unknown issue");
                        
                    return !task.Result ?
                        new BooleanReason(false, "One of more drives is not encrypted and/or encryption status is not available for one or more drives")
                        : new BooleanReason(true, "All drives are encrypted");    
                }
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine is this computer's drives are encrypted: {0}", e.Message);
                return new BooleanReason(e);
            }
        }

        private static bool AreDrivesEncrypted()
        {
            var allDrivesEncrypted = true;
            
            foreach (EncryptableVolume drive in EncryptableVolume.GetInstances())
            {
                if (!IsDriveFixed(drive.DriveLetter))
                {
                    Log.InfoFormat("Drive {0} is not fixed drive. Skipping.", drive.DriveLetter);
                    continue;
                }
                
                uint protectionStatus;
                drive.GetProtectionStatus(out protectionStatus);

                if (protectionStatus == 1)
                {
                    Log.InfoFormat("Windows reports that drive {0} has a protection status of {1}", drive.DriveLetter,
                               drive.ProtectionStatus);
                    continue;
                }
                
                Log.WarnFormat("Windows reports that drive {0} has a protection status of {1}", drive.DriveLetter,
                               drive.ProtectionStatus);
                allDrivesEncrypted = false;
            }

            return allDrivesEncrypted;
        }

        

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
