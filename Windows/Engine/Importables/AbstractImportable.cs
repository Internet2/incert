using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.CommandLineProcessors;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Errors.Mapping;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.Importables
{
    public abstract class AbstractImportable : IImportable, IHasEngineFields
    {
        private static readonly ILog Log = Logger.Create();

        private readonly List<string> _propertiesSpecified = new List<string>();

        protected AbstractImportable(IEngine engine)
        {
            Engine = engine;
        }

        public virtual void ConfigureFromNode(XElement element)
        {
            var propertiesNode = element.Elements("Properties").FirstOrDefault();
            MapChildrenToProperties(propertiesNode, this);

            var methodsNode = element.Elements("Methods").FirstOrDefault();
            MapChildrenToMethods(methodsNode, this);

        }

        public virtual bool Initialized()
        {
            return true;
        }

        public bool IsPropertySpecified(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return _propertiesSpecified.Any(e => e.Equals(name));
        }
        
        public static T GetInstanceFromNode<T>(XElement node) where T : class,IImportable
        {
            try
            {
                if (node == null)
                    return default(T);

                if (!XmlUtilities.IsContentNode(node))
                    return default(T);

                var type = typeof(T);
                var result = ReflectionUtilities.LoadFromAssembly<T>(node.Name.LocalName);
                if (result == null){
                    Log.WarnFormat("could not initialize {0}.{1} from xml", type.Namespace, node.Name.LocalName);
                    return default(T);
                }

                result.ConfigureFromNode(node);

                return result;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return default(T);
            }
        }
        
        protected void MapChildrenToProperties(XElement node, object target)
        {
            if (node == null)
                return;

            if (!node.Elements().Any())
                return;

            foreach (var child in node.Elements().Where(child => child.NodeType == XmlNodeType.Element))
            {
                MapNodeValueToProperty(child, target);
            }
        }

        protected static void MapChildrenToMethods(XElement node, object target)
        {
            if (node == null)
                return;

            if (!node.Elements().Any())
                return;

            foreach (var child in node.Elements().Where(child => child.NodeType == XmlNodeType.Element))
            {
                MapAttributesToMethod(child, target);
            }
        }

        private static bool PropertyIsValidTarget(MemberInfo propertyInfo, object target)
        {
            if (ReflectionUtilities.IsObsolete(propertyInfo))
                Log.Warn("The property " + propertyInfo.Name + " is obsolete for the class " + target.GetType().FullName + ".");

            if (!ReflectionUtilities.IsPropertyAllowedFromXml(propertyInfo))
            {
                Log.Warn("The property " + target.GetType().Name + "." + propertyInfo.Name + " cannot be set from xml");
                return false;
            }

            return true;
        }

        protected void MapNodeValueToProperty(XElement node, object target)
        {
            try
            {
                if (node == null)
                    return;

                var propertyInfo = target.GetType().GetProperty(node.Name.LocalName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (propertyInfo == null)
                    throw new Exception("The attribute " + node.Name + " does not map to a valid property of " + target.GetType().FullName);
                
                if (!PropertyIsValidTarget(propertyInfo, target))
                    throw new Exception("Could not assign property " + node.Name + " to " + target.GetType().FullName);
                
                var value = ConvertNodeToCorrectType(node, propertyInfo);
                if (value == null)
                    throw new Exception("Could not convert property " + node.Name + " to correct type");
                    
                _propertiesSpecified.Add(node.Name.LocalName);
                propertyInfo.SetValue(target, value, null);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        private static object ConvertNodeToCorrectType(XElement node, PropertyInfo info)
        {
            try
            {
                var result = ConvertNodeToEnum(node, info);
                if (result != null)
                    return result;

                result = ConvertNodeToImportable(node, info);
                if (result != null)
                    return result;

                var value = XmlUtilities.GetTextFromNode(node);
                return Convert.ChangeType(value, info.PropertyType);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }
        
        private static object ConvertNodeToEnum(XElement node, PropertyInfo info)
        {
            try
            {
                if (!info.PropertyType.IsEnum)
                    return null;

                var value = XmlUtilities.GetTextFromNode(node);
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("node must have a value set");

                value = value.Replace(' ', ',');
                return Enum.Parse(info.PropertyType, value);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        private static object ConvertNodeToImportable(XElement node, PropertyInfo info)
        {
            try
            {
                if (!info.PropertyType.GetInterfaces().Contains(typeof(IImportable)))
                    return null;

                var method = info.PropertyType.GetMethod("GetInstanceFromNode", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (method == null)
                    return null;

                var generic = method.MakeGenericMethod(new[] { info.PropertyType });
                return generic.Invoke(null, new object[] { node });
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        private static void MapAttributesToMethod(XElement node, object target)
        {
            try
            {
                if (node == null)
                    return;

                var method = target.GetType().GetMethod(node.Name.LocalName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (method == null)
                    throw new Exception(node.Name + " does not map to a valid method of " + target.GetType().FullName);
                
                if (ReflectionUtilities.IsObsolete(method))
                    Log.WarnFormat("The method {0} is obsolete for the class {1}.", method.Name, target.GetType().FullName);
                
                if (!ReflectionUtilities.IsMethodAllowedFromXml(method))
                    throw new Exception("The method " + method.Name + " cannot be invoked from xml");
                
                var parameters = MapAttributesToParameters(node, method.GetParameters());
                method.Invoke(target, parameters);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        private static object[] MapAttributesToParameters(XElement node, IEnumerable<ParameterInfo> parameters)
        {
            try
            {
                if (node == null)
                    return null;


                if (!node.HasAttributes)
                    return null;

                var values = new SortedList<int, object>();
                foreach (var info in parameters)
                {
                    var attribute = node.Attributes(info.Name).FirstOrDefault();
                    if (attribute == null)
                    {
                        Log.Warn("Parameter " + info.Name + " is not defined in " + node.Name);
                        continue;
                    }

                    var value = Convert.ChangeType(attribute.Value, info.ParameterType);
                    values.Add(info.Position, value);
                }

                return values.Values.ToArray();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public IEngine Engine { get; private set; }
        public ISettingsManager SettingsManager { get { return Engine.SettingsManager; } }
        public IBranchManager BranchManager { get { return Engine.BranchManager; } }
        public ICommandLineManager CommandLineManager { get { return Engine.CommandLineManager; } }
        public IBannerManager BannerManager { get { return Engine.BannerManager; } }
        public IAppearanceManager AppearanceManager { get { return Engine.AppearanceManager; } }
        public IErrorManager ErrorManager { get { return Engine.ErrorManager; } }
        public IDialogsManager DialogsManager { get { return Engine.DialogsManager; } }
        public IHelpManager HelpManager { get { return Engine.HelpManager; } }
        public IAdvancedMenuManager AdvancedMenuManager { get { return Engine.AdvancedMenuManager; } }
        public IEndpointManager EndpointManager { get { return Engine.EndpointManager; } }
    }




}
