using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Settings;
using log4net;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class XmlUtilities
    {
        private static readonly ILog Log = Logger.Create();


        public static bool IsContentNode(XElement node)
        {
            try
            {
                if (node == null)
                    return false;

                return node.NodeType == XmlNodeType.Element;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return false;
            }
        }


        public static XElement LoadXmlFromAssembly(string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    Log.Warn("Cannot load file (" + value + "). Empty string passed to LoadFromAssembly.");
                    return null;
                }

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(value))
                {
                    return stream == null ? null : XElement.Load(stream);
                }
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static string GetXmlText(XElement element)
        {
            try
            {
                if (element == null)
                    return "";

                using (var reader = element.CreateReader())
                {
                    reader.MoveToContent();
                    return reader.ReadInnerXml();
                }

            }
            catch (Exception e)
            {
                Log.Warn(e);
                return "";
            }
        }

        public static XElement LoadXmlFromFile(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Log.Warn("Cannot load xml from file; path is empty");
                    return null;
                }

                if (!File.Exists(path))
                {
                    Log.WarnFormat("Cannot load xml from file; path ({0}) is invalid", path);
                    return null;
                }

                return XElement.Load(path);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static string ResolveTextValues(ISettingsManager manager, string value)
        {
            var result = value;

            if (string.IsNullOrWhiteSpace(value))
                return result;

            var matches = Regex.Matches(value, @"\[(.*?)\]");
            if (matches.Count == 0)
                return result;

            foreach (Match match in matches)
            {
                if (match.Groups.Count < 2)
                    continue;

                var matchText = match.Groups[0].Value;
                if (string.IsNullOrWhiteSpace(matchText))
                    continue;

                var key = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                result = result.Replace(matchText, manager.GetTemporarySettingString(key));
            }

            return result;

        }

        public static string GetTextFromAttribute(XElement node, string name)
        {
            if (node == null)
                return "";

            if (!node.HasAttributes)
                return "";

            var valueNode = node.Attributes(name).FirstOrDefault();
            return valueNode == null ? "" : GetTextFromNode(valueNode);
        }

        public static string GetTextFromAttribute(XElement node, string name, string defaultValue)
        {
            var result = GetTextFromAttribute(node, name);
            return !string.IsNullOrWhiteSpace(result) ? result : defaultValue;
        }

        public static string GetTextFromChildNodeAttribute(XElement node, string name, string attribute)
        {
            if (node == null)
                return "";

            if (!node.Elements().Any())
                return "";

            var childNode = node.Elements(name).FirstOrDefault();
            return childNode == null ? "" : GetTextFromAttribute(childNode, attribute);
        }

        public static int GetIntegerFromChildNodeAttribute(XElement node, string name, string attribute)
        {
            var value = 0;
            if (node == null)
                return value;

            var rawValue = GetTextFromChildNodeAttribute(node, name, attribute);
            if (!string.IsNullOrEmpty(rawValue))
                int.TryParse(rawValue, out value);

            return value;
        }

        public static string GetTextFromChildNode(XElement node, string name)
        {
            if (node == null)
                return "";

            var valueNode = node.Elements(name).FirstOrDefault();
            return valueNode == null ? "" : GetTextFromNode(valueNode);
        }

        public static string GetTextFromNode(XElement node)
        {
            if (node == null)
                return "";

            if (string.IsNullOrWhiteSpace(node.Value))
                return "";

            return node.Value;
        }

        public static string GetTextFromNode(XAttribute node)
        {
            if (node == null)
                return "";

            if (string.IsNullOrWhiteSpace(node.Value))
                return "";

            return node.Value;
        }

        public static string GetTextFromChildNode(XElement node, string name, string defaultValue)
        {
            if (node == null)
                return defaultValue;

            var valueNode = node.Elements(name).FirstOrDefault();
            return valueNode == null ? defaultValue : GetTextFromNode(node);
        }

        public static int GetIntegerFromAttribute(XElement node, string name)
        {
            var value = 0;

            if (node == null)
                return value;

            if (!node.HasAttributes)
                return value;

            var valueNode = node.Attributes(name).FirstOrDefault();
            if ((valueNode != null))
            {

                int.TryParse(GetTextFromNode(valueNode), out value);
            }

            return value;
        }

        public static int GetIntegerFromAttribute(XElement node, string name, int defaultValue)
        {
            var value = defaultValue;

            if (node == null)
            {
                return value;
            }

            if (!node.HasAttributes)
            {
                return value;
            }

            var valueNode = node.Attributes(name).FirstOrDefault();
            if ((valueNode != null))
            {
                int.TryParse(GetTextFromNode(valueNode), out value);
            }
            else
            {
                return defaultValue;
            }

            return value;
        }

        public static List<string> GetStringCollectionFromChildNodes(XElement node, string name)
        {
            var result = new List<string>();
            if (node == null)
            {
                return result;
            }

            var nodes = node.Elements(name);
            result.AddRange(nodes.Select(GetTextFromNode));

            return result;
        }

        public static double GetDoubleFromAttribute(XElement node, string name, double defaultValue)
        {
            var value = defaultValue;

            if (node == null)
                return value;

            if (!node.HasAttributes)
                return value;

            var valueNode = node.Attributes(name).FirstOrDefault();
            if ((valueNode == null))
                return defaultValue;

            var rawValue = GetTextFromNode(valueNode);
            if (string.IsNullOrWhiteSpace(rawValue))
                return defaultValue;

            if (rawValue.ToLowerInvariant().Equals("auto"))
                value = double.NaN;
            else if (rawValue.ToLowerInvariant().Equals("*"))
                value = double.PositiveInfinity;
            else
                double.TryParse(rawValue, out value);

            return value;
        }

        public static int GetIntegerFromChildNode(XElement node, string name, int defaultValue)
        {
            if (node == null)
            {
                return defaultValue;
            }

            int value;
            var valueNode = node.Elements(name).FirstOrDefault();
            if (valueNode == null)
            {
                return defaultValue;
            }
            int.TryParse(GetTextFromNode(valueNode), out value);

            return value;
        }

        public static bool? GetBooleanFromAttribute(XElement node, string name)
        {
            if (node == null)
                return null;

            if (!node.HasAttributes)
                return null;

            var valueNode = node.Attributes(name).FirstOrDefault();
            if (valueNode == null)
                return null;

            bool value;
            if (!bool.TryParse(GetTextFromNode(valueNode), out value))
            {
                return null;
            }

            return value;
        }

        public static bool GetBooleanFromAttribute(XElement node, string name, bool defaultValue)
        {
            var value = defaultValue;
            if (node == null)
                return value;

            if (!node.HasAttributes)
                return value;

            var valueNode = node.Attributes(name).FirstOrDefault();
            if ((valueNode != null))
            {
                bool.TryParse(GetTextFromNode(valueNode), out value);
            }

            return value;
        }

        public static bool GetBooleanFromChildNode(XElement node, string name, bool defaultValue)
        {
            var value = defaultValue;
            if (node == null)
            {
                return defaultValue;
            }

            var valueNode = node.Elements(name).FirstOrDefault();
            if (valueNode == null)
            {
                return value;
            }

            bool.TryParse(GetTextFromNode(valueNode), out value);
            return value;
        }

        public static bool GetBooleanFromChildNodeAttribue(XElement node, string childName, string attributeName, bool defaultValue)
        {
            var value = defaultValue;
            if (node == null)
                return value;

            var valueNode = node.Elements(childName).FirstOrDefault();
            return valueNode == null ? value : GetBooleanFromAttribute(valueNode, attributeName, defaultValue);
        }


        public static XAttribute AddAttributeToNode(XElement node, string name, string value)
        {
            try
            {
                if (node == null)
                    return null;


                node.SetAttributeValue(name, value);

                return node.Attributes(name).FirstOrDefault();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static XElement AddUniqueChildNode(XElement node, string name)
        {
            try
            {
                if (node == null)
                    return null;

                var result = node.Elements(name).FirstOrDefault();
                if (result != null)
                    return result;

                return AddChildNode(node, name);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static XElement AddChildNode(XElement node, string name)
        {
            try
            {
                if (node == null)
                    return null;

                var child = new XElement(name);
                node.Add(child);
                return child;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static StringCollection GetTextCollectionFromChildNode(XElement node, string name)
        {
            if (node == null)
                return new StringCollection();

            var values = node.Elements(name).ToList();
            if (!values.Any())
                return new StringCollection();

            var valuesCollection = new StringCollection();
            foreach (var valueNode in values.Where(valueNode => !string.IsNullOrEmpty(GetTextFromNode(valueNode))))
            {
                valuesCollection.Add(GetTextFromNode(valueNode));
            }

            return valuesCollection;
        }

        public static object GetEnumValueFromChildNode(XElement node, string name, object defaultValue)
        {
            try
            {
                if (node == null)
                    return defaultValue;

                var type = defaultValue.GetType();
                if (!type.IsEnum)
                    throw new InvalidOperationException("default value must be a member of an enumeration");

                var rawValue = GetTextFromChildNode(node, name);
                if (string.IsNullOrEmpty(rawValue))
                    return defaultValue;

                return !Enum.IsDefined(type, rawValue) ? defaultValue : Enum.Parse(type, rawValue);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return defaultValue;
            }
        }

        public static object GetEnumValueFromChildNodeAttribute(XElement node, string name, string attribute, object defaultValue)
        {
            try
            {
                if (node == null)
                    return defaultValue;

                var type = defaultValue.GetType();
                if (!type.IsEnum)
                    throw new InvalidOperationException("default value must be a member of an enumeration");

                var rawValue = GetTextFromChildNodeAttribute(node, name, attribute);
                if (string.IsNullOrEmpty(rawValue))
                    return defaultValue;


                return !Enum.IsDefined(type, rawValue) ? defaultValue : Enum.Parse(type, rawValue);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return defaultValue;
            }
        }

        public static T GetEnumFlagsValueFromAttribute<T>(XElement node, string name, T defaultValue) where T : struct, IConvertible
        {
            try
            {
                if (node == null)
                    return defaultValue;

                var type = typeof(T);
                if (!type.IsEnum)
                    return defaultValue;

                if (type.GetCustomAttribute<FlagsAttribute>() == null)
                    return defaultValue;

                var rawValue = GetTextFromAttribute(node, name);
                if (string.IsNullOrWhiteSpace(rawValue))
                    return defaultValue;

                var result = 0;

                var elements = rawValue.Split(' ');
                foreach (var element in elements)
                {
                    if (!Enum.IsDefined(type, element))
                    {
                        Log.WarnFormat("The value {0} is not defined for the enumeration {1}", element, type);
                        continue;
                    }

                    var value = Convert.ToInt32((T)Enum.Parse(type, element));
                    result = result | value;
                }

                return (T)Enum.ToObject(type, result);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to convert an attribute list to an enum value: {0}", e);
                return defaultValue;
            }

        }

        public static T GetEnumValueFromAttribute<T>(XElement node, string name, T defaultValue) where T : struct, IConvertible
        {
            try
            {
                if (node == null)
                    return defaultValue;

                var type = typeof(T);
                if (!type.IsEnum)
                    return defaultValue;

                var rawValue = GetTextFromAttribute(node, name);
                if (string.IsNullOrEmpty(rawValue))
                    return defaultValue;

                return !Enum.IsDefined(type, rawValue) ? defaultValue : (T)Enum.Parse(type, rawValue);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return defaultValue;
            }
        }

        public static T GetClassBasedEnumValueFromAttribute<T>(XElement node, string name, T defaultValue)
            where T : class
        {
            try
            {
                if (node == null)
                    return defaultValue;

                var rawValue = GetTextFromAttribute(node, name);
                if (string.IsNullOrEmpty(rawValue))
                    return defaultValue;

                var baseType = typeof(T);
                var info = baseType.GetField(rawValue, BindingFlags.Public | BindingFlags.Static);
                if (info == null)
                    return defaultValue;

                if (info.FieldType != baseType)
                    return defaultValue;

                return info.GetValue(null) as T;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return defaultValue;
            }
        }

        public static T GetClassBasedEnumValueFromNode<T>(XElement node, T defaultValue)
            where T : class
        {
            try
            {
                if (node == null)
                    return defaultValue;

                var rawValue = node.Value;
                if (string.IsNullOrEmpty(rawValue))
                    return defaultValue;

                var baseType = typeof(T);
                var info = baseType.GetField(rawValue, BindingFlags.Public | BindingFlags.Static);
                if (info == null)
                    return defaultValue;

                if (info.FieldType != baseType)
                    return defaultValue;

                return info.GetValue(null) as T;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return defaultValue;
            }
        }
        public static bool IsAttributeSet(XElement node, string name)
        {
            if (node == null)
                return false;

            if (!node.HasAttributes)
                return false;

            var valueNode = node.Attributes(name).FirstOrDefault();
            return valueNode != null;
        }




        /// <summary>
        /// Compares two xml nodes and determines whether they are equivalent
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns>true if the nodes are equal, false otherwise or if an exception occurs</returns>
        /// <remarks>This function depends on the Microsoft XMLDiffPatch library.  See http://msdn.microsoft.com/en-us/library/aa302294.aspx for more information.</remarks>
        public static bool AreNodesEqual(XmlNode version1, XmlNode version2)
        {
            try
            {
                var comparer = new XNodeEqualityComparer();

                XNode version1XNode = XDocument.Parse(version1.OuterXml);
                XNode version2XNode = XDocument.Parse(version2.OuterXml);

                return comparer.Equals(version1XNode, version2XNode);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return false;
            }
        }

        public static string EscapeXmlText(string input)
        {
            try
            {
                input = input.Replace("&", "&amp;");
                input = input.Replace("<", "&lt;");
                input = input.Replace(">", "&gt;");
                input = input.Replace("\"", "&quot;");
                input = input.Replace("'", "&apos;");
                return input;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return input;
            }
        }


        public static string JoinChildNodeText(XElement node, string name, string delimiter)
        {
            try
            {
                var textCollection = GetStringCollectionFromChildNodes(node, name);
                if (textCollection.Count == 1)
                    return textCollection[0];


                if (textCollection.Count > 1)
                {
                    var lines = new string[textCollection.Count];
                    textCollection.CopyTo(lines, 0);
                    return String.Join(delimiter, lines);
                }

                return "";

            }
            catch (Exception e)
            {
                Log.Warn(e);
                return "";
            }

        }

        public static T ConvertFromAttributeUsingConverter<T>(XElement node, string name, TypeConverter converter, T defaultValue)
        {
            try
            {
                var value = GetTextFromAttribute(node, name);
                if (string.IsNullOrWhiteSpace(value))
                    return defaultValue;

                var result = converter.ConvertFromInvariantString(value);
                return (T)result;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return defaultValue;
            }

        }
    }
}
