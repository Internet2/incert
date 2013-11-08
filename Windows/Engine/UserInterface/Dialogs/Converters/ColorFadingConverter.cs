using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class ColorFadingConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = parameter as SolidColorBrush;
            if (brush == null)
                return null;

            return new SolidColorBrush(FadeColor(brush.Color));
        }

        private static Color FadeColor(Color value)
        {
            var color = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
            color = ControlPaint.Light(color);
            return new Color
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
