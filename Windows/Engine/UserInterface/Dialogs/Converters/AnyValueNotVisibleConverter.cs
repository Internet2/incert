using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class AnyValueNotVisibleConverter:IMultiValueConverter

    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<Visibility>().FirstOrDefault(value => value != Visibility.Visible);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
