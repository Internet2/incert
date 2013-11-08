using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.RegistryTasks
{
    class GetStringValue:AbstractRegistryTask
    {
        private static readonly ILog Log = Logger.Create();
        
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        
        public GetStringValue(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                {
                    Log.Warn("Cannot set value for registry key. Settings key is null or invalid");            
                    return new NextResult();
                }

                using (var key = RegistryRoot.OpenRegistryKey(RegistryKey, false))
                {
                    if (key == null)
                        return new NextResult();

                    var value = key.GetValue(RegistryValue);
                    if (value == null)
                    {
                        Log.WarnFormat("Could not get registry value for key {0} and value {1}", RegistryKey, RegistryValue);
                        SettingsManager.RemoveTemporarySettingString(SettingKey);
                        return new NextResult();
                    }

                    SettingsManager.SetTemporarySettingString(SettingKey, value.ToString());
                    return new NextResult();
                }
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
           return "Get string value from registry";
        }
    }
}
