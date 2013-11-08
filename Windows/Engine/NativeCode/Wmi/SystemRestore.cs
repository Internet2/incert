using System;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Management;

namespace Org.InCommon.InCert.Engine.NativeCode.Wmi
{
    // imported from auto-generated class using mgmtclassgen
    // mgmtclassgen SystemRestore /p "[outputpath]\SystemRestore.cs" /n root\default

    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // An Early Bound class generated for the WMI class.SystemRestore
    public class SystemRestore : Component
    {

        // Private property to hold the WMI namespace in which the class resides.
        private const string CreatedWmiNamespace = @"root\default";

        // Private property to hold the name of WMI class which created this class.
        private const string CreatedClassName = "SystemRestore";

        // Private member variable to hold the ManagementScope which is used by the various methods.
        private static ManagementScope _statMgmtScope = new ManagementScope(@"\\.\root\default");

        private ManagementSystemProperties _privateSystemProperties;

        // Underlying lateBound WMI object.
        private ManagementObject _privateLateBoundObject;

        // Member variable to store the 'automatic commit' behavior for the class.
        private bool _autoCommitProp;

        // Private variable to hold the embedded property representing the instance.
        private readonly ManagementBaseObject _embeddedObj;

        // The current WMI object used
        private ManagementBaseObject _curObj;

        // Flag to indicate if the instance is an embedded object.
        private bool _isEmbedded;

        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public SystemRestore()
        {
            InitializeObject(null, null, null);
        }

        public SystemRestore(uint keySequenceNumber)
        {
            InitializeObject(null, new ManagementPath(ConstructPath(keySequenceNumber)), null);
        }

        public SystemRestore(ManagementScope mgmtScope, uint keySequenceNumber)
        {
            InitializeObject(mgmtScope, new ManagementPath(ConstructPath(keySequenceNumber)), null);
        }

        public SystemRestore(ManagementPath path, ObjectGetOptions getOptions)
        {
            InitializeObject(null, path, getOptions);
        }

        public SystemRestore(ManagementScope mgmtScope, ManagementPath path)
        {
            InitializeObject(mgmtScope, path, null);
        }

        public SystemRestore(ManagementPath path)
        {
            InitializeObject(null, path, null);
        }

        public SystemRestore(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions)
        {
            InitializeObject(mgmtScope, path, getOptions);
        }

        public SystemRestore(ManagementObject theObject)
        {
            Initialize();
            if (CheckIfProperClass(theObject))
            {
                _privateLateBoundObject = theObject;
                _privateSystemProperties = new ManagementSystemProperties(_privateLateBoundObject);
                _curObj = _privateLateBoundObject;
            }
            else
            {
                throw new ArgumentException("Class name does not match.");
            }
        }

        public SystemRestore(ManagementBaseObject theObject)
        {
            Initialize();
            if (CheckIfProperClass(theObject))
            {
                _embeddedObj = theObject;
                _privateSystemProperties = new ManagementSystemProperties(theObject);
                _curObj = _embeddedObj;
                _isEmbedded = true;
            }
            else
            {
                throw new ArgumentException("Class name does not match.");
            }
        }

        // Property returns the namespace of the WMI class.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace
        {
            get
            {
                return "root\\default";
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName
        {
            get
            {
                var strRet = CreatedClassName;
                if ((_curObj != null))
                {
                    strRet = ((string)(_curObj["__CLASS"]));
                    if (string.IsNullOrEmpty(strRet))
                    {
                        strRet = CreatedClassName;
                    }
                }
                return strRet;
            }
        }

        // Property pointing to an embedded object to get System properties of the WMI object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementSystemProperties SystemProperties
        {
            get
            {
                return _privateSystemProperties;
            }
        }

        // Property returning the underlying lateBound object.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementBaseObject LateBoundObject
        {
            get
            {
                return _curObj;
            }
        }

        // ManagementScope of the object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementScope Scope
        {
            get
            {
                return (_isEmbedded == false) ? _privateLateBoundObject.Scope : null;
            }
            set
            {
                if ((_isEmbedded == false))
                {
                    _privateLateBoundObject.Scope = value;
                }
            }
        }

        // Property to show the commit behavior for the WMI object. If true, WMI object will be automatically saved after each property modification.(ie. Put() is called after modification of a property).
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit
        {
            get
            {
                return _autoCommitProp;
            }
            set
            {
                _autoCommitProp = value;
            }
        }

        // The ManagementPath of the underlying WMI object.
        [Browsable(true)]
        public ManagementPath Path
        {
            get
            {
                return (_isEmbedded == false) ? _privateLateBoundObject.Path : null;
            }
            set
            {
                if (_isEmbedded) return;
                if ((CheckIfProperClass(null, value, null) != true))
                {
                    throw new ArgumentException("Class name does not match.");
                }
                _privateLateBoundObject.Path = value;
            }
        }

        // Public static scope property which is used by the various methods.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static ManagementScope StaticScope
        {
            get
            {
                return _statMgmtScope;
            }
            set
            {
                _statMgmtScope = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CreationTime
        {
            get
            {
                return ((string)(_curObj["CreationTime"]));
            }
            set
            {
                _curObj["CreationTime"] = value;
                if (((_isEmbedded == false)
                            && _autoCommitProp))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Description
        {
            get
            {
                return ((string)(_curObj["Description"]));
            }
            set
            {
                _curObj["Description"] = value;
                if (((_isEmbedded == false)
                            && _autoCommitProp))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEventTypeNull
        {
            get
            {
                return (_curObj["EventType"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint EventType
        {
            get
            {
                if ((_curObj["EventType"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(_curObj["EventType"]));
            }
            set
            {
                _curObj["EventType"] = value;
                if ((_isEmbedded || !_autoCommitProp)) return;
                _privateLateBoundObject.Put();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRestorePointTypeNull
        {
            get
            {
                return (_curObj["RestorePointType"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint RestorePointType
        {
            get
            {
                if ((_curObj["RestorePointType"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(_curObj["RestorePointType"]));
            }
            set
            {
                _curObj["RestorePointType"] = value;
                if (((_isEmbedded == false)
                            && _autoCommitProp))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSequenceNumberNull
        {
            get
            {
                return (_curObj["SequenceNumber"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint SequenceNumber
        {
            get
            {
                if ((_curObj["SequenceNumber"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(_curObj["SequenceNumber"]));
            }
        }

        private bool CheckIfProperClass(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions optionsParam)
        {
            if (((path != null)
                        && (string.Compare(path.ClassName, ManagementClassName, true, CultureInfo.InvariantCulture) == 0)))
            {
                return true;
            }
            return CheckIfProperClass(new ManagementObject(mgmtScope, path, optionsParam));
        }

        private bool CheckIfProperClass(ManagementBaseObject theObj)
        {
            if (((theObj != null)
                        && (string.Compare(((string)(theObj["__CLASS"])), ManagementClassName, true, CultureInfo.InvariantCulture) == 0)))
            {
                return true;
            }
            if (theObj != null)
            {
                var parentClasses = ((Array)(theObj["__DERIVATION"]));
                if ((parentClasses != null))
                {
                    int count;
                    for (count = 0; (count < parentClasses.Length); count = (count + 1))
                    {
                        if ((string.Compare(((string)(parentClasses.GetValue(count))), ManagementClassName, true, CultureInfo.InvariantCulture) == 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void ResetCreationTime()
        {
            _curObj["CreationTime"] = null;
            if (((_isEmbedded == false)
                        && _autoCommitProp))
            {
                _privateLateBoundObject.Put();
            }
        }

        private void ResetDescription()
        {
            _curObj["Description"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeEventType()
        {
            if ((IsEventTypeNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetEventType()
        {
            _curObj["EventType"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeRestorePointType()
        {
            if ((IsRestorePointTypeNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetRestorePointType()
        {
            _curObj["RestorePointType"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeSequenceNumber()
        {
            if ((IsSequenceNumberNull == false))
            {
                return true;
            }
            return false;
        }

        [Browsable(true)]
        public void CommitObject()
        {
            if ((_isEmbedded == false))
            {
                _privateLateBoundObject.Put();
            }
        }

        [Browsable(true)]
        public void CommitObject(PutOptions putOptions)
        {
            if ((_isEmbedded == false))
            {
                _privateLateBoundObject.Put(putOptions);
            }
        }

        private void Initialize()
        {
            _autoCommitProp = true;
            _isEmbedded = false;
        }

        private static string ConstructPath(uint keySequenceNumber)
        {
            var strPath = "root\\default:SystemRestore";
            strPath = string.Concat(strPath, string.Concat(".SequenceNumber=", keySequenceNumber.ToString(CultureInfo.InvariantCulture)));
            return strPath;
        }

        private void InitializeObject(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions)
        {
            Initialize();
            if ((path != null))
            {
                if ((CheckIfProperClass(mgmtScope, path, getOptions) != true))
                {
                    throw new ArgumentException("Class name does not match.");
                }
            }
            _privateLateBoundObject = new ManagementObject(mgmtScope, path, getOptions);
            _privateSystemProperties = new ManagementSystemProperties(_privateLateBoundObject);
            _curObj = _privateLateBoundObject;
        }

        // Different overloads of GetInstances() help in enumerating instances of the WMI class.
        public static SystemRestoreCollection GetInstances()
        {
            return GetInstances(null, null, null);
        }

        public static SystemRestoreCollection GetInstances(string condition)
        {
            return GetInstances(null, condition, null);
        }

        public static SystemRestoreCollection GetInstances(string[] selectedProperties)
        {
            return GetInstances(null, null, selectedProperties);
        }

        public static SystemRestoreCollection GetInstances(string condition, string[] selectedProperties)
        {
            return GetInstances(null, condition, selectedProperties);
        }

        public static SystemRestoreCollection GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions)
        {
            if ((mgmtScope == null))
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = "root\\default" } };
            }
            var pathObj = new ManagementPath { ClassName = "SystemRestore", NamespacePath = "root\\default" };
            var clsObject = new ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null))
            {
                enumOptions = new EnumerationOptions { EnsureLocatable = true };
            }
            return new SystemRestoreCollection(clsObject.GetInstances(enumOptions));
        }

        public static SystemRestoreCollection GetInstances(ManagementScope mgmtScope, string condition)
        {
            return GetInstances(mgmtScope, condition, null);
        }

        public static SystemRestoreCollection GetInstances(ManagementScope mgmtScope, string[] selectedProperties)
        {
            return GetInstances(mgmtScope, null, selectedProperties);
        }

        public static SystemRestoreCollection GetInstances(ManagementScope mgmtScope, string condition, string[] selectedProperties)
        {
            if ((mgmtScope == null))
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = "root\\default" } };
            }
            using (var objectSearcher = new ManagementObjectSearcher(mgmtScope, new SelectQuery("SystemRestore", condition, selectedProperties)))
            {
                var enumOptions = new EnumerationOptions { EnsureLocatable = true };
                objectSearcher.Options = enumOptions;
                return new SystemRestoreCollection(objectSearcher.Get());
            }
        }

        [Browsable(true)]
        public static SystemRestore CreateInstance()
        {
            ManagementScope mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = CreatedWmiNamespace } };
            var mgmtPath = new ManagementPath(CreatedClassName);
            var tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null);
            return new SystemRestore(tmpMgmtClass.CreateInstance());
        }

        [Browsable(true)]
        public void Delete()
        {
            _privateLateBoundObject.Delete();
        }

        public static uint CreateRestorePoint(string description, uint eventType, uint restorePointType)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, new ObjectGetOptions());
            var inParams = classObj.GetMethodParameters("CreateRestorePoint");
            inParams["Description"] = description;
            inParams["EventType"] = eventType;
            inParams["RestorePointType"] = restorePointType;
            var outParams = classObj.InvokeMethod("CreateRestorePoint", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint Disable(string drive)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("Disable");
            inParams["Drive"] = drive;
            var outParams = classObj.InvokeMethod("Disable", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint Enable(string drive, bool waitTillEnabled)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("Enable");
            inParams["Drive"] = drive;
            inParams["WaitTillEnabled"] = waitTillEnabled;
            var outParams = classObj.InvokeMethod("Enable", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint GetLastRestoreStatus()
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var outParams = classObj.InvokeMethod("GetLastRestoreStatus", null, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint Restore(uint sequenceNumber)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("Restore");
            inParams["SequenceNumber"] = sequenceNumber;
            var outParams = classObj.InvokeMethod("Restore", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        // Enumerator implementation for enumerating instances of the class.
        public class SystemRestoreCollection : object, ICollection
        {

            private readonly ManagementObjectCollection _privColObj;

            public SystemRestoreCollection(ManagementObjectCollection objCollection)
            {
                _privColObj = objCollection;
            }

            public virtual int Count
            {
                get
                {
                    return _privColObj.Count;
                }
            }

            public virtual bool IsSynchronized
            {
                get
                {
                    return _privColObj.IsSynchronized;
                }
            }

            public virtual object SyncRoot
            {
                get
                {
                    return this;
                }
            }

            public virtual void CopyTo(Array array, int index)
            {
                _privColObj.CopyTo(array, index);
                int nCtr;
                for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1))
                {
                    array.SetValue(new SystemRestore(((ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }

            public virtual IEnumerator GetEnumerator()
            {
                return new SystemRestoreEnumerator(_privColObj.GetEnumerator());
            }

            public class SystemRestoreEnumerator : object, IEnumerator
            {

                private readonly ManagementObjectCollection.ManagementObjectEnumerator _privObjEnum;

                public SystemRestoreEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum)
                {
                    _privObjEnum = objEnum;
                }

                public virtual object Current
                {
                    get
                    {
                        return new SystemRestore(((ManagementObject)(_privObjEnum.Current)));
                    }
                }

                public virtual bool MoveNext()
                {
                    return _privObjEnum.MoveNext();
                }

                public virtual void Reset()
                {
                    _privObjEnum.Reset();
                }
            }
        }

        // TypeConverter to handle null values for ValueType properties
        public class WmiValueTypeConverter : TypeConverter
        {

            private readonly TypeConverter _baseConverter;

            private readonly Type _baseType;

            public WmiValueTypeConverter(Type inBaseType)
            {
                _baseConverter = TypeDescriptor.GetConverter(inBaseType);
                _baseType = inBaseType;
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
            {
                return _baseConverter.CanConvertFrom(context, srcType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return _baseConverter.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return _baseConverter.ConvertFrom(context, culture, value);
            }

            public override object CreateInstance(ITypeDescriptorContext context, IDictionary dictionary)
            {
                return _baseConverter.CreateInstance(context, dictionary);
            }

            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
            {
                return _baseConverter.GetCreateInstanceSupported(context);
            }

            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributeVar)
            {
                return _baseConverter.GetProperties(context, value, attributeVar);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return _baseConverter.GetPropertiesSupported(context);
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return _baseConverter.GetStandardValues(context);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return _baseConverter.GetStandardValuesExclusive(context);
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return _baseConverter.GetStandardValuesSupported(context);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if ((_baseType.BaseType == typeof(Enum)))
                {
                    return (value.GetType() == destinationType) ? value : _baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((_baseType == typeof(bool))
                            && (_baseType.BaseType == typeof(ValueType))))
                {
                    if (context.PropertyDescriptor != null && (((value == null))
                                                               && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
                    {
                        return "";
                    }
                    return _baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (context.PropertyDescriptor != null && ((context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false)))
                {
                    return "";
                }
                return _baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }

        // Embedded class to represent WMI system Properties.
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ManagementSystemProperties
        {

            private readonly ManagementBaseObject _privateLateBoundObject;

            public ManagementSystemProperties(ManagementBaseObject managedObject)
            {
                _privateLateBoundObject = managedObject;
            }

            [Browsable(true)]
            public int Genus
            {
                get
                {
                    return ((int)(_privateLateBoundObject["__GENUS"]));
                }
            }

            [Browsable(true)]
            public string Class
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__CLASS"]));
                }
            }

            [Browsable(true)]
            public string Superclass
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__SUPERCLASS"]));
                }
            }

            [Browsable(true)]
            public string Dynasty
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__DYNASTY"]));
                }
            }

            [Browsable(true)]
            public string Relpath
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__RELPATH"]));
                }
            }

            [Browsable(true)]
            public int PropertyCount
            {
                get
                {
                    return ((int)(_privateLateBoundObject["__PROPERTY_COUNT"]));
                }
            }

            [Browsable(true)]
            public string[] Derivation
            {
                get
                {
                    return ((string[])(_privateLateBoundObject["__DERIVATION"]));
                }
            }

            [Browsable(true)]
            public string Server
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__SERVER"]));
                }
            }

            [Browsable(true)]
            public string Namespace
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__NAMESPACE"]));
                }
            }

            [Browsable(true)]
            public string ClassPath
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__PATH"]));
                }
            }
        }
    }
}
