using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class FontSizeToLeftThicknessConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return new Thickness(0);
            
            var fontSize = value as double?;
            if (!fontSize.HasValue)
                return new Thickness(0);

            var result = ConvertFromString(parameter.ToString(), new Thickness(0));
            result.Left = result.Left + (fontSize.Value*.25);
            return result;
        }

        private static Thickness ConvertFromString(string value, Thickness defaultValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return defaultValue;

                var converter = new ThicknessConverter();
              

                var result = converter.ConvertFromInvariantString(value);
                if (result == null)
                    return defaultValue;

                return (Thickness)result;
            }
            catch (Exception)
            {
                return defaultValue;    
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
