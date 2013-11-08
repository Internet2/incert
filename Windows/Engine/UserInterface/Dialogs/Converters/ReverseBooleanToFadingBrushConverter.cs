using System;
using System.Globalization;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class ReverseBooleanToFadingBrushConverter : BooleanToFadingBrushConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = parameter as SolidColorBrush;
            if (brush == null)
                return parameter;

            var fade = (bool)value;
            return !fade ? parameter : FadedColorBrush(brush);
        }



    }
}
