using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Org.InCommon.InCert.BootstrapperEngine.Views.Converters
{
    class BooleanToThicknessConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return new Thickness(0);

            if (!(value is bool))
                return new Thickness(0);

            var parameterValue = parameter.ToString();
            if (string.IsNullOrWhiteSpace(parameterValue))
                return new Thickness(0);

            return FromString(parameterValue);
        }

        private Thickness FromString(string value)
        {
            try
            {
                var result =  new ThicknessConverter().ConvertFromString(value) as Thickness?;
                if (!result.HasValue)
                    return new Thickness(0);

                return result.Value;

            }
            catch (Exception)
            {
                return new Thickness(0);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
