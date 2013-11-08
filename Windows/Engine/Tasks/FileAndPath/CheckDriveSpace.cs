using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Path;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.FileAndPath
{
    class CheckDriveSpace:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        
        public CheckDriveSpace(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
           try
           {
               var systemDrive = PathUtilities.GetSystemDrive();
               if (systemDrive == null)
               {
                   Log.Warn("Could not determine system drive; skipping drive space check");
                   return new NextResult();
               }

               var totalSpace = systemDrive.TotalSize;
               var availableSpace = systemDrive.AvailableFreeSpace;

               var percentFree = availableSpace/totalSpace;
               if (percentFree < 0.05)
               {
                   Log.Warn("Less than 5% disk space remains on computer.");
                   return new DiskSpaceLow();
               }

               var availableGigabytes = PathUtilities.BytesToGigabytes(availableSpace);
               if (availableGigabytes < 5)
               {
                   Log.Warn("Less than 5 gigabytes of space remains on this computer's system drive");
                   return new DiskSpaceLow();
               }

               return new NextResult();
           }
           catch (Exception e)
           {
               return new ExceptionOccurred(e);
           }
        }

        public override string GetFriendlyName()
        {
            return "Check drive space";
        }
    }
}
