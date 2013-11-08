using System;
using System.Globalization;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class SettingToIntegerConverter : SettingsConverter
    {
        

        public SettingToIntegerConverter(AbstractModel model, ISettingsManager manager) : base(model,manager)
        {
           
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return 0;

            var key = parameter.ToString();
            if (string.IsNullOrWhiteSpace(key))
                return 0;

            int result;
            int.TryParse(Manager.GetTemporarySettingString(key), out result);

            return result;

        }

        
    }
}
