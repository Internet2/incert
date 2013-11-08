using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class ReturnPreservedResult:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ReturnPreservedResult(IEngine engine):base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            if (string.IsNullOrWhiteSpace(SettingKey))
                return new ExceptionOccurred(new Exception("No settings key specified"));

            var result = SettingsManager.GetTemporaryObject(SettingKey) as IResult;
            return result ??
                   new ExceptionOccurred(
                       new Exception(string.Format("No valid result object exists for the key {0}", SettingKey)));
        }

        public override string GetFriendlyName()
        {
            return "Return preserved result";
        }
    }
}
