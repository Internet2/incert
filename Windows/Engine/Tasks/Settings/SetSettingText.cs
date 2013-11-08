using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    public class SetSettingText : AbstractTask
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

        public SetSettingText(IEngine engine):base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            foreach (var setter in _setters)
            {
                var key = setter.Key;
                var value = setter.Value;

                if (string.IsNullOrWhiteSpace(key))
                {
                    Log.Warn("No setting key specified; cannot set value");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.WarnFormat("Cannot set value for key {0}: no value specified. To clear setting values use ClearSettingText task.", key);
                    continue;
                }

                SettingsManager.SetTemporarySettingString(key, value);
            }

            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Set settings text values";
        }
    }
}
