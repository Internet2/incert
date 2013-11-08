using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class ThicknessToStrokeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Thickness))
                return null;
            
            var borderThickness = (Thickness)value;
            return borderThickness.Top;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return null;
        }
    }
}
