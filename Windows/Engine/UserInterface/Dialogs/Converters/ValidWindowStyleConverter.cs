using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class ValidWindowStyleConverter : IValueConverter
    {
        private readonly Window _instance;

        public ValidWindowStyleConverter(Window instance)
        {
            _instance = instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_instance == null)
                return value;

            if (_instance.AllowsTransparency)
                return WindowStyle.None;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
