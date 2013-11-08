using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings.SerializableDictionaries;
using log4net;

namespace Org.InCommon.InCert.Engine.Extensions
{
    internal static class KeyedSettingsPropertyExtensions
    {
        private static readonly ILog Log = Logger.Create();
        private const string DefaultStringStore = "PersistedProperties";
        private const string DefaultTimestampStore = "PersistedTimestamps";

        public static string GetKeyedProperty(this Properties.Settings settings, string key)
        {
            try
            {
                if (!PropertyKeyExists(settings.Properties, DefaultStringStore))
                {
                    Log.WarnFormat("The persistable property store {0} does not exist", DefaultStringStore);
                    return "";
                }

                var value = settings[DefaultStringStore];
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                    return "";

                var entries = SerializableStringDictionary.LoadFromString(value.ToString());
                if (entries == null)
                {
                    Log.WarnFormat("Could not deserialize store {0}", DefaultStringStore);
                    return "";
                }

                return !entries.ContainsKey(key) ? "" : entries[key];
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return "";
            }
        }

        public static DateTime? GetKeyedTimestamp(this Properties.Settings settings, string key)
        {
            try
            {
                if (!PropertyKeyExists(settings.Properties, DefaultTimestampStore))
                {
                    Log.WarnFormat("The persistable timestamp store {0} does not exist", DefaultTimestampStore);
                    return null;
                }

                var value = settings[DefaultTimestampStore];
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                    return null;

                var entries = SerializableTimestampDictionary.LoadFromString(value.ToString());
                if (entries == null)
                {
                    Log.WarnFormat("Could not deserialize store {0}", DefaultTimestampStore);
                    return null;
                }

                if (!entries.ContainsKey(key))
                    return null;

                return entries[key];
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static void SaveKeyedProperty(this Properties.Settings settings, string key, string value)
        {
            try
            {
                if (!PropertyKeyExists(settings.Properties, DefaultStringStore))
                {
                    Log.WarnFormat("The persistable property store {0} does not exist", DefaultStringStore);
                    return;
                }

                var rawText = "";
                var rawEntry = settings[DefaultStringStore];
                if (rawEntry != null)
                    rawText = rawEntry.ToString();

                var entries = SerializableStringDictionary.LoadFromString(rawText);
                if (entries == null)
                {
                    Log.WarnFormat("Could not deserialize store {0}", DefaultStringStore);
                    return;
                }

                entries[key] = value;

                settings[DefaultStringStore] = entries.ToString();
                settings.Save();

            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        public static void SaveKeyedTimestamp(this Properties.Settings settings, string key, DateTime value)
        {
            try
            {
                if (!PropertyKeyExists(settings.Properties, DefaultTimestampStore))
                {
                    Log.WarnFormat("The persistable timestamp store {0} does not exist", DefaultTimestampStore);
                    return;
                }

                var rawText = "";
                var rawEntry = settings[DefaultTimestampStore];
                if (rawEntry != null)
                    rawText = rawEntry.ToString();

                var entries = SerializableTimestampDictionary.LoadFromString(rawText);
                if (entries == null)
                {
                    Log.WarnFormat("Could not deserialize store {0}", DefaultTimestampStore);
                    return;
                }

                entries[key] = value;

                settings[DefaultTimestampStore] = entries.ToString();
                settings.Save();
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }



        private static bool PropertyKeyExists(IEnumerable properties, string name)
        {
            return
                properties.Cast<SettingsProperty>().Any(
                    property => property.Name.Equals(name, StringComparison.InvariantCulture));
        }


    }
}
