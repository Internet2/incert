using System;
using System.Globalization;
using System.Windows.Data;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    public class SettingsConverter:IValueConverter
    {
        private readonly  AbstractModel _model;
        protected readonly ISettingsManager Manager;

        public SettingsConverter(AbstractModel model, ISettingsManager manager)
        {
           _model = model;
            Manager = manager;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return null;

            var key = parameter.ToString();

            return string.IsNullOrWhiteSpace(key) ? null : Manager.GetTemporarySettingString(key);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return null;

            var key = parameter.ToString();

            return string.IsNullOrWhiteSpace(key) ? null : new StringSettingWrapper(key, value.ToString(), _model);
        }
    }
}
