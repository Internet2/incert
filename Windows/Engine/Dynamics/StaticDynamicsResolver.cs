using System;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings;
using log4net;

namespace Org.InCommon.InCert.Engine.Dynamics
{
    public static class StaticDynamicsResolver
    {
        private static readonly ILog Log = Logger.Create();
        
        public static string Resolve(this string value, ISettingsManager settingsManager, bool resolveTokens)
        {
            try
            {
                return string.IsNullOrWhiteSpace(value) ? value 
                    : AbstractDynamicPropertyContainer.ResolveDynamicTokenValues(settingsManager, value, resolveTokens);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to resolve the value {0}: {1}", value, e.Message);
                return value;
            }    
        }
    }
}
