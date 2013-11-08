using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class ClearSettingText : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        private readonly List<string> _keys = new List<string>();

        public ClearSettingText(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.Warn("Cannot add key to values collection; key cannot be null or whitespace.");
                    return;
                }

                _keys.Add(value);
            }
        }


        public override IResult Execute(IResult previousResults)
        {
            foreach (var key in _keys)
            {
                SettingsManager.RemoveTemporarySettingString(key);
            }

            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Set settings text value";
        }
    }
}
