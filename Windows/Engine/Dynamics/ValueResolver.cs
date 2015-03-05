using System.Linq;
using System.Text.RegularExpressions;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Dynamics
{
    public class ValueResolver : IValueResolver
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IStandardTokens _standardTokens;

        public ValueResolver(ISettingsManager settingsManager, IStandardTokens standardTokens)
        {
            _settingsManager = settingsManager;
            _standardTokens = standardTokens;
        }

        public string Resolve(string value, bool resolveSystemTokens)
        {
            var resolvedValue = ResolveDynamicValue(_settingsManager, value);
            return !resolveSystemTokens
                ? resolvedValue
                : _standardTokens.ResolveTokens(resolvedValue);
        }


        private static string ResolveDynamicValue(ISettingsManager manager, string value)
        {
            var result = value;

            if (string.IsNullOrWhiteSpace(value))
                return result;

            result = ResolveTextValues(manager, result);
            result = ResolveObjectValues(manager, result);

            return result;

        }

        private static string ResolveTextValues(ISettingsManager manager, string value)
        {
            var result = value;
            var matches = Regex.Matches(value, @"\[(.*?)\]");
            if (matches.Count == 0)
                return result;

            foreach (Match match in matches)
            {
                if (match.Groups.Count < 2) continue;

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

        private static string ResolveObjectValues(ISettingsManager manager, string value)
        {
            var result = value;
            var matches = Regex.Matches(value, @"\{(.*?)\}");
            if (matches.Count == 0)
                return result;

            foreach (Match match in matches)
            {
                if (match.Groups.Count < 2) continue;

                var matchText = match.Groups[0].Value;
                if (string.IsNullOrWhiteSpace(matchText))
                    continue;

                var fullKey = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(fullKey))
                    continue;

                var parts = fullKey.Split('.');
                if (parts.Length<2) continue;
                
                var instance = manager.GetTemporaryObject(parts[0]);
                if (instance == null) continue;

                var replaceWith = ReflectionUtilities.GetPropertyValue(instance, parts.Skip(1).ToArray()).ToStringOrDefault("");
                result = result.Replace(matchText,replaceWith);
            }
            return result;
        }

        
    }
}
