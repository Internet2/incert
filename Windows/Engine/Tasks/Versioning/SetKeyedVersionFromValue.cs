using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Versioning
{
    class SetKeyedVersionFromValue : AbstractTask
    {
        [PropertyAllowedFromXml]
        public string Version
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public SetKeyedVersionFromValue(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw new Exception("Cannot persist version to setting; key is null or invalid.");

                if (string.IsNullOrWhiteSpace(Version))
                    throw new Exception("Cannot persist version to setting; value is null or invalid.");

                Version version;
                if (!System.Version.TryParse(Version, out version))
                    throw new Exception(
                        string.Format("Cannot persist version to setting; value {0} is invalid", Version));

                // we save the version as a string, but we convert it first to ensure that it's valid
                SettingsManager.SetTemporarySettingString(SettingKey, version.ToString());

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set keyed version ({0}) to {1}", SettingKey, Version);
        }
    }
}
