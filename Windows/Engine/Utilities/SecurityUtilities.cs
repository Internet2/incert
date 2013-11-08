using System;
using System.Runtime.InteropServices;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class SecurityUtilities
    {
        private static readonly ILog Log = Logger.Create();
        
        public enum SecurityProviderHealth
        {
            Good,
            NotMonitored,
            Poor,
            Snooze,
        }

        public enum SecurityProviders
        {
            Firewall = 1,
            AutoUpdateSettings = 2,
            AntiVirus = 4,
            AntiSpyware = 8,
            InternetSettings = 16,
            UserAccountControl = 32,
            SecurityService = 64,
            None,
            All = (Firewall
                | AutoUpdateSettings
                | AntiVirus
                | AntiSpyware
                | InternetSettings
                | UserAccountControl
                | SecurityService)
        }

        
        internal static class NativeMethods
        {
            [DllImport("Wscapi.dll", EntryPoint = "WscGetSecurityProviderHealth")]
            internal static extern int WscGetSecurityProviderHealth(SecurityProviders providers, [Out] out SecurityProviderHealth health);
        }

        public static SecurityProviderHealth QuerySecurityProviderHealth(SecurityProviders provider)
        {
            SecurityProviderHealth result;
            var queryResult = NativeMethods.WscGetSecurityProviderHealth(provider, out result);
            if (queryResult == 0)
                return result;

            // this means that the security center service is disabled
            if (queryResult == 1 && result == SecurityProviderHealth.Poor)
            {
                Log.Warn("Windows reports that the security center service is disabled"); 
                return result;
            }
                
            throw new Exception(string.Format("Unable to query {0} health: issue {1}", provider, queryResult));
        }



    }
}
