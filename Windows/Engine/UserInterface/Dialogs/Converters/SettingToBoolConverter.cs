using System;
using System.Globalization;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    public class SettingToBoolConverter : SettingsConverter
    {
        public SettingToBoolConverter(AbstractModel model, ISettingsManager manager):base(model, manager)
        {
        
        }
        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return false;

            var key = parameter.ToString();
            if (string.IsNullOrWhiteSpace(key))
                return false;

            var settingValue = Manager.GetTemporarySettingString(key);
            bool result;
            return bool.TryParse(settingValue, out result) && result;
        }

        
    }
}
