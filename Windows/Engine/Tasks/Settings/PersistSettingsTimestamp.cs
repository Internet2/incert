using System;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class PersistSettingsTimestamp : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        public PersistSettingsTimestamp(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                {
                    Log.Warn("Cannot persist timestamp; settings key not specified");
                    return new NextResult();
                }

                Properties.Settings.Default.SaveKeyedTimestamp(SettingKey, DateTime.UtcNow);
                return new NextResult();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new NextResult();
            }
        }

        public override string GetFriendlyName()
        {
            return "Persist timestamp to permanent settings store";
        }
    }
}
