using System;
using System.Windows.Media;

namespace Org.InCommon.InCert.BootstrapperEngine.Extensions
{
    public static class UserInterfaceExtensions
    {
        public static SolidColorBrush ToSolidColorBrush(this string value, Color defaultColor)
        {
            try
            {
                var result = ColorConverter.ConvertFromString(value);
                return result == null ? new SolidColorBrush(defaultColor) : new SolidColorBrush((Color)result);
            }
            catch (Exception)
            {
                return new SolidColorBrush(defaultColor);
            }
        }

        public static string ToProductMessage(this string message, params object[] arguments)
        {
            return string.IsNullOrWhiteSpace(message) ? "" : string.Format(message, arguments);
        }
    }
}
