using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class BooleanToFadingBrushConverter : IValueConverter
    {
      
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = parameter as SolidColorBrush;
            if (brush == null)
                return parameter;
            
            var fade = (bool)value;
            return !fade ?
                FadedColorBrush(brush) :
                parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        protected SolidColorBrush FadedColorBrush(SolidColorBrush normalValue)
        {
            return new SolidColorBrush(FadeColor(normalValue.Color));
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
    }
}
