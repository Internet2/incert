using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Time
{
    class SetTimeZone : AbstractTimeTask
    {
        private static readonly ILog Log = Logger.Create();

        private const string PrivilegeName = "SeTimeZonePrivilege";
        private const string BaseRegistryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones";

        [PropertyAllowedFromXml]
        public string TimeZone
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public SetTimeZone(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            ProcessExtensions.TokenPrivileges? previousState = null;
            RegistryKey baseKey = null;
            try
            {
                baseKey = Registry.LocalMachine.OpenSubKey(Path.Combine(BaseRegistryPath, TimeZone), false);
                if (baseKey == null)
                    throw new Exception(string.Format("Cannot open registry store for provided timezone value ({0})", TimeZone));

                var info = GetDynamicTimeZoneInfo(baseKey);
                if (info == null)
                    throw new Exception(string.Format("Cannot retrieve time zone information from registry store for provided timezone value ({0})", TimeZone));

                var lastState = new ProcessExtensions.TokenPrivileges();
                if (!SetTimeZonePrivilege(ref lastState))
                    throw new Exception("Cannot set change time-zone privilege for current process");

                // put in nullable so we can tell whether we need to restore this in finally block
                previousState = lastState;

                var infoInstance = info.Value;
                if (!NativeMethods.SetDynamicTimeZoneInformation(ref infoInstance))
                    throw new Win32Exception();

                return new NextResult();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to set this computer's time zone: {0}", e.Message);
                return new ExceptionOccurred(e);
            }
            finally
            {
                if (previousState.HasValue)
                    Process.GetCurrentProcess().SetPrivilegeTokens(previousState.Value);

                if (baseKey != null)
                    baseKey.Close();
            }
        }


        private static NativeMethods.DynamicTimeZoneInformation? GetDynamicTimeZoneInfo(RegistryKey baseKey)
        {
            RegistryKey dynamicKey = null;
            try
            {
                if (baseKey == null)
                    return null;

                dynamicKey = baseKey.OpenSubKey("Dynamic DST", false);
                if (dynamicKey == null)
                    return null;

                var firstEntryValue = dynamicKey.GetValue("FirstEntry");
                if (firstEntryValue == null)
                    return null;

                var rawInfo = dynamicKey.GetValue(firstEntryValue.ToString());
                if (rawInfo == null)
                    return null;

                if (!(rawInfo is byte[]))
                    return null;

                var result = new NativeMethods.DynamicTimeZoneInformation();
                result.SetBytes((byte[])rawInfo);

                if (baseKey.GetValue("std") != null)
                    result.StandardName = baseKey.GetValue("std").ToString();

                if (baseKey.GetValue("dlt") != null)
                    result.DaylightName = baseKey.GetValue("dlt").ToString();

                result.TimeZoneKeyName = Path.GetFileNameWithoutExtension(baseKey.Name);
                result.DynamicDaylightTimeDisabled = 0;

                return result;
            }
            finally
            {
                if (dynamicKey != null)
                    dynamicKey.Close();
            }


        }

        private static bool SetTimeZonePrivilege(ref ProcessExtensions.TokenPrivileges previousState)
        {
            if (!Process.GetCurrentProcess().SetPrivilegeTokens(PrivilegeName, ref previousState))
            {
                Log.WarnFormat("Could not set time zone: unable to set appropriate privileges for process");
                return false;
            }

            return true;
        }

        public override string GetFriendlyName()
        {
            return "Set time zone";
        }






    }


}



