using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Versioning
{
    class SetEffectiveEngineVersion:AbstractTask
    {

        [PropertyAllowedFromXml]
        public string Version
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public SetEffectiveEngineVersion(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Version))
                    throw new Exception("Cannot persist version to setting; value is null or invalid.");

                Version version;
                if (!System.Version.TryParse(Version, out version))
                    throw new Exception(
                        string.Format("Cannot persist version to setting; value {0} is invalid", Version));

                // we save the version as a string, but we convert it first to ensure that it's valid
                SettingsManager.EffectiveEngineVersion = version;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set effective engine version";
        }
    }
}
