using System;
using System.ComponentModel;
using System.Management;
using System.Collections;
using System.Globalization;

namespace Org.InCommon.InCert.Engine.NativeCode.Wmi {
    
    
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // Datetime conversion functions ToDateTime and ToDmtfDateTime are added to the class to convert DMTF datetime to DateTime and vice-versa.
    // An Early Bound class generated for the WMI class.Win32_BIOS
    public class Bios : Component {
        
        // Private property to hold the WMI namespace in which the class resides.
        private const string CreatedWmiNamespace = "root\\CimV2";

        // Private property to hold the name of WMI class which created this class.
        private const string CreatedClassName = "Win32_BIOS";

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
        
        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public Bios() {
            InitializeObject(null, null, null);
        }
        
        public Bios(string keyName, string keySoftwareElementId, ushort keySoftwareElementState, ushort keyTargetOperatingSystem, string keyVersion) {
            InitializeObject(null, new ManagementPath(ConstructPath(keyName, keySoftwareElementId, keySoftwareElementState, keyTargetOperatingSystem, keyVersion)), null);
        }
        
        public Bios(ManagementScope mgmtScope, string keyName, string keySoftwareElementId, ushort keySoftwareElementState, ushort keyTargetOperatingSystem, string keyVersion) {
            InitializeObject(mgmtScope, new ManagementPath(ConstructPath(keyName, keySoftwareElementId, keySoftwareElementState, keyTargetOperatingSystem, keyVersion)), null);
        }
        
        public Bios(ManagementPath path, ObjectGetOptions getOptions) {
            InitializeObject(null, path, getOptions);
        }
        
        public Bios(ManagementScope mgmtScope, ManagementPath path) {
            InitializeObject(mgmtScope, path, null);
        }
        
        public Bios(ManagementPath path) {
            InitializeObject(null, path, null);
        }
        
        public Bios(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions) {
            InitializeObject(mgmtScope, path, getOptions);
        }
        
        public Bios(ManagementObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject))) {
                _privateLateBoundObject = theObject;
                _privateSystemProperties = new ManagementSystemProperties(_privateLateBoundObject);
                _curObj = _privateLateBoundObject;
            }
            else {
                throw new ArgumentException("Class name does not match.");
            }
        }
        
        public Bios(ManagementBaseObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject))) {
                _embeddedObj = theObject;
                _privateSystemProperties = new ManagementSystemProperties(theObject);
                _curObj = _embeddedObj;
                _isEmbedded = true;
            }
            else {
                throw new ArgumentException("Class name does not match.");
            }
        }
        
        // Property returns the namespace of the WMI class.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace {
            get {
                return "root\\CimV2";
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName {
            get {
                string strRet = CreatedClassName;
                if ((_curObj != null)) {
                    strRet = ((string)(_curObj["__CLASS"]));
                    if (string.IsNullOrEmpty(strRet)) {
                             strRet = CreatedClassName;
                         }
                }
                return strRet;
            }
        }
        
        // Property pointing to an embedded object to get System properties of the WMI object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementSystemProperties SystemProperties {
            get {
                return _privateSystemProperties;
            }
        }
        
        // Property returning the underlying lateBound object.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementBaseObject LateBoundObject {
            get {
                return _curObj;
            }
        }
        
        // ManagementScope of the object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementScope Scope {
            get
            {
                if ((_isEmbedded == false)) {
                    return _privateLateBoundObject.Scope;
                }

                return null;
            }
            set {
                if ((_isEmbedded == false)) {
                    _privateLateBoundObject.Scope = value;
                }
            }
        }
        
        // Property to show the commit behavior for the WMI object. If true, WMI object will be automatically saved after each property modification.(ie. Put() is called after modification of a property).
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit {
            get {
                return _autoCommitProp;
            }
            set {
                _autoCommitProp = value;
            }
        }
        
        // The ManagementPath of the underlying WMI object.
        [Browsable(true)]
        public ManagementPath Path {
            get
            {
                if ((_isEmbedded == false)) {
                    return _privateLateBoundObject.Path;
                }
                return null;
            }
            set {
                if ((_isEmbedded == false)) {
                    if ((CheckIfProperClass(null, value, null) != true)) {
                        throw new ArgumentException("Class name does not match.");
                    }
                    _privateLateBoundObject.Path = value;
                }
            }
        }
        
        // Public static scope property which is used by the various methods.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static ManagementScope StaticScope {
            get {
                return _statMgmtScope;
            }
            set {
                _statMgmtScope = value;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The BiosCharacteristics property identifies the BIOS characteristics supported by" +
            " the system as defined by the System Management BIOS Reference Specification")]
        public BiosCharacteristicsValues[] BiosCharacteristics {
            get {
                var arrEnumVals = ((Array)(_curObj["BiosCharacteristics"]));
                var enumToRet = new BiosCharacteristicsValues[arrEnumVals.Length];
                int counter;
                for (counter = 0; (counter < arrEnumVals.Length); counter = (counter + 1)) {
                    enumToRet[counter] = ((BiosCharacteristicsValues)(Convert.ToInt32(arrEnumVals.GetValue(counter))));
                }
                return enumToRet;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The BIOSVersion array property contains the complete System BIOS information. In " +
            "many machines, there can be several version strings stored in the Registry repre" +
            "senting the system BIOS info.  The property contains the complete set. ")]
        public string[] BiosVersion {
            get {
                return ((string[])(_curObj["BIOSVersion"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The internal identifier for this compilation of this software element.")]
        public string BuildNumber {
            get {
                return ((string)(_curObj["BuildNumber"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Caption property is a short textual description (one-line string) of the obje" +
            "ct.")]
        public string Caption {
            get {
                return ((string)(_curObj["Caption"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The code set used by this software element. ")]
        public string CodeSet {
            get {
                return ((string)(_curObj["CodeSet"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The CurrentLanguage property shows the name of the current BIOS language.")]
        public string CurrentLanguage {
            get {
                return ((string)(_curObj["CurrentLanguage"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Description property provides a textual description of the object. ")]
        public string Description {
            get {
                return ((string)(_curObj["Description"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(" The value of this property is the manufacturer\'s identifier for this software el" +
            "ement. Often this will be a stock keeping unit (SKU) or a part number.")]
        public string IdentificationCode {
            get {
                return ((string)(_curObj["IdentificationCode"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInstallableLanguagesNull {
            get
            {
                if ((_curObj["InstallableLanguages"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The InstallableLanguages property indicates the number of languages available for" +
            " installation on this  Language may determine properties such as the need" +
            " for Unicode and bi-directional text.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ushort InstallableLanguages {
            get {
                if ((_curObj["InstallableLanguages"] == null)) {
                    return Convert.ToUInt16(0);
                }
                return ((ushort)(_curObj["InstallableLanguages"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInstallDateNull {
            get
            {
                if ((_curObj["InstallDate"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The InstallDate property is datetime value indicating when the object was install" +
            "ed. A lack of a value does not indicate that the object is not installed.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public DateTime InstallDate {
            get
            {
                if ((_curObj["InstallDate"] != null)) {
                    return ToDateTime(((string)(_curObj["InstallDate"])));
                }

                return DateTime.MinValue;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The value of this property identifies the language edition of this software element. The language codes defined in ISO 639 should be used. Where the software element represents multi-lingual or international version of a product, the string multilingual should be used.")]
        public string LanguageEdition {
            get {
                return ((string)(_curObj["LanguageEdition"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The ListOfLanguages property contains a list of namesof available BIOS-installabl" +
            "e languages.")]
        public string[] ListOfLanguages {
            get {
                return ((string[])(_curObj["ListOfLanguages"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Manufacturer of this software element")]
        public string Manufacturer {
            get {
                return ((string)(_curObj["Manufacturer"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The name used to identify this software element")]
        public string Name {
            get {
                return ((string)(_curObj["Name"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@" The OtherTargetOS property records the manufacturer and  operating system type for a software element when  the TargetOperatingSystem property has a value of  1 (""Other"").  Therefore, when the TargetOperatingSystem property has a value of ""Other"", the OtherTargetOS  property must have a non-null value.  For all other values  of TargetOperatingSystem, the OtherTargetOS property is to be NULL. ")]
        public string OtherTargetOs {
            get {
                return ((string)(_curObj["OtherTargetOS"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPrimaryBiosNull {
            get
            {
                if ((_curObj["PrimaryBIOS"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("If true, this is the primary BIOS of the computer ")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool PrimaryBios {
            get {
                if ((_curObj["PrimaryBIOS"] == null)) {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["PrimaryBIOS"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsReleaseDateNull {
            get
            {
                if ((_curObj["ReleaseDate"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The ReleaseDate property indicates the release date of the Win32 BIOS in the Coor" +
            "dinated Universal Time (UTC) format of YYYYMMDDHHMMSS.MMMMMM(+-)OOO.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public DateTime ReleaseDate {
            get
            {
                if ((_curObj["ReleaseDate"] != null)) {
                    return ToDateTime(((string)(_curObj["ReleaseDate"])));
                }
                return DateTime.MinValue;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The assigned serial number of this software element.")]
        public string SerialNumber {
            get {
                return ((string)(_curObj["SerialNumber"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SmBiosBIOSVersion property contains the BIOS version as reported by SM")]
        public string SmBiosBiosVersion {
            get {
                return ((string)(_curObj["SMBIOSBIOSVersion"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSmBiosMajorVersionNull {
            get
            {
                if ((_curObj["SMBIOSMajorVersion"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SmBiosMajorVersion property contains the major SmBios version number. This pr" +
            "operty will be NULL if SmBios not found.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ushort SmBiosMajorVersion {
            get {
                if ((_curObj["SMBIOSMajorVersion"] == null)) {
                    return Convert.ToUInt16(0);
                }
                return ((ushort)(_curObj["SMBIOSMajorVersion"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSmBiosMinorVersionNull {
            get
            {
                if ((_curObj["SMBIOSMinorVersion"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SmBiosMinorVersion property contains the minor SmBios Version number. This pr" +
            "operty will be NULL if SmBios not found.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ushort SmBiosMinorVersion {
            get {
                if ((_curObj["SMBIOSMinorVersion"] == null)) {
                    return Convert.ToUInt16(0);
                }
                return ((ushort)(_curObj["SMBIOSMinorVersion"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSmBiosPresentNull {
            get
            {
                if ((_curObj["SMBIOSPresent"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SmBiosPresent property indicates whether the SmBios is available on this comp" +
            "uter \nValues: TRUE or FALSE. If TRUE, SmBios is on this computer.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool SmBiosPresent {
            get {
                if ((_curObj["SMBIOSPresent"] == null)) {
                    return Convert.ToBoolean(0);
                }
                return ((bool)(_curObj["SMBIOSPresent"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(" This is an identifier for this software element and is designed to be  used in c" +
            "onjunction with other keys to create a unique representation  of this CIM_Softwa" +
            "reElement")]
        public string SoftwareElementId {
            get {
                return ((string)(_curObj["SoftwareElementID"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSoftwareElementStateNull {
            get
            {
                if ((_curObj["SoftwareElementState"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@" The SoftwareElementState is defined in this model to  identify various states of a software elements life cycle.   - A software element in the deployable state describes     the details necessary to successful distribute it and     the details (conditions and actions) required to create     a software element in the installable state (i.e., the next state).  - A software element in the installable state describes     the details necessary to successfully install it and the    details (conditions and actions required to create a     software element in the executable state (i.e., the next state).  - A software element in the executable state describes the     details necessary to successfully  start it and the details     (conditions and actions required to create a software element in     the running state (i.e., the next state).  - A software element in the running state describes the details     necessary to monitor and operate on a start element.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ushort SoftwareElementState {
            get {
                if ((_curObj["SoftwareElementState"] == null)) {
                    return Convert.ToUInt16(0);
                }
                return ((ushort)(_curObj["SoftwareElementState"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The Status property is a string indicating the current status of the object. Various operational and non-operational statuses can be defined. Operational statuses are ""OK"", ""Degraded"" and ""Pred Fail"". ""Pred Fail"" indicates that an element may be functioning properly but predicting a failure in the near future. An example is a SMART-enabled hard drive. Non-operational statuses can also be specified. These are ""Error"", ""Starting"", ""Stopping"" and ""Service"". The latter, ""Service"", could apply during mirror-resilvering of a disk, reload of a user permissions list, or other administrative work. Not all such work is on-line, yet the managed element is neither ""OK"" nor in one of the other states.")]
        public string Status {
            get {
                return ((string)(_curObj["Status"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTargetOperatingSystemNull {
            get
            {
                if ((_curObj["TargetOperatingSystem"] == null)) {
                    return true;
                }
                return false;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The TargetOperatingSystem property allows the provider to specify the  operating system environment. The value of this property does not  ensure binary executable.  Two other pieces of information are needed.   First, the version of the OS needs to be specified using the OS  version check. The second piece of information is the architecture the  OS runs on. The combination of these constructs allows the provider to  clearly identify the level of OS required for a particular software  element.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ushort TargetOperatingSystem {
            get {
                if ((_curObj["TargetOperatingSystem"] == null)) {
                    return Convert.ToUInt16(0);
                }
                return ((ushort)(_curObj["TargetOperatingSystem"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Version property contains the version of the  This string is created by " +
            "the BIOS manufacturer. ")]
        public string Version {
            get {
                return ((string)(_curObj["Version"]));
            }
        }
        
        private bool CheckIfProperClass(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions optionsParam)
        {
            if (((path != null) 
                        && (string.Compare(path.ClassName, ManagementClassName, true, CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            return CheckIfProperClass(new ManagementObject(mgmtScope, path, optionsParam));
        }

        private bool CheckIfProperClass(ManagementBaseObject theObj) {
            if (theObj == null)
                return false;
            
            if (((string.Compare(((string)(theObj["__CLASS"])), ManagementClassName, true, CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            
            var parentClasses = ((Array)(theObj["__DERIVATION"]));
            if ((parentClasses != null)) {
                int count;
                for (count = 0; (count < parentClasses.Length); count = (count + 1)) {
                    if ((string.Compare(((string)(parentClasses.GetValue(count))), ManagementClassName, true, CultureInfo.InvariantCulture) == 0)) {
                        return true;
                    }
                }
            }
            return false;
        }
        
        private bool ShouldSerializeInstallableLanguages() {
            if ((IsInstallableLanguagesNull == false)) {
                return true;
            }
            return false;
        }
        
        // Converts a given datetime in DMTF format to DateTime object.
        static DateTime ToDateTime(string dmtfDate) {
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
            if ((dmtf == null)) {
                throw new ArgumentOutOfRangeException();
            }
            if ((dmtf.Length == 0)) {
                throw new ArgumentOutOfRangeException();
            }
            if ((dmtf.Length != 25)) {
                throw new ArgumentOutOfRangeException();
            }
            try {
                tempString = dmtf.Substring(0, 4);
                if (("****" != tempString)) {
                    year = int.Parse(tempString);
                }
                tempString = dmtf.Substring(4, 2);
                if (("**" != tempString)) {
                    month = int.Parse(tempString);
                }
                tempString = dmtf.Substring(6, 2);
                if (("**" != tempString)) {
                    day = int.Parse(tempString);
                }
                tempString = dmtf.Substring(8, 2);
                if (("**" != tempString)) {
                    hour = int.Parse(tempString);
                }
                tempString = dmtf.Substring(10, 2);
                if (("**" != tempString)) {
                    minute = int.Parse(tempString);
                }
                tempString = dmtf.Substring(12, 2);
                if (("**" != tempString)) {
                    second = int.Parse(tempString);
                }
                tempString = dmtf.Substring(15, 6);
                if (("******" != tempString)) {
                    ticks = (long.Parse(tempString) * (TimeSpan.TicksPerMillisecond / 1000));
                }
                if (((((((((year < 0) 
                            || (month < 0)) 
                            || (day < 0)) 
                            || (hour < 0)) 
                            || (minute < 0)) 
                            || (minute < 0)) 
                            || (second < 0)) 
                            || (ticks < 0))) {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e) {
                throw new ArgumentOutOfRangeException(null, e.Message);
            }
            var datetime = new DateTime(year, month, day, hour, minute, second, 0);
            datetime = datetime.AddTicks(ticks);
            var tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
            var offsetMins = tickOffset.Ticks / TimeSpan.TicksPerMinute;
            tempString = dmtf.Substring(22, 3);
            if ((tempString != "******")) {
                tempString = dmtf.Substring(21, 4);
                int utcOffset;
                try {
                    utcOffset = int.Parse(tempString);
                }
                catch (Exception e) {
                    throw new ArgumentOutOfRangeException(null, e.Message);
                }
                var offsetToBeAdjusted = ((int)((offsetMins - utcOffset)));
                datetime = datetime.AddMinutes(offsetToBeAdjusted);
            }
            return datetime;
        }
        
        // Converts a given DateTime object to DMTF datetime format.

        private bool ShouldSerializeInstallDate() {
            if ((IsInstallDateNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializePrimaryBios() {
            if ((IsPrimaryBiosNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeReleaseDate() {
            if ((IsReleaseDateNull == false)) {
                return true;
            }
            return false;
        }

        private bool ShouldSerializeSoftwareElementState() {
            if ((IsSoftwareElementStateNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeTargetOperatingSystem() {
            if ((IsTargetOperatingSystemNull == false)) {
                return true;
            }
            return false;
        }
        
        [Browsable(true)]
        public void CommitObject() {
            if ((_isEmbedded == false)) {
                _privateLateBoundObject.Put();
            }
        }
        
        [Browsable(true)]
        public void CommitObject(PutOptions putOptions) {
            if ((_isEmbedded == false)) {
                _privateLateBoundObject.Put(putOptions);
            }
        }
        
        private void Initialize() {
            _autoCommitProp = true;
            _isEmbedded = false;
        }
        
        private static string ConstructPath(string keyName, string keySoftwareElementId, ushort keySoftwareElementState, ushort keyTargetOperatingSystem, string keyVersion) {
            string strPath = "root\\CimV2:Win32_BIOS";
            strPath = string.Concat(strPath, string.Concat(".Name=", string.Concat("\"", string.Concat(keyName, "\""))));
            strPath = string.Concat(strPath, string.Concat(",SoftwareElementID=", string.Concat("\"", string.Concat(keySoftwareElementId, "\""))));
            strPath = string.Concat(strPath, string.Concat(",SoftwareElementState=", keySoftwareElementState.ToString(CultureInfo.InvariantCulture)));
            strPath = string.Concat(strPath, string.Concat(",TargetOperatingSystem=", keyTargetOperatingSystem.ToString(CultureInfo.InvariantCulture)));
            strPath = string.Concat(strPath, string.Concat(",Version=", string.Concat("\"", string.Concat(keyVersion, "\""))));
            return strPath;
        }
        
        private void InitializeObject(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions) {
            Initialize();
            if ((path != null)) {
                if ((CheckIfProperClass(mgmtScope, path, getOptions) != true)) {
                    throw new ArgumentException("Class name does not match.");
                }
            }
            _privateLateBoundObject = new ManagementObject(mgmtScope, path, getOptions);
            _privateSystemProperties = new ManagementSystemProperties(_privateLateBoundObject);
            _curObj = _privateLateBoundObject;
        }
        
        // Different overloads of GetInstances() help in enumerating instances of the WMI class.
        public static BiosCollection GetInstances() {
            return GetInstances(null, null, null);
        }
        
        public static BiosCollection GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }
        
        public static BiosCollection GetInstances(string[] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }
        
        public static BiosCollection GetInstances(string condition, string[] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }
        
        public static BiosCollection GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions) {
            if ((mgmtScope == null)) {
                mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = "root\\CimV2"}};
            }
            var pathObj = new ManagementPath {ClassName = "Win32_BIOS", NamespacePath = "root\\CimV2"};
            var clsObject = new ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null)) {
                enumOptions = new EnumerationOptions {EnsureLocatable = true};
            }
            return new BiosCollection(clsObject.GetInstances(enumOptions));
        }
        
        public static BiosCollection GetInstances(ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }
        
        public static BiosCollection GetInstances(ManagementScope mgmtScope, string[] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }
        
        public static BiosCollection GetInstances(ManagementScope mgmtScope, string condition, string[] selectedProperties) {
            if ((mgmtScope == null)) {
                mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = "root\\CimV2"}};
            }
            var objectSearcher = new ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_BIOS", condition, selectedProperties));
            var enumOptions = new EnumerationOptions {EnsureLocatable = true};
            objectSearcher.Options = enumOptions;
            return new BiosCollection(objectSearcher.Get());
        }
        
        [Browsable(true)]
        public static Bios CreateInstance() {
            var mgmtScope = _statMgmtScope ?? new ManagementScope {Path = {NamespacePath = CreatedWmiNamespace}};
            var mgmtPath = new ManagementPath(CreatedClassName);
            var tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null);
            return new Bios(tmpMgmtClass.CreateInstance());
        }
        
        [Browsable(true)]
        public void Delete() {
            _privateLateBoundObject.Delete();
        }
        
        public enum BiosCharacteristicsValues {
            
            Reserved = 0,
            Reserved0 = 1,
            Unknown0 = 2,
            BiosCharacteristicsNotSupported = 3,
            IsaIsSupported = 4,
            McaIsSupported = 5,
            EisaIsSupported = 6,
            PciIsSupported = 7,
            PcCardPcmciaIsSupported = 8,
            PlugAndPlayIsSupported = 9,
            ApmIsSupported = 10,
            BiosIsUpgradeableFlash = 11,
            BiosShadowingIsAllowed = 12,
            VlVesaIsSupported = 13,
            EscdSupportIsAvailable = 14,
            BootFromCdIsSupported = 15,
            SelectableBootIsSupported = 16,
            BiosRomIsSocketed = 17,
            BootFromPcCardPcmciaIsSupported = 18,
            EddEnhancedDiskDriveSpecificationIsSupported = 19,
            Int13HJapaneseFloppyForNec980012Mb35_1KBytesSector360RpmIsSupported = 20,
            Int13HJapaneseFloppyForToshiba12Mb35360RpmIsSupported = 21,
            Int13H525360KbFloppyServicesAreSupported = 22,
            Int13H52512MbFloppyServicesAreSupported = 23,
            Int13H35720KbFloppyServicesAreSupported = 24,
            Int13H35288MbFloppyServicesAreSupported = 25,
            Int_5HPrintScreenServiceIsSupported = 26,
            Int_9H8042KeyboardServicesAreSupported = 27,
            Int14HSerialServicesAreSupported = 28,
            Int17HPrinterServicesAreSupported = 29,
            Int10HCgaMonoVideoServicesAreSupported = 30,
            NecPc98 = 31,
            AcpiSupported = 32,
            UsbLegacyIsSupported = 33,
            AgpIsSupported = 34,
            I2OBootIsSupported = 35,
            Ls120BootIsSupported = 36,
            AtapiZipDriveBootIsSupported = 37,
            Val1394BootIsSupported = 38,
            SmartBatterySupported = 39,
            NullEnumValue = 40,
        }
        
        // Enumerator implementation for enumerating instances of the class.
        public class BiosCollection : object, ICollection {
            
            private readonly ManagementObjectCollection _privColObj;
            
            public BiosCollection(ManagementObjectCollection objCollection) {
                _privColObj = objCollection;
            }
            
            public virtual int Count {
                get {
                    return _privColObj.Count;
                }
            }
            
            public virtual bool IsSynchronized {
                get {
                    return _privColObj.IsSynchronized;
                }
            }
            
            public virtual object SyncRoot {
                get {
                    return this;
                }
            }
            
            public virtual void CopyTo(Array array, int index) {
                _privColObj.CopyTo(array, index);
                int nCtr;
                for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1)) {
                    array.SetValue(new Bios(((ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }
            
            public virtual IEnumerator GetEnumerator() {
                return new BiosEnumerator(_privColObj.GetEnumerator());
            }
            
            public class BiosEnumerator : object, IEnumerator {
                
                private readonly ManagementObjectCollection.ManagementObjectEnumerator _privObjEnum;
                
                public BiosEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum) {
                    _privObjEnum = objEnum;
                }
                
                public virtual object Current {
                    get {
                        return new Bios(((ManagementObject)(_privObjEnum.Current)));
                    }
                }
                
                public virtual bool MoveNext() {
                    return _privObjEnum.MoveNext();
                }
                
                public virtual void Reset() {
                    _privObjEnum.Reset();
                }
            }
        }
        
        // TypeConverter to handle null values for ValueType properties
        public class WmiValueTypeConverter : TypeConverter {
            
            private readonly TypeConverter _baseConverter;
            
            private readonly Type _baseType;
            
            public WmiValueTypeConverter(Type inBaseType) {
                _baseConverter = TypeDescriptor.GetConverter(inBaseType);
                _baseType = inBaseType;
            }
            
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType) {
                return _baseConverter.CanConvertFrom(context, srcType);
            }
            
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
                return _baseConverter.CanConvertTo(context, destinationType);
            }
            
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
                return _baseConverter.ConvertFrom(context, culture, value);
            }
            
            public override object CreateInstance(ITypeDescriptorContext context, IDictionary dictionary) {
                return _baseConverter.CreateInstance(context, dictionary);
            }
            
            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
                return _baseConverter.GetCreateInstanceSupported(context);
            }
            
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributeVar) {
                return _baseConverter.GetProperties(context, value, attributeVar);
            }
            
            public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
                return _baseConverter.GetPropertiesSupported(context);
            }
            
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
                return _baseConverter.GetStandardValues(context);
            }
            
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
                return _baseConverter.GetStandardValuesExclusive(context);
            }
            
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
                return _baseConverter.GetStandardValuesSupported(context);
            }
            
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
                if ((_baseType.BaseType == typeof(Enum))) {
                    if ((value.GetType() == destinationType)) {
                        return value;
                    }

                    return _baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((_baseType == typeof(bool)) 
                            && (_baseType.BaseType == typeof(ValueType)))) {
                    if ((((value == null) 
                                && (context != null)) 
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return "";
                    }
                    return _baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((context != null) 
                            && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                    return "";
                }
                return _baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }
        
        // Embedded class to represent WMI system Properties.
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ManagementSystemProperties {
            
            private readonly ManagementBaseObject _lateBoundObject;
            
            public ManagementSystemProperties(ManagementBaseObject managedObject) {
                _lateBoundObject = managedObject;
            }
            
            [Browsable(true)]
            public int Genus {
                get {
                    return ((int)(_lateBoundObject["__GENUS"]));
                }
            }
            
            [Browsable(true)]
            public string Class {
                get {
                    return ((string)(_lateBoundObject["__CLASS"]));
                }
            }
            
            [Browsable(true)]
            public string Superclass {
                get {
                    return ((string)(_lateBoundObject["__SUPERCLASS"]));
                }
            }
            
            [Browsable(true)]
            public string Dynasty {
                get {
                    return ((string)(_lateBoundObject["__DYNASTY"]));
                }
            }
            
            [Browsable(true)]
            public string Relpath {
                get {
                    return ((string)(_lateBoundObject["__RELPATH"]));
                }
            }
            
            [Browsable(true)]
            public int PropertyCount {
                get {
                    return ((int)(_lateBoundObject["__PROPERTY_COUNT"]));
                }
            }
            
            [Browsable(true)]
            public string[] Derivation {
                get {
                    return ((string[])(_lateBoundObject["__DERIVATION"]));
                }
            }
            
            [Browsable(true)]
            public string Server {
                get {
                    return ((string)(_lateBoundObject["__SERVER"]));
                }
            }
            
            [Browsable(true)]
            public string Namespace {
                get {
                    return ((string)(_lateBoundObject["__NAMESPACE"]));
                }
            }
            
            [Browsable(true)]
            public string Path {
                get {
                    return ((string)(_lateBoundObject["__PATH"]));
                }
            }
        }
    }
}
