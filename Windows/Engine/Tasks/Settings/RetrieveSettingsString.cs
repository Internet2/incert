using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class RetrieveSettingsString : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        private readonly List<KeyedDynamicStringPropertyEntry> _setters = new List<KeyedDynamicStringPropertyEntry>();

        [PropertyAllowedFromXml]
        public KeyedDynamicStringPropertyEntry Setter
        {
            set
            {
                if (value == null)
                    return;

                _setters.Add(value);
            }
        }

        public RetrieveSettingsString(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (var setter in _setters)
                {
                    var key = setter.Key;
                    var value = setter.Value;

                    if (string.IsNullOrWhiteSpace(key))
                    {
                        Log.Warn("Cannot retrieve settings value to user settings; settings key not specified");
                        continue;
                    }

                    var settingsPropertyName = value;
                    if (string.IsNullOrWhiteSpace(settingsPropertyName))
                        settingsPropertyName = key;

                    var settingsValue = Properties.Settings.Default.GetKeyedProperty(settingsPropertyName);
                    SettingsManager.SetTemporarySettingString(key, settingsValue);
                }

                return new NextResult();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new NextResult();
            }
        }

        public override string GetFriendlyName()
        {
            return "Get settings string from persisted store";
        }
    }
}
