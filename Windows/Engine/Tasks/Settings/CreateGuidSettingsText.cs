using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class CreateGuidSettingsText:AbstractTask
    {
        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }
        public CreateGuidSettingsText(IEngine engine) : base(engine)
        {
            
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw new Exception("Settings key cannot be null or empty");

                SettingsManager.SetTemporarySettingString(SettingKey, Guid.NewGuid().ToString());
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Create guid settings text";
        }
    }
}
