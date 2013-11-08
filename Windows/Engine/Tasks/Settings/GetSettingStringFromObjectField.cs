using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class GetSettingStringFromObjectField : AbstractTask
    {

        [PropertyAllowedFromXml]
        public string ObjectKey
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

        [PropertyAllowedFromXml]
        public string BaseText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public GetSettingStringFromObjectField(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw new Exception("Setting key required");

                SettingsManager.SetTemporarySettingString(
                    SettingKey,
                    ReflectionUtilities.GetObjectPropertyText(
                        SettingsManager.GetTemporaryObject(ObjectKey), 
                        BaseText));
                    
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Get setting string from object field.";
        }
    }
}
