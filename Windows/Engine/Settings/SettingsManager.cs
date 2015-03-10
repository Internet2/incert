using System.Windows;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net;
using System.Security;
using System.Security.Permissions;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using log4net;

namespace Org.InCommon.InCert.Engine.Settings
{

    [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
    public class SettingsManager : ISettingsManager
    {
        private static readonly ILog Log = Logger.Create();

        private readonly Dictionary<string, SecureString> _settingsDictionary =
            new Dictionary<string, SecureString>();

        private readonly Dictionary<string, object> _tempObjectsCollection = new Dictionary<string, object>();

        private bool _settingsLoaded;

        public int CurrentSecurityLevel { get; set; }
        public NetworkCredential DownloadCredentials { get; set; }
        public string BaseWebServiceUrl { get; set; }

        public bool AreSettingsLoaded()
        {
            return _settingsLoaded;
        }

        public void Initialize()
        {
            try
            {
                // initialize the settings
                ResetSettings();
                BindingProxy = new BindingSettingsProxy(this);
                EffectiveEngineVersion = Application.Current.GetVersion();
                _settingsLoaded = true;
            }
            catch (Exception ex)
            {
                Log.Warn("an exception occurred while initializing the settings object: " + ex.Message);
            }
        }

        public BindingSettingsProxy BindingProxy { get; private set; }

        private void ResetSettings()
        {
            try
            {
                // clear the various collections
                _settingsDictionary.Clear();

                _tempObjectsCollection.Clear();

                // now set the various other variables
                CurrentSecurityLevel = 0;
                DownloadCredentials = null;
                _settingsLoaded = false;
            }
            catch (Exception ex)
            {
                Log.Warn("an exception occurred while resetting the settings object: " + ex.Message);
            }
        }

        [SecurityCritical]
        public bool IsTemporarySettingStringPresent(string key)
        {
            if (_settingsDictionary == null)
                return false;

            if (String.IsNullOrWhiteSpace(key))
                return false;

            if (!_settingsDictionary.ContainsKey(key))
                return false;

            var secureEntry = _settingsDictionary[key];
            if (secureEntry == null)
                return false;

            return secureEntry.Length > 0;
        }

        [SecurityCritical]
        public string GetTemporarySettingString(string key)
        {
            if (_settingsDictionary == null)
                return "";

            if (String.IsNullOrWhiteSpace(key))
                return "";

            if (!_settingsDictionary.ContainsKey(key))
                return "";

            var secureEntry = _settingsDictionary[key];
            if (secureEntry == null)
                return "";

            var securePointer = Marshal.SecureStringToBSTR(secureEntry);
            return Marshal.PtrToStringBSTR(securePointer);
        }

        [SecurityCritical]
        public SecureString GetSecureTemporarySettingString(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return new SecureString();

            if (!_settingsDictionary.ContainsKey(key))
                return new SecureString();

            return _settingsDictionary[key];
        }

        [SecurityCritical]
        public void RemoveTemporarySettingString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (!_settingsDictionary.ContainsKey(key))
                return;

            _settingsDictionary.Remove(key);
        }

        [SecurityCritical]
        public void SetTemporarySettingString(string key, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                if (_settingsDictionary.ContainsKey(key))
                    RemoveTemporarySettingString(key);

                return;
            }

            var secureString = new SecureString();
            foreach (var entry in value.ToCharArray())
            {
                secureString.AppendChar(entry);
            }
            _settingsDictionary[key] = secureString;
        }

        [SecurityCritical]
        public void SetTemporarySettingString(string key, SecureString value)
        {
            _settingsDictionary[key] = value;
        }

        public bool IsTemporaryObjectPresent(string key)
        {
            return _tempObjectsCollection.ContainsKey(key);
        }

        [SecurityCritical]
        public object GetTemporaryObject(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;

            return _tempObjectsCollection.ContainsKey(key) ? _tempObjectsCollection[key] : null;
        }

        [SecurityCritical]
        public void SetTemporaryObject(string key, object value)
        {
            _tempObjectsCollection[key] = value;
        }

        [SecurityCritical]
        public void RemoveTemporaryObject(string key)
        {
            if (!_tempObjectsCollection.ContainsKey(key))
                return;

            _tempObjectsCollection.Remove(key);
        }

        public IResult PreviousTaskResult { get; set; }
        public Version EffectiveEngineVersion { get; set; }

        public string Username
        {
            get
            {
                var username = GetTemporarySettingString("username");
                return string.IsNullOrWhiteSpace(username) ? "[unknown]" : username;
            }
            set { SetTemporarySettingString("username", value); }
        }
    }



}
