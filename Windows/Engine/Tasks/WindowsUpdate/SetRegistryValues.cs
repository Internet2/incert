using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Importables.PropertySetters;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    internal class SetRegistryValues : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        private const string BaseRegistryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\WindowsUpdate";
        private const string AuRegistryPath = "SOFTWARE\\Policies\\Microsoft\\Windows\\WindowsUpdate\\AU";


        private readonly List<KeyedDynamicStringPropertyEntry> _setters = new List<KeyedDynamicStringPropertyEntry>();

        public SetRegistryValues(IEngine engine)
            : base(engine)
        {
        }


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

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                foreach (var setter in _setters)
                    CallSetterFunction(setter.Key, setter.Value);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }

        }

        public override string GetFriendlyName()
        {
            return string.Format("Set windows update registry values");
        }

        private void CallSetterFunction(string key, string value)
        {
            var methodInfo = GetType()
                .GetMethod("set" + key.ToLowerInvariant(), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic);

            if (methodInfo == null)
                throw new Exception(string.Format("Could not find setter method for {0}", key));

            if (!ReflectionUtilities.IsMethodAllowedFromXml(methodInfo))
                throw new Exception(string.Format("Method {0} not callable from xml", key));

            methodInfo.Invoke(this, new object[] { value });
        }

        internal void SetDWordValue(RegistryKey key, string valueName, string value)
        {
            if (key == null)
                throw new Exception("Registry key is not valid");

            if (string.IsNullOrWhiteSpace(value))
            {
                key.DeleteValue(valueName, false);
                Log.DebugFormat("{0} value cleared", valueName);
                return;
            }

            int newValue;
            if (!int.TryParse(value, out newValue))
                throw new Exception(string.Format("Could not convert {0} to valid integer value.", value));


            key.SetValue(valueName, newValue);
            Log.DebugFormat("{0} value set to {1}", valueName, value);
        }

        internal void SetStringValue(RegistryKey key, string valueName, string value)
        {
            if (key == null)
                throw new Exception("Registry key is not valid");

            if (string.IsNullOrWhiteSpace(value))
            {
                key.DeleteValue(valueName, false);
                Log.DebugFormat("{0} value cleared", valueName);
                return;
            }

            key.SetValue(valueName, value);
            Log.DebugFormat("{0} value set to {1}", valueName, value);
        }

        [MethodAllowedFromXml]
        internal void SetAuOptions(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "AUOptions", value);
        }

        [MethodAllowedFromXml]
        internal void SetAutoInstallMinorUpdates(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "AutoInstallMinorUpdates", value);
        }

        [MethodAllowedFromXml]
        internal void SetDetectionFrequency(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "DetectionFrequency", value);
        }

        [MethodAllowedFromXml]
        internal void SetDetectionFrequencyEnabled(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "DetectionFrequencyEnabled", value);
        }

        [MethodAllowedFromXml]
        internal void SetNoAutoRebootWithLoggedOnUsers(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "NoAutoRebootWithLoggedOnUsers", value);
        }

        [MethodAllowedFromXml]
        internal void SetNoAutoUpdate(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "NoAutoUpdate", value);
        }

        [MethodAllowedFromXml]
        internal void SetRebootRelaunchTimeout(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "RebootRelaunchTimeout", value);
        }

        [MethodAllowedFromXml]
        internal void SetRebootRelaunchTimeoutEnabled(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "RebootRelaunchTimeoutEnabled", value);
        }

        [MethodAllowedFromXml]
        internal void SetRebootWarningTimeout(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "RebootWarningTimeout", value);
        }

        [MethodAllowedFromXml]
        internal void SetRebootWarningTimeoutEnabled(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "RebootWarningTimeoutEnabled", value);
        }

        [MethodAllowedFromXml]
        internal void SetRescheduleWaitTime(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "RescheduleWaitTime", value);
        }

        [MethodAllowedFromXml]
        internal void SetRescheduleWaitTimeEnabled(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "RescheduleWaitTimeEnabled", value);
        }

        [MethodAllowedFromXml]
        internal void SetScheduledInstallDay(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "ScheduledInstallDay", value);
        }

        [MethodAllowedFromXml]
        internal void SetScheduledInstallTime(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "ScheduledInstallTime", value);
        }

        [MethodAllowedFromXml]
        internal void SetUseWuServer(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(AuRegistryPath))
                SetDWordValue(key, "UseWUServer", value);
        }

        [MethodAllowedFromXml]
        internal void SetElevateNonAdmins(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(BaseRegistryPath))
                SetDWordValue(key, "ElevateNonAdmins", value);
        }

        [MethodAllowedFromXml]
        internal void SetTargetGroup(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(BaseRegistryPath))
                SetStringValue(key, "TargetGroup", value);
        }

        [MethodAllowedFromXml]
        internal void SetTargetGroupEnabled(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(BaseRegistryPath))
                SetDWordValue(key, "TargetGroupEnabled", value);
        }

        [MethodAllowedFromXml]
        internal void SetWuServer(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(BaseRegistryPath))
                SetStringValue(key, "WUServer", value);
        }

        [MethodAllowedFromXml]
        internal void SetWuStatusServer(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(BaseRegistryPath))
                SetStringValue(key, "WUStatusServer", value);

        }

        [MethodAllowedFromXml]
        internal void SetAcceptTrustedPublisherCerts(string value)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(BaseRegistryPath))
                SetDWordValue(key, "AcceptTrustedPublisherCerts", value);
        }

    }
}
