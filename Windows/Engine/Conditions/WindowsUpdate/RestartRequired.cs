using System;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Conditions.WindowsUpdate
{
    class RestartRequired : AbstractCondition
    {
        private static readonly ILog Log = Logger.Create();

        private const string KeyName = "SOFTWARE\\Microsoft\\Updates\\UpdateExeVolatile";

        public RestartRequired(IEngine engine)
            : base (engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var registryResult = RestartFlaggedInRegistry();
                if (registryResult.Result) return registryResult;

                var apiResult = RestartFlaggedInApi();
                return apiResult.Result 
                    ? apiResult 
                    : new BooleanReason(false, "no pending restarts detected");
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return true;
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
    }
}
