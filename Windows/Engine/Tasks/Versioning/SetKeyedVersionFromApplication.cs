using System;
using System.Windows;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Tasks.Versioning
{
    class SetKeyedVersionFromApplication:AbstractTask
    {

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public SetKeyedVersionFromApplication(IEngine engine) : base(engine)
        {
           
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                SettingsManager.SetTemporarySettingString(SettingKey, Application.Current.GetVersion().ToString());
                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return string.Format("Set keyed version {0} to {1}", SettingKey, Application.Current.GetVersion());
        }
    }
}
