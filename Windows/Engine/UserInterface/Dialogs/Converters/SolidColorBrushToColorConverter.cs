using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class SolidColorBrushToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Black;

            var brush = value as SolidColorBrush;
            if (brush == null)
                return Colors.Black;

            return brush.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof (Color))
                return null;

            return new SolidColorBrush((Color) value);
        }
    }
}
