using System;
using System.IO;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    class UninstallVersionAtLeast : AbstractCondition
    {
        public UninstallVersionAtLeast(IEngine engine)
            : base (engine)
        {
        }

        public string UninstallKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string VersionValueName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UninstallKey))
                    return new BooleanReason(false, "No uninstall key specified");

                if (string.IsNullOrWhiteSpace(VersionValueName))
                    return new BooleanReason(false, "No value name specified");

                var normalPath = Path.Combine(new[] { "SOFTWARE", "Microsoft", "Windows", "CurrentVersion", "Uninstall", UninstallKey });
                var wow6432Path = Path.Combine(new[] { "SOFTWARE", "Wow6432Node", "Microsoft", "Windows", "CurrentVersion", "Uninstall", UninstallKey });
                var registryVersion =
                    GetVersionFromKey(normalPath,VersionValueName) ?? 
                    GetVersionFromKey(wow6432Path, VersionValueName);

                if (registryVersion == null)
                    return new BooleanReason(false, "Could not get version from key value {0}", VersionValueName);

                var info = SettingsManager.GetTemporaryObject(ObjectKey) as FileInfoWrapper;
                if (info == null)
                    return new BooleanReason(false, "No wrapper exists for the key {0}", ObjectKey);

                Version compareVersion;
                if (!Version.TryParse(info.Version, out compareVersion))
                    return new BooleanReason(false, "Could not convert value {0} to version instance");

                return registryVersion< compareVersion 
                    ? new BooleanReason(false, "Registry version ({0}) is less than {1}", registryVersion, compareVersion) 
                    : new BooleanReason(true, "Registry version ({0}) is at least {1}", registryVersion, compareVersion);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while evaluating the condition: {0}", e.Message);
            }
        }

        private static Version GetVersionFromKey(string keyPath, string valueName)
        {
            using (var key = RegistryUtilities.RegistryRootValues.LocalMachine.GetExistingKey(keyPath))
            {
                if (key == null)
                    return null;

                var value = key.GetValue(valueName).ToStringOrDefault("");
                if (string.IsNullOrWhiteSpace(value))
                    return null;

                Version version;
                return !Version.TryParse(value, out version) ? null : version;
            }
        }

        public override bool IsInitialized()
        {
            return IsPropertySet("UninstallKey") && IsPropertySet("VersionValueName");
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            UninstallKey = XmlUtilities.GetTextFromAttribute(node, "uninstallKey");
            VersionValueName = XmlUtilities.GetTextFromAttribute(node, "valueName");
            ObjectKey = XmlUtilities.GetTextFromAttribute(node, "objectKey");
        }
    }
}
