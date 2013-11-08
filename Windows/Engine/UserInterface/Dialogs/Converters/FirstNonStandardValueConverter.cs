using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Converters
{
    class FirstNonStandardValueConverter:IMultiValueConverter
    {
     
        private readonly IComparer _comparer;
  
        public FirstNonStandardValueConverter()
        {
            _comparer = null;
        }

        public FirstNonStandardValueConverter(IComparer comparer )
        {
            _comparer = comparer;
        }
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return parameter;

            if (!values.Any())
                return parameter;

            foreach (var value in values)
            {
                if (_comparer == null)
                {
                    if (value != parameter)
                        return value;
                }
                else
                {
                    if (_comparer.Compare(value, parameter) != 0)
                        return value;
                }
            }

            return parameter;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
