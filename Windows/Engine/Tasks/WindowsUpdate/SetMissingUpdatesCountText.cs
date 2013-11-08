using System;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using WUApiLib;

namespace Org.InCommon.InCert.Engine.Tasks.WindowsUpdate
{
    class SetMissingUpdatesCountText:AbstractTask
    {
        public SetMissingUpdatesCountText(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string ObjectKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string SettingsKey
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
        
        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ObjectKey))
                    throw new Exception("Object key cannot be null");

                if (string.IsNullOrWhiteSpace(SettingsKey))
                    throw new Exception("Setting key cannot be null");

                if (string.IsNullOrWhiteSpace(BaseText))
                    BaseText = "{0}";


                var updateCollection = SettingsManager.GetTemporaryObject(ObjectKey) as UpdateCollection;
                if (updateCollection == null)
                    throw  new Exception("Update collection not present");

                SettingsManager.SetTemporarySettingString(SettingsKey, string.Format(BaseText, updateCollection.Count));

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Set missing updates count text";
        }
    }
}
