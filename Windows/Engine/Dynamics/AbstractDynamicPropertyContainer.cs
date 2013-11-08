using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Settings;

namespace Org.InCommon.InCert.Engine.Dynamics
{
    public class AbstractDynamicPropertyContainer : AbstractImportable
    {
        private readonly Dictionary<string, string> _dynamicProperties = new Dictionary<string, string>();

        public AbstractDynamicPropertyContainer(IEngine engine)
            : base(engine)
        {
        }

        protected void SetDynamicValue(string value, [CallerMemberName] string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            _dynamicProperties[key] = value;
        }

        protected string GetRawValue([CallerMemberName] string key = "")
        {
            if (string.IsNullOrWhiteSpace(key))
                return "";

            return !_dynamicProperties.ContainsKey(key)
                       ? ""
                       : _dynamicProperties[key];
        }

        protected string GetDynamicValue([CallerMemberName] string key = "", bool resolveTokens = true)
        {
            if (string.IsNullOrWhiteSpace(key))
                return "";

            return !_dynamicProperties.ContainsKey(key) ? "" :
                ResolveDynamicTokenValues(SettingsManager, _dynamicProperties[key], resolveTokens);
        }

        protected bool IsPropertySet(string key)
        {
            return _dynamicProperties.ContainsKey(key);
        }

        protected List<string> GetUnderlyingSettingKeys(string key)
        {
            var result = new List<string>();
            if (!_dynamicProperties.ContainsKey(key))
                return result;

            var baseValue = _dynamicProperties[key];
            if (string.IsNullOrWhiteSpace(baseValue))
                return result;

            var matches = Regex.Matches(baseValue, @"\[(.*?)\]");
            if (matches.Count == 0)
                return result;

            result.AddRange(
                from Match match in matches
                where match.Groups.Count >= 2
                select match.Groups[1].Value into settingKey
                where !string.IsNullOrWhiteSpace(settingKey)
                select settingKey);

            return result;
        }

        public static string ResolveDynamicTokenValues(ISettingsManager manager, string value, bool resolveSystemTokens)
        {
            if (!resolveSystemTokens)
                return ResolveDynamicValue(manager, value);

            var instance = StandardTokens.GetInstance();
            return instance == null
                ? ResolveDynamicValue(manager, value)
                : instance.ResolveTokens(ResolveDynamicValue(manager, value));
        }

        private static string ResolveDynamicValue(ISettingsManager manager, string value)
        {
            var result = value;

            if (string.IsNullOrWhiteSpace(value))
                return result;

            var matches = Regex.Matches(value, @"\[(.*?)\]");
            if (matches.Count == 0)
                return result;

            foreach (Match match in matches)
            {
                if (match.Groups.Count < 2)
                    continue;

                var matchText = match.Groups[0].Value;
                if (string.IsNullOrWhiteSpace(matchText))
                    continue;

                var key = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                result = result.Replace(matchText, manager.GetTemporarySettingString(key));
            }

            return result;

        }
    }
}
