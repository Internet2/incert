using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class PreserveLastResult:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public PreserveLastResult(IEngine engine) : base(engine)
        {
        }
        
        public override IResult Execute(IResult previousResults)
        {
            if (string.IsNullOrWhiteSpace(SettingKey))
                return new ExceptionOccurred(new Exception("No settings key specified"));

            SettingsManager.SetTemporaryObject(SettingKey, previousResults);

            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Preserve last result";
        }
    }
}
