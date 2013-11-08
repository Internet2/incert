using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Org.InCommon.InCert.BootstrapperEngine.Views.Converters
{
    class BooleanToBoldConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return FontWeights.Normal;

            if (!(bool) value)
                return FontWeights.Normal;

            return FontWeights.Bold;

        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
