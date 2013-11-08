using System;
using System.Net;
using System.Security;
using Org.InCommon.InCert.Engine.Results;

namespace Org.InCommon.InCert.Engine.Settings
{
    public interface ISettingsManager
    {
        int CurrentSecurityLevel { get; set; }
        NetworkCredential DownloadCredentials { get; set; }
        string BaseWebServiceUrl { get; set; }
        BindingSettingsProxy BindingProxy { get; }
        string Username { get; set; }
        bool AreSettingsLoaded();
        
        bool IsTemporarySettingStringPresent(string key);
        string GetTemporarySettingString(string key);
        SecureString GetSecureTemporarySettingString(string key);
        void RemoveTemporarySettingString(string key);

        [SecurityCritical]
        void SetTemporarySettingString(string key, string value);

        [SecurityCritical]
        void SetTemporarySettingString(string key, SecureString value);

        object GetTemporaryObject(string key);
        void SetTemporaryObject(string key, object value);
        void RemoveTemporaryObject(string key);

        void Initialize();

        IResult PreviousTaskResult { get; set; }
        Version EffectiveEngineVersion { get; set; }
    }
}