using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class BooleanToColorConverter:IValueConverter
    {
        private readonly Color _trueColor;
        private readonly Color _falseColor;

        public BooleanToColorConverter(Color trueColor, Color falseColor)
        {
            _trueColor = trueColor;
            _falseColor = falseColor;
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value ? _falseColor : _trueColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
