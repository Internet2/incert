using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using Org.InCommon.InCert.BootstrapperEngine.Logging;

namespace Org.InCommon.InCert.BootstrapperEngine.Extensions
{
    public static class XElementExtensions
    {
        public static string ChildNodeValue(this XElement node, string name, string defaultValue = "")
        {
            if (node == null)
                return defaultValue;

            var childNode = node.Element(name);
            return childNode == null ? defaultValue : childNode.Value;
        }

        public static bool AttributeValue(this XElement node, string name, bool defaultValue)
        {
            var attribute = node.Attribute(name);
            if (attribute == null)
                return defaultValue;

            bool value;
            return !bool.TryParse(attribute.Value, out value) ? defaultValue : value;
        }

        public static Brush ChildNodeBrushValue(this XElement node, string name, Brush defaultValue = null)
        {
            try
            {
                if (node == null)
                    return defaultValue;

                var childNode = node.Element(name);
                if (childNode == null)
                    return defaultValue;

                var value = childNode.Value;
                Logger.Standard("Converting {0} to color", value);
                if (string.IsNullOrWhiteSpace(value))
                    return defaultValue;

                var result = value.ToSystemBrush();
                return result ?? value.ToBrush(defaultValue);
            }
            catch (Exception e)
            {
                Logger.Error("Could not convert xml tag value {0} to valid brush: {1}", name, e.Message);
                return defaultValue;
            }
        }

        private static Brush ToBrush(this string value, Brush defaultValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return defaultValue;

                var converter = new BrushConverter();
                return (Brush)converter.ConvertFromInvariantString(value);
            }
            catch (Exception e)
            {
                Logger.Error("Could not convert {0} to valid brush: {1}", value, e.Message);
                return defaultValue;
            }
        }

        private static Brush ToSystemBrush(this string value)
        {
            try
            {
                var type = typeof(SystemColors);
                var property = type.GetProperty(value, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);

                if (property == null)
                    return null;
                
                var result = property.GetValue(null, null) as SolidColorBrush;
                if (result == null)
                    Logger.Error("Could not conver {0} to system solid color brush: {1}", value, property.PropertyType.FullName);

                return result;
            }
            catch (Exception e)
            {
                Logger.Error("Could not convert {0} to system solid color brush: {1}", value, e.Message);
                return null;
            }
        }
    }
}
