using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using log4net;
using Ninject;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class ReflectionUtilities
    {

        private static readonly ILog Log = Logger.Create();

        private static List<string> _knownAssemblyNames = Assembly.GetExecutingAssembly().GetTypes().Select(t => string.Format("{0}.{1}", t.Namespace, t.Name)).ToList(); 

        public static bool IsMethodAllowedFromXml(MemberInfo method)
        {
            var allowedAttributes = method.GetCustomAttributes(typeof(MethodAllowedFromXml), true);
            return (allowedAttributes.Any());
        }

        public static bool IsObsolete(MemberInfo target)
        {
            var obsoleteAttributes = target.GetCustomAttributes(typeof(ObsoleteAttribute), true);
            return (obsoleteAttributes.Any());
        }

        public static bool IsPropertyAllowedFromXml(MemberInfo target)
        {
            var allowedAttributes = target.GetCustomAttributes(typeof(PropertyAllowedFromXml), true);
            return (allowedAttributes.Any());
        }

       public static Type ResolveTypeForName<T>(string typeName) where T:class
        {
            var baseType = typeof (T);

            if (!baseType.IsAbstract)
                return baseType;

            var targetAssembly = Assembly.GetExecutingAssembly();
            var fullTypeName = ResolvePath(string.Format("{0}.{1}", baseType.Namespace, typeName));
            
            return targetAssembly.GetType(fullTypeName);
        }

       private static string ResolvePath(string value)
       {
           return _knownAssemblyNames.FirstOrDefault(t => t.Equals(value, StringComparison.InvariantCultureIgnoreCase));
       }

        public static T LoadFromAssembly<T>(string typeName) where T: class
        {
            try
            {
                var targetType = ResolveTypeForName<T>(typeName);
                if (targetType == null)
                    throw new Exception("Cannot initialize class (" + typeName + ") from assembly " +
                                        "and type name. Could not load class type.");
                
                return LoadFromType(targetType) as T;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot initialize class (" + typeof(T).Name + ") from assembly and type name. An exception occurred while attempting to initialize the task: " + ex.Message);
                return null;
            }
        }

        public static object LoadFromType(Type taskType)
        {
            try
            {
                return Application.Current.CurrentKernel().Get(taskType);
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot initialize class from type (" + taskType.FullName + "). An exception occurred while attempting to initialize the task: " + ex.Message);
                return null;
            }
        }

        public static string NormalizeTypeName(string value, string nameSpace)
        {
            if (value.StartsWith(nameSpace, StringComparison.InvariantCulture))
                return value;

            return nameSpace + "." + value;
        }

        public static object GetPropertyValue(object target, string[] propertyNames)
        {
            var result = target;

            foreach (var propertyName in propertyNames)
            {
                var info = result.GetType().GetProperty(propertyName);
                if (info == null) return null;

                result = result.GetType().GetProperty(propertyName).GetValue(result, null);
                if (result == null) return null;
            }

            return result;
        }

        public static object GetPropertyValue(object target, string propertyName)
        {
            var info = target.GetType().GetProperty(propertyName);
            return info == null ? null : target.GetType().GetProperty(propertyName).GetValue(target, null);
        }

        public static void SetStringPropertyValue(object target, string propertyName, string value)
        {
            var info = target.GetType().GetProperty(propertyName);
            if (info == null)
                return;

            if (info.PropertyType != typeof(string))
                return;

            info.SetValue(target, value);
        }

        public static object CopyObjectProperties(Object source, Object destination, BindingFlags flags)
        {
            if (source.GetType() == destination.GetType())
                return source;

            var properties = destination.GetType().GetProperties(flags);
            foreach (var destProperty in properties)
            {
                var sourceProperty = source.GetType().GetProperty(destProperty.Name, flags);
                if (sourceProperty == null)
                    continue;

                if (sourceProperty.PropertyType != destProperty.PropertyType)
                    continue;
                
                destProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
            }

            return destination;
        }

        public static object[] GetValuesForTokens(object target, string baseText)
        {
            if (target == null)
                return new object[0];

            if (string.IsNullOrWhiteSpace(baseText))
                return new object[0];

            var matches = Regex.Matches(baseText, @"\{(.*?)\}");
            if (matches.Count == 0)
                return new object[0];

            var values = new List<object>();
            for (var index = 0; index < matches.Count; index++)
            {
                var match = matches[index];
                if (match.Groups.Count < 2)
                    continue;

                var key = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(key))
                    continue;
                
                values.Add(GetValueForProperty(target, key));
            }

            return values.ToArray();
        }

        public static string GetObjectPropertyText(object target, string baseText)
        {
            try
            {
                if (target == null)
                    return baseText;

                if (string.IsNullOrWhiteSpace(baseText))
                    return baseText;

                var matches = Regex.Matches(baseText, @"\{(.*?)\}");
                if (matches.Count == 0)
                    return baseText;

                var values = new List<object>();
                for (var index = 0; index < matches.Count; index++)
                {
                    var match = matches[index];
                    if (match.Groups.Count < 2)
                        continue;

                    var matchText = match.Groups[0].Value;
                    if (string.IsNullOrWhiteSpace(matchText))
                        continue;

                    var key = match.Groups[1].Value;
                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    baseText = baseText.Replace(matchText, "{" + index + "}");
                    values.Add(GetValueForProperty(target, key));
                }

                return string.Format(baseText, values.ToArray());
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to get text from object: {0}", e.Message);
                return "";
            }
        }

        private static object GetValueForProperty(object target, string propertyName)
        {
            var propertyValue = GetPropertyValue(target, propertyName);
            return propertyValue ?? "";
        }
    }
}
