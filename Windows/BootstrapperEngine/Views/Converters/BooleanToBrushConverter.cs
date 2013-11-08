using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Org.InCommon.InCert.BootstrapperEngine.Views.Converters
{
    class BooleanToBrushConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brushes = parameter as Brush[];
            if (brushes == null)
                return null;

            if (brushes.Length < 2)
                return null;

            if (!(value is bool))
                return brushes[0];

            if (!(bool) value)
                return brushes[0];

            return brushes[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
