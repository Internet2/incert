using System;
using System.Collections.Generic;
using System.Text;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;

namespace Org.InCommon.InCert.Engine.Tasks.Settings
{
    class AppendLineToSettingsText : AbstractTask
    {
        public AppendLineToSettingsText(IEngine engine)
            : base(engine)
        {
        }

        private readonly List<string> _lines = new List<string>();
            
        [PropertyAllowedFromXml]
        public string Line
        {
            set
            {
                _lines.Add(value);
            }
        }

        [PropertyAllowedFromXml]
        public string SettingKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string Text
        {
            get { return GetDynamicValue(); }
            private set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SettingKey))
                    throw new Exception("Setting key cannot be empty");

                Text = string.Join(Environment.NewLine, _lines);

                var builder = new StringBuilder(SettingsManager.GetTemporarySettingString(SettingKey));
                builder.AppendLine(Text);

                var wrapper = new StringSettingWrapper(SettingKey, builder.ToString(), null);
                SettingsManager.BindingProxy.SettingProperty = wrapper;

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        public override string GetFriendlyName()
        {
            return "Append to settings text";
        }
    }
}
