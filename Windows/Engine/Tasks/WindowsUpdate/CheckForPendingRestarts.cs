using System;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.WindowsUpdate;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class CheckForPendingRestarts : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        private const string KeyName = "SOFTWARE\\Microsoft\\Updates\\UpdateExeVolatile";

        public CheckForPendingRestarts(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var registryResult = RestartFlaggedInRegistry();
                if (registryResult.Result)
                {
                    Log.WarnFormat("Pending restart detected: {0}", registryResult.Reason);
                    return new RestartPending();
                }

                var apiResult = RestartFlaggedInApi();
                if (apiResult.Result)
                {
                    Log.WarnFormat("Pending restart detected: {0}", registryResult.Reason);
                    return new RestartPending();
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private static BooleanReason RestartFlaggedInRegistry()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(KeyName, false))
            {
                if (key == null)
                    return new BooleanReason(false, "Registry key does not exist");

                var value = key.GetValue("flags").ToIntOrDefault(0);
                if (value != 0)
                    return new BooleanReason(true, "Registry value ({0}) present", value);
            }

            return new BooleanReason(false, "");
        }

        private static BooleanReason RestartFlaggedInApi()
        {
            try
            {
                var library = new WUApiLib.SystemInformation();
                return library.RebootRequired
                    ? new BooleanReason(true, "Restart indicated by Windows Update Api")
                    : new BooleanReason(false, "");
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to query the Windows Update api for a pending restart: {0}", e.Message);
                return new BooleanReason(false, e.Message);
            }
        }

        public override string GetFriendlyName()
        {
            return "Check for pending restarts";
        }
    }
}
