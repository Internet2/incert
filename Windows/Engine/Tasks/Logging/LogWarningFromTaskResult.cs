using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Logging
{
    class LogWarningFromTaskResult : AbstractTask
    {
        private static readonly ILog Log = Logger.Create();

        [PropertyAllowedFromXml]
        public string BaseText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ResultKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public LogWarningFromTaskResult(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var result = SettingsManager.GetTemporaryObject(ResultKey) ?? previousResults;

                var text = ReflectionUtilities.GetObjectPropertyText(result, BaseText);
                if (!string.IsNullOrWhiteSpace(text))
                    Log.Warn(text);
                else
                    Log.WarnFormat("Logging warning for result {0}", result);

                return new NextResult();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting record a log warning for a result: {0}", e.Message);
                return new NextResult();
            }
        }

        public override string GetFriendlyName()
        {
            return "Log warning from task result";
        }
    }
}
