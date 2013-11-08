using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Collections;

namespace Org.InCommon.InCert.Engine.NativeCode.Wmi
{
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // Datetime conversion functions ToDateTime and ToDmtfDateTime are added to the class to convert DMTF datetime to DateTime and vice-versa.
    // An Early Bound class generated for the WMI class.Win32_ComputerSystem
    public class ComputerSystem : Component
    {

        // Private property to hold the WMI namespace in which the class resides.
        private const string CreatedWmiNamespace = "root\\CimV2";

        // Private property to hold the name of WMI class which created this class.
        private const string CreatedClassName = "Win32_ComputerSystem";

        // Private member variable to hold the ManagementScope which is used by the various methods.
        private static ManagementScope _statMgmtScope;

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

        /// <summary>
        /// Gets the computer system instance for the local machine
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ComputerSystem GetComputerSystem()
        {
            return (
                from object computer in GetInstances() 
                select computer as ComputerSystem).FirstOrDefault();
        }


        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public ComputerSystem()
        {
            InitializeObject(null, null, null);
        }

        public ComputerSystem(string keyName)
        {
            InitializeObject(null, new ManagementPath(ConstructPath(keyName)), null);
        }

        public ComputerSystem(ManagementScope mgmtScope, string keyName)
        {
            InitializeObject(mgmtScope, new ManagementPath(ConstructPath(keyName)), null);
        }

        public ComputerSystem(ManagementPath path, ObjectGetOptions getOptions)
        {
            InitializeObject(null, path, getOptions);
        }

        public ComputerSystem(ManagementScope mgmtScope, ManagementPath path)
        {
            InitializeObject(mgmtScope, path, null);
        }

        public ComputerSystem(ManagementPath path)
        {
            InitializeObject(null, path, null);
        }

        public ComputerSystem(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions)
        {
            InitializeObject(mgmtScope, path, getOptions);
        }

        public ComputerSystem(ManagementObject theObject)
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

        public ComputerSystem(ManagementBaseObject theObject)
        {
            Initialize();
            if ((!CheckIfProperClass(theObject)))
            {
                throw new ArgumentException("Class name does not match.");
            }
            _embeddedObj = theObject;
            _privateSystemProperties = new ManagementSystemProperties(theObject);
            _curObj = _embeddedObj;
            _isEmbedded = true;
        }

        // Property returns the namespace of the WMI class.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace
        {
            get
            {
                return "root\\CimV2";
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAdminPasswordStatusNull
        {
            get
            {
                return (_curObj["AdminPasswordStatus"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The AdminPasswordStatus property identifies the system-wide hardware security set" +
            "tings for Administrator Password Status.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public AdminPasswordStatusValues AdminPasswordStatus
        {
            get
            {
                if ((_curObj["AdminPasswordStatus"] == null))
                {
                    return ((AdminPasswordStatusValues)(Convert.ToInt32(4)));
                }
                return ((AdminPasswordStatusValues)(Convert.ToInt32(_curObj["AdminPasswordStatus"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAutomaticManagedPagefileNull
        {
            get
            {
                return (_curObj["AutomaticManagedPagefile"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The AutomaticManagedPagefile property determines whether the system managed pagef" +
            "ile is enabled. This capability is notAvailable on windows server 2003,XP and lo" +
            "wer versions.\nValues: TRUE or FALSE. If TRUE, the automatic managed pagefile is " +
            "enabled.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool AutomaticManagedPagefile
        {
            get
            {
                if ((_curObj["AutomaticManagedPagefile"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["AutomaticManagedPagefile"]));
            }
            set
            {
                _curObj["AutomaticManagedPagefile"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAutomaticResetBootOptionNull
        {
            get
            {
                return (_curObj["AutomaticResetBootOption"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The AutomaticResetBootOption property determines whether the automatic reset boot" +
            " option is enabled, i.e. whether the machine will try to reboot after a system f" +
            "ailure.\nValues: TRUE or FALSE. If TRUE, the automatic reset boot option is enabl" +
            "ed.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool AutomaticResetBootOption
        {
            get
            {
                if ((_curObj["AutomaticResetBootOption"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["AutomaticResetBootOption"]));
            }
            set
            {
                _curObj["AutomaticResetBootOption"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAutomaticResetCapabilityNull
        {
            get
            {
                return (_curObj["AutomaticResetCapability"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The AutomaticResetCapability property determines whether the auto reboot feature " +
            "is available with this machine. This capability is available on Windows NT but n" +
            "ot on Windows 95.\nValues: TRUE or FALSE. If TRUE, the automatic reset is enabled" +
            ".")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool AutomaticResetCapability
        {
            get
            {
                if ((_curObj["AutomaticResetCapability"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["AutomaticResetCapability"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBootOptionOnLimitNull
        {
            get
            {
                return (_curObj["BootOptionOnLimit"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Boot Option on Limit. Identifies the system action to be taken when the Reset Lim" +
            "it is reached.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public BootOptionOnLimitValues BootOptionOnLimit
        {
            get
            {
                if ((_curObj["BootOptionOnLimit"] == null))
                {
                    return ((BootOptionOnLimitValues)(Convert.ToInt32(4)));
                }
                return ((BootOptionOnLimitValues)(Convert.ToInt32(_curObj["BootOptionOnLimit"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBootOptionOnWatchDogNull
        {
            get
            {
                return (_curObj["BootOptionOnWatchDog"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The BootOptionOnWatchDog Property indicates the type of re-boot action to be take" +
            "n after the time on the watchdog timer has elapsed.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public BootOptionOnWatchDogValues BootOptionOnWatchDog
        {
            get
            {
                if ((_curObj["BootOptionOnWatchDog"] == null))
                {
                    return ((BootOptionOnWatchDogValues)(Convert.ToInt32(4)));
                }
                return ((BootOptionOnWatchDogValues)(Convert.ToInt32(_curObj["BootOptionOnWatchDog"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsBootRomSupportedNull
        {
            get
            {
                return (_curObj["BootROMSupported"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The BootROMSupported property determines whether a boot ROM is supported.\nValues " +
            "are TRUE or FALSE. If BootROMSupported equals TRUE, then a boot ROM is supported" +
            ".")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool BootRomSupported
        {
            get
            {
                if ((_curObj["BootROMSupported"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["BootROMSupported"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The BootupState property specifies how the system was started. Fail-safe boot (al" +
            "so called SafeBoot) bypasses the user\'s startup files. \nConstraints: Must have a" +
            " value.")]
        public string BootupState
        {
            get
            {
                return ((string)(_curObj["BootupState"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Caption property is a short textual description (one-line string) of the obje" +
            "ct.")]
        public string Caption
        {
            get
            {
                return ((string)(_curObj["Caption"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsChassisBootupStateNull
        {
            get
            {
                return (_curObj["ChassisBootupState"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The ChassisBootupState property indicates the enclosure\'s bootup state.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ChassisBootupStateValues ChassisBootupState
        {
            get
            {
                if ((_curObj["ChassisBootupState"] == null))
                {
                    return ((ChassisBootupStateValues)(Convert.ToInt32(0)));
                }
                return ((ChassisBootupStateValues)(Convert.ToInt32(_curObj["ChassisBootupState"])));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The CreationClassName property indicates the name of the class or the subclass used in the creation of an instance. When used with the other key properties of this class, this property allows all instances of this class and its subclasses to be uniquely identified.")]
        public string CreationClassName
        {
            get
            {
                return ((string)(_curObj["CreationClassName"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCurrentTimeZoneNull
        {
            get
            {
                return (_curObj["CurrentTimeZone"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The CurrentTimeZone property  indicates the amount of time the unitary computer s" +
            "ystem is offset from Coordinated Universal Time.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public short CurrentTimeZone
        {
            get
            {
                if ((_curObj["CurrentTimeZone"] == null))
                {
                    return Convert.ToInt16(0);
                }
                return ((short)(_curObj["CurrentTimeZone"]));
            }
            set
            {
                _curObj["CurrentTimeZone"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDaylightInEffectNull
        {
            get
            {
                return (_curObj["DaylightInEffect"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DaylightInEffect property specifies if the daylight savings is in effect. \nVa" +
            "lues: TRUE or FALSE.  If TRUE, daylight savings is presently being observed.  In" +
            " most cases this means that the current time is one hour earlier than the standa" +
            "rd time.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool DaylightInEffect
        {
            get
            {
                if ((_curObj["DaylightInEffect"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["DaylightInEffect"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Description property provides a textual description of the object. ")]
        public string Description
        {
            get
            {
                return ((string)(_curObj["Description"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DNSHostName property indicates the DNS host name of the local computer.")]
        public string DnsHostName
        {
            get
            {
                return ((string)(_curObj["DNSHostName"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Domain property indicates the name of the domain to which the computer belong" +
            "s.")]
        public string Domain
        {
            get
            {
                return ((string)(_curObj["Domain"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDomainRoleNull
        {
            get
            {
                return (_curObj["DomainRole"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DomainRole property indicates the role this computer plays within its assigned domain-workgroup. The domain-workgroup is a collection of computers on the same network.  For example, the DomainRole property may show this computer is a ""Member Workstation"" (value of [1]).")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public DomainRoleValues DomainRole
        {
            get
            {
                if ((_curObj["DomainRole"] == null))
                {
                    return ((DomainRoleValues)(Convert.ToInt32(6)));
                }
                return ((DomainRoleValues)(Convert.ToInt32(_curObj["DomainRole"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEnableDaylightSavingsTimeNull
        {
            get
            {
                return (_curObj["EnableDaylightSavingsTime"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The EnableDaylightSavingsTime property indicates whether Daylight Savings Time is" +
            " recognized on this machine.  FALSE - time does not move an hour ahead or behind" +
            " in the year.  NULL - the status of DST is unknown on this system")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool EnableDaylightSavingsTime
        {
            get
            {
                if ((_curObj["EnableDaylightSavingsTime"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["EnableDaylightSavingsTime"]));
            }
            set
            {
                _curObj["EnableDaylightSavingsTime"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFrontPanelResetStatusNull
        {
            get
            {
                return (_curObj["FrontPanelResetStatus"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The FrontPanelResetStatus property identifies the hardware security settings for " +
            "the reset button on the machine.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public FrontPanelResetStatusValues FrontPanelResetStatus
        {
            get
            {
                if ((_curObj["FrontPanelResetStatus"] == null))
                {
                    return ((FrontPanelResetStatusValues)(Convert.ToInt32(4)));
                }
                return ((FrontPanelResetStatusValues)(Convert.ToInt32(_curObj["FrontPanelResetStatus"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHypervisorPresentNull
        {
            get
            {
                return (_curObj["HypervisorPresent"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The HypervisorPresent property determines whether the system is running under a h" +
            "ypervisor that follows the industry standard convention for reporting a hypervis" +
            "or is present.\nValues: TRUE or FALSE. If TRUE, a hypervisor is present.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool HypervisorPresent
        {
            get
            {
                if ((_curObj["HypervisorPresent"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["HypervisorPresent"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInfraredSupportedNull
        {
            get
            {
                return (_curObj["InfraredSupported"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The InfraredSupported property determines whether an infrared (IR) port exists on" +
            " the computer \nValues are TRUE or FALSE. If InfraredSupported equals TRUE" +
            ", then an IR port exists.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool InfraredSupported
        {
            get
            {
                if ((_curObj["InfraredSupported"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["InfraredSupported"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This object contains the data needed to find either the initial load device (its " +
            "key) or the boot service to request the operating system to start up. In additio" +
            "n, the load parameters (ie, a pathname and parameters) may also be specified.")]
        public string[] InitialLoadInfo
        {
            get
            {
                return ((string[])(_curObj["InitialLoadInfo"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInstallDateNull
        {
            get
            {
                return (_curObj["InstallDate"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The InstallDate property is datetime value indicating when the object was install" +
            "ed. A lack of a value does not indicate that the object is not installed.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public DateTime InstallDate
        {
            get
            {
                return (_curObj["InstallDate"] != null) ? ToDateTime(((string)(_curObj["InstallDate"]))) : DateTime.MinValue;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsKeyboardPasswordStatusNull
        {
            get
            {
                return (_curObj["KeyboardPasswordStatus"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The KeyboardPasswordStatus property identifies the system-wide hardware security " +
            "settings for Keyboard Password Status.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public KeyboardPasswordStatusValues KeyboardPasswordStatus
        {
            get
            {
                if ((_curObj["KeyboardPasswordStatus"] == null))
                {
                    return ((KeyboardPasswordStatusValues)(Convert.ToInt32(4)));
                }
                return ((KeyboardPasswordStatusValues)(Convert.ToInt32(_curObj["KeyboardPasswordStatus"])));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This object contains the data identifying either the initial load device (its key" +
            ") or the boot service that requested the last operating system load. In addition" +
            ", the load parameters (ie, a pathname and parameters) may also be specified.")]
        public string LastLoadInfo
        {
            get
            {
                return ((string)(_curObj["LastLoadInfo"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Manufacturer property indicates the name of the computer manufacturer.\nExampl" +
            "e: Acme")]
        public string Manufacturer
        {
            get
            {
                return ((string)(_curObj["Manufacturer"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Model property indicates the product name of the computer given by the manufa" +
            "cturer.")]
        public string Model
        {
            get
            {
                return ((string)(_curObj["Model"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Name property defines the label by which the object is known. When subclassed" +
            ", the Name property can be overridden to be a Key property.")]
        public string Name
        {
            get
            {
                return ((string)(_curObj["Name"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The CIM_ComputerSystem object and its derivatives are Top Level Objects of CIM. They provide the scope for numerous components. Having unique CIM_System keys is required. A heuristic is defined to create the CIM_ComputerSystem name to attempt to always generate the same name, independent of discovery protocol. This prevents inventory and management problems where the same asset or entity is discovered multiple times, but can not be resolved to a single object. Use of the heuristic is optional, but recommended. 

 The NameFormat property identifies how the computer system name is generated, using a heuristic. The heuristic is outlined, in detail, in the CIM V2 Common Model specification. It assumes that the documented rules are traversed in order, to determine and assign a name. The NameFormat values list defines the precedence order for assigning the computer system name. Several rules do map to the same Value. 

 Note that the CIM_ComputerSystem Name calculated using the heuristic is the system's key value. Other names can be assigned and used for the CIM_ComputerSystem that better suit the business, using Aliases.")]
        public string NameFormat
        {
            get
            {
                return ((string)(_curObj["NameFormat"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNetworkServerModeEnabledNull
        {
            get
            {
                return (_curObj["NetworkServerModeEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The NetworkServerModeEnabled property determines whether Network Server Mode is e" +
            "nabled.\nValues: TRUE or FALSE.  If TRUE, Network Server Mode is enabled.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool NetworkServerModeEnabled
        {
            get
            {
                if ((_curObj["NetworkServerModeEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["NetworkServerModeEnabled"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumberOfLogicalProcessorsNull
        {
            get
            {
                return (_curObj["NumberOfLogicalProcessors"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The NumberOfLogicalProcessors property indicates the number of logical processors" +
            " currently available on the ")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint NumberOfLogicalProcessors
        {
            get
            {
                if ((_curObj["NumberOfLogicalProcessors"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(_curObj["NumberOfLogicalProcessors"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumberOfProcessorsNull
        {
            get
            {
                return (_curObj["NumberOfProcessors"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The NumberOfProcessors property indicates the number of physical processors currently available on the  This is the number of processors whose status is ""enabled"" - versus simply the number of processors for the computer  The former can be determined by enumerating the number of processor instances associated with the computer system object, using the Win32_ComputerSystemProcessor association.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint NumberOfProcessors
        {
            get
            {
                if ((_curObj["NumberOfProcessors"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(_curObj["NumberOfProcessors"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The OEMLogoBitmap array holds the data for a bitmap created by the OEM.")]
        public byte[] OemLogoBitmap
        {
            get
            {
                return ((byte[])(_curObj["OEMLogoBitmap"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This structure contains free form strings defined by the OEM. Examples of this ar" +
            "e: Part Numbers for Reference Documents for the system, contact information for " +
            "the manufacturer, etc.")]
        public string[] OemStringArray
        {
            get
            {
                return ((string[])(_curObj["OEMStringArray"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPartOfDomainNull
        {
            get
            {
                return (_curObj["PartOfDomain"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The PartOfDomain property indicates whether the computer is part of a domain or w" +
            "orkgroup.  If TRUE, the computer is part of a domain.  If FALSE, the computer is" +
            " part of a workgroup.  If NULL, the computer is not part of a network group, or " +
            "is unknown.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool PartOfDomain
        {
            get
            {
                if ((_curObj["PartOfDomain"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["PartOfDomain"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPauseAfterResetNull
        {
            get
            {
                return (_curObj["PauseAfterReset"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The PauseAfterReset property identifies the time delay before the reboot is initi" +
            "ated.  It is used after a system power cycle, system reset (local or remote), an" +
            "d automatic system reset.  A value of -1 indicates that the pause value is unkno" +
            "wn")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public long PauseAfterReset
        {
            get
            {
                if ((_curObj["PauseAfterReset"] == null))
                {
                    return Convert.ToInt64(0);
                }
                return ((long)(_curObj["PauseAfterReset"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPcSystemTypeNull
        {
            get
            {
                return (_curObj["PCSystemType"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The PCSystemType property indicates the nature of the PC a user is working with l" +
            "ike Laptop, Desktop, Tablet-PC etc. ")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public PcSystemTypeValues PcSystemType
        {
            get
            {
                if ((_curObj["PCSystemType"] == null))
                {
                    return ((PcSystemTypeValues)(Convert.ToInt32(9)));
                }
                return ((PcSystemTypeValues)(Convert.ToInt32(_curObj["PCSystemType"])));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Indicates the specific power-related capabilities of a computer system and its associated running operating  The values, 0=""Unknown"", 1=""Not Supported"", and 2=""Disabled"" are self-explanatory. The value, 3=""Enabled"" indicates that the power management features are currently enabled but the exact feature set is unknown or the information is unavailable. ""Power Saving Modes Entered Automatically"" (4) describes that a system can change its power state based on usage or other criteria. ""Power State Settable"" (5) indicates that the SetPowerState method is supported. ""Power Cycling Supported"" (6) indicates that the SetPowerState method can be invoked with the PowerState parameter set to 5 (""Power Cycle""). ""Timed Power On Supported"" (7) indicates that the SetPowerState method can be invoked with the PowerState parameter set to 5 (""Power Cycle"") and the Time parameter set to a specific date and time, or interval, for power-on.")]
        public PowerManagementCapabilitiesValues[] PowerManagementCapabilities
        {
            get
            {
                var arrEnumVals = ((Array)(_curObj["PowerManagementCapabilities"]));
                var enumToRet = new PowerManagementCapabilitiesValues[arrEnumVals.Length];
                for (var counter = 0; (counter < arrEnumVals.Length); counter = (counter + 1))
                {
                    enumToRet[counter] = ((PowerManagementCapabilitiesValues)(Convert.ToInt32(arrEnumVals.GetValue(counter))));
                }
                return enumToRet;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPowerManagementSupportedNull
        {
            get
            {
                return (_curObj["PowerManagementSupported"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Boolean indicating that the ComputerSystem, with its running OperatingSystem, supports power management. This boolean does not indicate that power management features are currently enabled, or if enabled, what features are supported. Refer to the PowerManagementCapabilities array for this information. If this boolean is false, the integer value 1 for the string, ""Not Supported"", should be the only entry in the PowerManagementCapabilities array.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool PowerManagementSupported
        {
            get
            {
                if ((_curObj["PowerManagementSupported"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["PowerManagementSupported"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPowerOnPasswordStatusNull
        {
            get
            {
                return (_curObj["PowerOnPasswordStatus"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The PowerOnPasswordStatus property identifies the system-wide hardware security s" +
            "ettings for Power On Password Status.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public PowerOnPasswordStatusValues PowerOnPasswordStatus
        {
            get
            {
                if ((_curObj["PowerOnPasswordStatus"] == null))
                {
                    return ((PowerOnPasswordStatusValues)(Convert.ToInt32(4)));
                }
                return ((PowerOnPasswordStatusValues)(Convert.ToInt32(_curObj["PowerOnPasswordStatus"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPowerStateNull
        {
            get
            {
                return (_curObj["PowerState"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"Indicates the current power state of the computer system and its associated operating  Regarding the power saving states, these are defined as follows: Value 4 (Unknown) indicates that the system is known to be in a power save mode, but its exact status in this mode is unknown; 2 (Low Power Mode) indicates that the system is in a power save state but still functioning, and may exhibit degraded performance; 3 (Standby) describes that the system is not functioning but could be brought to full power 'quickly'; and value 7 (Warning) indicates that the computerSystem is in a warning state, though also in a power save mode.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public PowerStateValues PowerState
        {
            get
            {
                if ((_curObj["PowerState"] == null))
                {
                    return ((PowerStateValues)(Convert.ToInt32(10)));
                }
                return ((PowerStateValues)(Convert.ToInt32(_curObj["PowerState"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPowerSupplyStateNull
        {
            get
            {
                return (_curObj["PowerSupplyState"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The PowerSupplyState identifies the state of the enclosure\'s power supply (or sup" +
            "plies) when last booted.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public PowerSupplyStateValues PowerSupplyState
        {
            get
            {
                if ((_curObj["PowerSupplyState"] == null))
                {
                    return ((PowerSupplyStateValues)(Convert.ToInt32(0)));
                }
                return ((PowerSupplyStateValues)(Convert.ToInt32(_curObj["PowerSupplyState"])));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A string that provides information on how the primary system owner can be reached" +
            " (e.g. phone number, email address, ...).")]
        public string PrimaryOwnerContact
        {
            get
            {
                return ((string)(_curObj["PrimaryOwnerContact"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The name of the primary system owner.")]
        public string PrimaryOwnerName
        {
            get
            {
                return ((string)(_curObj["PrimaryOwnerName"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsResetCapabilityNull
        {
            get
            {
                return (_curObj["ResetCapability"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"If enabled (value = 4), the unitary computer system can be reset via hardware (e.g. the power and reset buttons). If disabled (value = 3), hardware reset is not allowed. In addition to Enabled and Disabled, other values for the property are also defined - ""Not Implemented"" (5), ""Other"" (1) and ""Unknown"" (2).")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ResetCapabilityValues ResetCapability
        {
            get
            {
                if ((_curObj["ResetCapability"] == null))
                {
                    return ((ResetCapabilityValues)(Convert.ToInt32(0)));
                }
                return ((ResetCapabilityValues)(Convert.ToInt32(_curObj["ResetCapability"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsResetCountNull
        {
            get
            {
                return (_curObj["ResetCount"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The ResetCount property indicates the number of automatic resets since the last i" +
            "ntentional reset.  A value of -1 indicates that the count is unknown.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public short ResetCount
        {
            get
            {
                if ((_curObj["ResetCount"] == null))
                {
                    return Convert.ToInt16(0);
                }
                return ((short)(_curObj["ResetCount"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsResetLimitNull
        {
            get
            {
                return (_curObj["ResetLimit"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The ResetLimit property indicates the number of consecutive time the system reset" +
            " will be attempted. A value of -1 indicates that the limit is unknown")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public short ResetLimit
        {
            get
            {
                if ((_curObj["ResetLimit"] == null))
                {
                    return Convert.ToInt16(0);
                }
                return ((short)(_curObj["ResetLimit"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"An array (bag) of strings that specify the roles this System plays in the IT-environment. Subclasses of System may override this property to define explicit Roles values. Alternately, a Working Group may describe the heuristics, conventions and guidelines for specifying Roles. For example, for an instance of a networking system, the Roles property might contain the string, 'Switch' or 'Bridge'.")]
        public string[] Roles
        {
            get
            {
                return ((string[])(_curObj["Roles"]));
            }
            set
            {
                _curObj["Roles"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The Status property is a string indicating the current status of the object. Various operational and non-operational statuses can be defined. Operational statuses are ""OK"", ""Degraded"" and ""Pred Fail"". ""Pred Fail"" indicates that an element may be functioning properly but predicting a failure in the near future. An example is a SMART-enabled hard drive. Non-operational statuses can also be specified. These are ""Error"", ""Starting"", ""Stopping"" and ""Service"". The latter, ""Service"", could apply during mirror-resilvering of a disk, reload of a user permissions list, or other administrative work. Not all such work is on-line, yet the managed element is neither ""OK"" nor in one of the other states.")]
        public string Status
        {
            get
            {
                return ((string)(_curObj["Status"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SupportContactDescription property is an array that indicates the support con" +
            "tact information for the Win32 computer ")]
        public string[] SupportContactDescription
        {
            get
            {
                return ((string[])(_curObj["SupportContactDescription"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSystemStartupDelayNull
        {
            get
            {
                return (_curObj["SystemStartupDelay"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SystemStartupDelay property indicates the time to delay before starting the o" +
            "perating system\n\nNote:  The SE_SYSTEM_ENVIRONMENT privilege is required on IA64b" +
            "it machines. This privilege is not required for 32bit systems.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ushort SystemStartupDelay
        {
            get
            {
                if ((_curObj["SystemStartupDelay"] == null))
                {
                    return Convert.ToUInt16(0);
                }
                return ((ushort)(_curObj["SystemStartupDelay"]));
            }
            set
            {
                _curObj["SystemStartupDelay"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The SystemStartupOptions property array indicates the options for starting up the computer  Note that this property is not writable on IA64 bit machines. 
Constraints: Must have a value.

Note:  The SE_SYSTEM_ENVIRONMENT privilege is required on IA64bit machines. This privilege is not required for other systems.")]
        public string[] SystemStartupOptions
        {
            get
            {
                return ((string[])(_curObj["SystemStartupOptions"]));
            }
            set
            {
                _curObj["SystemStartupOptions"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSystemStartupSettingNull
        {
            get
            {
                return (_curObj["SystemStartupSetting"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The SystemStartupSetting property indicates the index of the default start profile. This value is 'calculated' so that it usually returns zero (0) because at write-time, the profile string is physically moved to the top of the list. (This is how Windows NT determines which value is the default.)

Note:  The SE_SYSTEM_ENVIRONMENT privilege is required on IA64bit machines. This privilege is not required for 32bit systems.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public byte SystemStartupSetting
        {
            get
            {
                if ((_curObj["SystemStartupSetting"] == null))
                {
                    return Convert.ToByte(0);
                }
                return ((byte)(_curObj["SystemStartupSetting"]));
            }
            set
            {
                _curObj["SystemStartupSetting"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SystemType property indicates the type of system running on the Win32 compute" +
            "r.\nConstraints: Must have a value")]
        public string SystemType
        {
            get
            {
                return ((string)(_curObj["SystemType"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsThermalStateNull
        {
            get
            {
                return (_curObj["ThermalState"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The ThermalState property identifies the enclosure\'s thermal state when last boot" +
            "ed.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ThermalStateValues ThermalState
        {
            get
            {
                if ((_curObj["ThermalState"] == null))
                {
                    return ((ThermalStateValues)(Convert.ToInt32(0)));
                }
                return ((ThermalStateValues)(Convert.ToInt32(_curObj["ThermalState"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTotalPhysicalMemoryNull
        {
            get
            {
                return (_curObj["TotalPhysicalMemory"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The TotalPhysicalMemory property indicates the total size of physical memory.\nExa" +
            "mple: 67108864")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ulong TotalPhysicalMemory
        {
            get
            {
                if ((_curObj["TotalPhysicalMemory"] == null))
                {
                    return Convert.ToUInt64(0);
                }
                return ((ulong)(_curObj["TotalPhysicalMemory"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The UserName property indicates the name of the currently-logged-on user.\nConstra" +
            "ints: Must have a value. \nExample: johnsmith")]
        public string UserName
        {
            get
            {
                return ((string)(_curObj["UserName"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWakeUpTypeNull
        {
            get
            {
                return (_curObj["WakeUpType"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The WakeUpType property indicates the event that caused the system to power up.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public WakeUpTypeValues WakeUpType
        {
            get
            {
                if ((_curObj["WakeUpType"] == null))
                {
                    return ((WakeUpTypeValues)(Convert.ToInt32(9)));
                }
                return ((WakeUpTypeValues)(Convert.ToInt32(_curObj["WakeUpType"])));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Workgroup property contains the name of the workgroup.  This value is only va" +
            "lid if the PartOfDomain property is FALSE.")]
        public string Workgroup
        {
            get
            {
                return ((string)(_curObj["Workgroup"]));
            }
            set
            {
                _curObj["Workgroup"] = value;
                if (((_isEmbedded == false)
                            && (_autoCommitProp)))
                {
                    _privateLateBoundObject.Put();
                }
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
            if (theObj == null)
                return false;

            if ((string.Compare(((string)(theObj["__CLASS"])), ManagementClassName, true, CultureInfo.InvariantCulture) == 0))
            {
                return true;
            }

            {
                var parentClasses = ((Array)(theObj["__DERIVATION"]));
                if ((parentClasses != null))
                {
                    for (var count = 0; (count < parentClasses.Length); count = (count + 1))
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

        private bool ShouldSerializeAdminPasswordStatus()
        {
            if ((IsAdminPasswordStatusNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeAutomaticManagedPagefile()
        {
            if ((IsAutomaticManagedPagefileNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetAutomaticManagedPagefile()
        {
            _curObj["AutomaticManagedPagefile"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeAutomaticResetBootOption()
        {
            if ((IsAutomaticResetBootOptionNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetAutomaticResetBootOption()
        {
            _curObj["AutomaticResetBootOption"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeAutomaticResetCapability()
        {
            if ((IsAutomaticResetCapabilityNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeBootOptionOnLimit()
        {
            if ((IsBootOptionOnLimitNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeBootOptionOnWatchDog()
        {
            if ((IsBootOptionOnWatchDogNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeBootRomSupported()
        {
            return (IsBootRomSupportedNull == false);
        }

        private bool ShouldSerializeChassisBootupState()
        {
            if ((IsChassisBootupStateNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeCurrentTimeZone()
        {
            if ((IsCurrentTimeZoneNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetCurrentTimeZone()
        {
            _curObj["CurrentTimeZone"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeDaylightInEffect()
        {
            if ((IsDaylightInEffectNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeDomainRole()
        {
            if ((IsDomainRoleNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeEnableDaylightSavingsTime()
        {
            if ((IsEnableDaylightSavingsTimeNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetEnableDaylightSavingsTime()
        {
            _curObj["EnableDaylightSavingsTime"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeFrontPanelResetStatus()
        {
            if ((IsFrontPanelResetStatusNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeHypervisorPresent()
        {
            if ((IsHypervisorPresentNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeInfraredSupported()
        {
            if ((IsInfraredSupportedNull == false))
            {
                return true;
            }
            return false;
        }

        // Converts a given datetime in DMTF format to DateTime object.
        static DateTime ToDateTime(string dmtfDate)
        {
            var initializer = DateTime.MinValue;
            var year = initializer.Year;
            var month = initializer.Month;
            var day = initializer.Day;
            var hour = initializer.Hour;
            var minute = initializer.Minute;
            var second = initializer.Second;
            long ticks = 0;
            var dmtf = dmtfDate;
            string tempString;
            if ((dmtf == null))
            {
                throw new ArgumentOutOfRangeException();
            }
            if ((dmtf.Length == 0))
            {
                throw new ArgumentOutOfRangeException();
            }
            if ((dmtf.Length != 25))
            {
                throw new ArgumentOutOfRangeException();
            }
            try
            {
                tempString = dmtf.Substring(0, 4);
                if (("****" != tempString))
                {
                    year = int.Parse(tempString);
                }
                tempString = dmtf.Substring(4, 2);
                if (("**" != tempString))
                {
                    month = int.Parse(tempString);
                }
                tempString = dmtf.Substring(6, 2);
                if (("**" != tempString))
                {
                    day = int.Parse(tempString);
                }
                tempString = dmtf.Substring(8, 2);
                if (("**" != tempString))
                {
                    hour = int.Parse(tempString);
                }
                tempString = dmtf.Substring(10, 2);
                if (("**" != tempString))
                {
                    minute = int.Parse(tempString);
                }
                tempString = dmtf.Substring(12, 2);
                if (("**" != tempString))
                {
                    second = int.Parse(tempString);
                }
                tempString = dmtf.Substring(15, 6);
                if (("******" != tempString))
                {
                    ticks = (long.Parse(tempString) * (TimeSpan.TicksPerMillisecond / 1000));
                }
                if (((((((((year < 0)
                            || (month < 0))
                            || (day < 0))
                            || (hour < 0))
                            || (minute < 0))
                            || (minute < 0))
                            || (second < 0))
                            || (ticks < 0)))
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException(null, e.Message);
            }
            var datetime = new DateTime(year, month, day, hour, minute, second, 0);
            datetime = datetime.AddTicks(ticks);
            var tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
            var offsetMins = tickOffset.Ticks / TimeSpan.TicksPerMinute;
            tempString = dmtf.Substring(22, 3);
            if ((tempString != "******"))
            {
                tempString = dmtf.Substring(21, 4);
                int utcOffset;
                try
                {
                    utcOffset = int.Parse(tempString);
                }
                catch (Exception e)
                {
                    throw new ArgumentOutOfRangeException(null, e.Message);
                }
                var offsetToBeAdjusted = ((int)((offsetMins - utcOffset)));
                datetime = datetime.AddMinutes(offsetToBeAdjusted);
            }
            return datetime;
        }

        // Converts a given DateTime object to DMTF datetime format.
        static string ToDmtfDateTime(DateTime date)
        {
            string utcString;
            var tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
            var offsetMins = tickOffset.Ticks / TimeSpan.TicksPerMinute;
            if ((Math.Abs(offsetMins) > 999))
            {
                date = date.ToUniversalTime();
                utcString = "+000";
            }
            else
            {
                if ((tickOffset.Ticks >= 0))
                {
                    utcString = string.Concat("+", (tickOffset.Ticks / TimeSpan.TicksPerMinute).ToString(CultureInfo.InvariantCulture).PadLeft(3, '0'));
                }
                else
                {
                    string strTemp = offsetMins.ToString(CultureInfo.InvariantCulture);
                    utcString = string.Concat("-", strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
                }
            }
            string dmtfDateTime = date.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
            dmtfDateTime = string.Concat(dmtfDateTime, date.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, date.Second.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ".");
            var dtTemp = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
            var microsec = ((date.Ticks - dtTemp.Ticks)
                            * 1000)
                           / TimeSpan.TicksPerMillisecond;
            string strMicrosec = microsec.ToString(CultureInfo.InvariantCulture);
            if ((strMicrosec.Length > 6))
            {
                strMicrosec = strMicrosec.Substring(0, 6);
            }
            dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, utcString);
            return dmtfDateTime;
        }

        private bool ShouldSerializeInstallDate()
        {
            if ((IsInstallDateNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeKeyboardPasswordStatus()
        {
            if ((IsKeyboardPasswordStatusNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeNetworkServerModeEnabled()
        {
            if ((IsNetworkServerModeEnabledNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeNumberOfLogicalProcessors()
        {
            if ((IsNumberOfLogicalProcessorsNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeNumberOfProcessors()
        {
            if ((IsNumberOfProcessorsNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePartOfDomain()
        {
            if ((IsPartOfDomainNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePauseAfterReset()
        {
            if ((IsPauseAfterResetNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePcSystemType()
        {
            return (IsPcSystemTypeNull == false);
        }

        private bool ShouldSerializePowerManagementSupported()
        {
            if ((IsPowerManagementSupportedNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePowerOnPasswordStatus()
        {
            if ((IsPowerOnPasswordStatusNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePowerState()
        {
            if ((IsPowerStateNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializePowerSupplyState()
        {
            if ((IsPowerSupplyStateNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeResetCapability()
        {
            if ((IsResetCapabilityNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeResetCount()
        {
            if ((IsResetCountNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeResetLimit()
        {
            if ((IsResetLimitNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetRoles()
        {
            _curObj["Roles"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeSystemStartupDelay()
        {
            if ((IsSystemStartupDelayNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetSystemStartupDelay()
        {
            _curObj["SystemStartupDelay"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private void ResetSystemStartupOptions()
        {
            _curObj["SystemStartupOptions"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeSystemStartupSetting()
        {
            if ((IsSystemStartupSettingNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetSystemStartupSetting()
        {
            _curObj["SystemStartupSetting"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
        }

        private bool ShouldSerializeThermalState()
        {
            if ((IsThermalStateNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeTotalPhysicalMemory()
        {
            if ((IsTotalPhysicalMemoryNull == false))
            {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeWakeUpType()
        {
            if ((IsWakeUpTypeNull == false))
            {
                return true;
            }
            return false;
        }

        private void ResetWorkgroup()
        {
            _curObj["Workgroup"] = null;
            if (((_isEmbedded == false)
                        && (_autoCommitProp)))
            {
                _privateLateBoundObject.Put();
            }
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

        private static string ConstructPath(string keyName)
        {
            string strPath = "root\\CimV2:Win32_ComputerSystem";
            strPath = string.Concat(strPath, string.Concat(".Name=", string.Concat("\"", string.Concat(keyName, "\""))));
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
        public static ComputerSystemCollection GetInstances()
        {
            return GetInstances(null, null, null);
        }

        public static ComputerSystemCollection GetInstances(string condition)
        {
            return GetInstances(null, condition, null);
        }

        public static ComputerSystemCollection GetInstances(string[] selectedProperties)
        {
            return GetInstances(null, null, selectedProperties);
        }

        public static ComputerSystemCollection GetInstances(string condition, string[] selectedProperties)
        {
            return GetInstances(null, condition, selectedProperties);
        }

        public static ComputerSystemCollection GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions)
        {
            if ((mgmtScope == null))
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = "root\\CimV2" } };
            }
            var pathObj = new ManagementPath { ClassName = "Win32_ComputerSystem", NamespacePath = "root\\CimV2" };
            var clsObject = new ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null))
            {
                enumOptions = new EnumerationOptions { EnsureLocatable = true };
            }
            return new ComputerSystemCollection(clsObject.GetInstances(enumOptions));
        }

        public static ComputerSystemCollection GetInstances(ManagementScope mgmtScope, string condition)
        {
            return GetInstances(mgmtScope, condition, null);
        }

        public static ComputerSystemCollection GetInstances(ManagementScope mgmtScope, string[] selectedProperties)
        {
            return GetInstances(mgmtScope, null, selectedProperties);
        }

        public static ComputerSystemCollection GetInstances(ManagementScope mgmtScope, string condition, string[] selectedProperties)
        {
            if ((mgmtScope == null))
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = "root\\CimV2" } };
            }
            var objectSearcher = new ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_ComputerSystem", condition, selectedProperties));
            var enumOptions = new EnumerationOptions { EnsureLocatable = true };
            objectSearcher.Options = enumOptions;
            return new ComputerSystemCollection(objectSearcher.Get());
        }

        [Browsable(true)]
        public static ComputerSystem CreateInstance()
        {
            ManagementScope mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = CreatedWmiNamespace } };
            var mgmtPath = new ManagementPath(CreatedClassName);
            var tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null);
            return new ComputerSystem(tmpMgmtClass.CreateInstance());
        }

        [Browsable(true)]
        public void Delete()
        {
            _privateLateBoundObject.Delete();
        }

        public uint JoinDomainOrWorkgroup(string accountOu, uint fJoinOptions, string name, string password, string userName)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("JoinDomainOrWorkgroup");
            inParams["AccountOU"] = accountOu;
            inParams["FJoinOptions"] = fJoinOptions;
            inParams["Name"] = name;
            inParams["Password"] = password;
            inParams["UserName"] = userName;
            var outParams = _privateLateBoundObject.InvokeMethod("JoinDomainOrWorkgroup", inParams,
                                                                                  null);
            return outParams == null ? Convert.ToUInt32(-1) : Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
        }

        public uint Rename(string name, string password, string userName)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("Rename");
            inParams["Name"] = name;
            inParams["Password"] = password;
            inParams["UserName"] = userName;
            var outParams = _privateLateBoundObject.InvokeMethod("Rename", inParams, null);
            return outParams == null ? Convert.ToUInt32(-1) : Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
        }

        public uint SetPowerState(ushort powerState, DateTime time)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetPowerState");
            inParams["PowerState"] = powerState;
            inParams["Time"] = ToDmtfDateTime(time);
            var outParams = _privateLateBoundObject.InvokeMethod("SetPowerState", inParams, null);
            return outParams == null ? Convert.ToUInt32(-1) : Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
        }

        public uint UnjoinDomainOrWorkgroup(uint fUnjoinOptions, string password, string userName)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("UnjoinDomainOrWorkgroup");
            inParams["FUnjoinOptions"] = fUnjoinOptions;
            inParams["Password"] = password;
            inParams["UserName"] = userName;
            var outParams = _privateLateBoundObject.InvokeMethod("UnjoinDomainOrWorkgroup",
                                                                                  inParams, null);
            return outParams == null ? Convert.ToUInt32(-1) : Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
        }

        public enum AdminPasswordStatusValues
        {
            Disabled = 0,
            Enabled = 1,
            NotImplemented = 2,
            Unknown0 = 3,
            NullEnumValue = 4,
        }

        public enum BootOptionOnLimitValues
        {

            Reserved = 0,

            OperatingSystem = 1,

            SystemUtilities = 2,

            DoNotReboot = 3,

            NullEnumValue = 4,
        }

        public enum BootOptionOnWatchDogValues
        {

            Reserved = 0,

            OperatingSystem = 1,

            SystemUtilities = 2,

            DoNotReboot = 3,

            NullEnumValue = 4,
        }

        public enum ChassisBootupStateValues
        {

            Other0 = 1,

            Unknown0 = 2,

            Safe = 3,

            Warning = 4,

            Critical = 5,

            NonRecoverable = 6,

            NullEnumValue = 0,
        }

        public enum DomainRoleValues
        {

            StandaloneWorkstation = 0,

            MemberWorkstation = 1,

            StandaloneServer = 2,

            MemberServer = 3,

            BackupDomainController = 4,

            PrimaryDomainController = 5,

            NullEnumValue = 6,
        }

        public enum FrontPanelResetStatusValues
        {

            Disabled = 0,

            Enabled = 1,

            NotImplemented = 2,

            Unknown0 = 3,

            NullEnumValue = 4,
        }

        public enum KeyboardPasswordStatusValues
        {

            Disabled = 0,

            Enabled = 1,

            NotImplemented = 2,

            Unknown0 = 3,

            NullEnumValue = 4,
        }

        public enum PcSystemTypeValues
        {

            Unspecified = 0,

            Desktop = 1,

            Mobile = 2,

            Workstation = 3,

            EnterpriseServer = 4,

            SohoServer = 5,

            AppliancePc = 6,

            PerformanceServer = 7,

            Maximum = 8,

            NullEnumValue = 9,
        }

        public enum PowerManagementCapabilitiesValues
        {

            Unknown0 = 0,

            NotSupported = 1,

            Disabled = 2,

            Enabled = 3,

            PowerSavingModesEnteredAutomatically = 4,

            PowerStateSettable = 5,

            PowerCyclingSupported = 6,

            TimedPowerOnSupported = 7,

            NullEnumValue = 8,
        }

        public enum PowerOnPasswordStatusValues
        {

            Disabled = 0,

            Enabled = 1,

            NotImplemented = 2,

            Unknown0 = 3,

            NullEnumValue = 4,
        }

        public enum PowerStateValues
        {

            Unknown0 = 0,

            FullPower = 1,

            PowerSaveLowPowerMode = 2,

            PowerSaveStandby = 3,

            PowerSaveUnknown = 4,

            PowerCycle = 5,

            PowerOff = 6,

            PowerSaveWarning = 7,

            PowerSaveHibernate = 8,

            PowerSaveSoftOff = 9,

            NullEnumValue = 10,
        }

        public enum PowerSupplyStateValues
        {

            Other0 = 1,

            Unknown0 = 2,

            Safe = 3,

            Warning = 4,

            Critical = 5,

            NonRecoverable = 6,

            NullEnumValue = 0,
        }

        public enum ResetCapabilityValues
        {

            Other0 = 1,

            Unknown0 = 2,

            Disabled = 3,

            Enabled = 4,

            NotImplemented = 5,

            NullEnumValue = 0,
        }

        public enum ThermalStateValues
        {

            Other0 = 1,

            Unknown0 = 2,

            Safe = 3,

            Warning = 4,

            Critical = 5,

            NonRecoverable = 6,

            NullEnumValue = 0,
        }

        public enum WakeUpTypeValues
        {

            Reserved = 0,

            Other0 = 1,

            Unknown0 = 2,

            ApmTimer = 3,

            ModemRing = 4,

            LanRemote = 5,

            PowerSwitch = 6,

            PciPme = 7,

            AcPowerRestored = 8,

            NullEnumValue = 9,
        }

        // Enumerator implementation for enumerating instances of the class.
        public class ComputerSystemCollection : object, ICollection
        {

            private readonly ManagementObjectCollection _privColObj;

            public ComputerSystemCollection(ManagementObjectCollection objCollection)
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
                    array.SetValue(new ComputerSystem(((ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }

            public virtual IEnumerator GetEnumerator()
            {
                return new ComputerSystemEnumerator(_privColObj.GetEnumerator());
            }

            public class ComputerSystemEnumerator : object, IEnumerator
            {

                private readonly ManagementObjectCollection.ManagementObjectEnumerator _privObjEnum;

                public ComputerSystemEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum)
                {
                    _privObjEnum = objEnum;
                }

                public virtual object Current
                {
                    get
                    {
                        return new ComputerSystem(((ManagementObject)(_privObjEnum.Current)));
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
                    if ((value.GetType() == destinationType))
                    {
                        return value;
                    }

                    return _baseConverter.ConvertTo(context, culture, value, destinationType);
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
            public string WmiPath
            {
                get
                {
                    return ((string)(_privateLateBoundObject["__PATH"]));
                }
            }
        }
    }
}
