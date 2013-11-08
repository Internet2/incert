using System;
using System.Globalization;
using System.Windows.Data;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class AdditiveConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value;

            var originalValue = value as double?;
            if (!originalValue.HasValue)
                return value;


            double toAdd;
            if (!double.TryParse(parameter.ToString(), out toAdd))
                return value;

            return originalValue.Value + toAdd;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
