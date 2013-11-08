using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class BooleanToBrushConverter:IValueConverter
    {
        private readonly Brush _trueColorBrush;
        private readonly Brush _falseColorBrush;

        public BooleanToBrushConverter(Brush trueColorBrush, Brush falseColorBrush)
        {
            _trueColorBrush = trueColorBrush;
            _falseColorBrush = falseColorBrush;
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = (bool)value;
            return !visible ? _falseColorBrush : _trueColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
