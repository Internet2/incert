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
    class RetrieveSettingsTimestamp : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public RetrieveSettingsTimestamp(IEngine engine):base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var timestamp = Properties.Settings.Default.GetKeyedTimestamp(SettingKey);
                if (!timestamp.HasValue)
                    return new NextResult();

                SettingsManager.SetTemporaryObject(SettingKey, timestamp.Value);
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
            return "Retrieve timestamp from permanent settings store";
        }
    }
}
