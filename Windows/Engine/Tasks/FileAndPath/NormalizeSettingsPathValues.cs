using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.FileAndPath
{
    class NormalizeSettingsPathValues : AbstractTask
    {
        private readonly List<string> _keys;

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            set {_keys.Add(value);}
        }

        public NormalizeSettingsPathValues(IEngine engine)
            : base(engine)
        {
            _keys = new List<string>();
        }

        public override IResult Execute(IResult previousResults)
        {
            foreach (var key in _keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                if (!SettingsManager.IsTemporarySettingStringPresent(key))
                    continue;

                var value = SettingsManager.GetTemporarySettingString(key);
                value = PathUtilities.NormalizePath(value);
                SettingsManager.SetTemporarySettingString(key, value);

            }

            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Normalize settings path values";
        }
    }
}
