using System.ComponentModel;
using System.Management;
using System.Collections;

namespace Org.InCommon.InCert.Engine.NativeCode.Wmi {
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // Datetime conversion functions ToDateTime and ToDmtfDateTime are added to the class to convert DMTF datetime to System.DateTime and vice-versa.
    // An Early Bound class generated for the WMI class.Win32_SystemEnclosure
    public class SystemEnclosure : System.ComponentModel.Component {
        
        // Private property to hold the WMI namespace in which the class resides.
        private static string CreatedWmiNamespace = "root\\cimv2";
        
        // Private property to hold the name of WMI class which created this class.
        private static string CreatedClassName = "Win32_SystemEnclosure";
        
        // Private member variable to hold the ManagementScope which is used by the various methods.
        private static System.Management.ManagementScope statMgmtScope = null;
        
        private ManagementSystemProperties PrivateSystemProperties;
        
        // Underlying lateBound WMI object.
        private System.Management.ManagementObject PrivateLateBoundObject;
        
        // Member variable to store the 'automatic commit' behavior for the class.
        private bool AutoCommitProp;
        
        // Private variable to hold the embedded property representing the instance.
        private System.Management.ManagementBaseObject embeddedObj;
        
        // The current WMI object used
        private System.Management.ManagementBaseObject curObj;
        
        // Flag to indicate if the instance is an embedded object.
        private bool isEmbedded;
        
        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public SystemEnclosure() {
            this.InitializeObject(null, null, null);
        }
        
        public SystemEnclosure(string keyTag) {
            this.InitializeObject(null, new System.Management.ManagementPath(SystemEnclosure.ConstructPath(keyTag)), null);
        }
        
        public SystemEnclosure(System.Management.ManagementScope mgmtScope, string keyTag) {
            this.InitializeObject(((System.Management.ManagementScope)(mgmtScope)), new System.Management.ManagementPath(SystemEnclosure.ConstructPath(keyTag)), null);
        }
        
        public SystemEnclosure(System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(null, path, getOptions);
        }
        
        public SystemEnclosure(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path) {
            this.InitializeObject(mgmtScope, path, null);
        }
        
        public SystemEnclosure(System.Management.ManagementPath path) {
            this.InitializeObject(null, path, null);
        }
        
        public SystemEnclosure(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(mgmtScope, path, getOptions);
        }
        
        public SystemEnclosure(System.Management.ManagementObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                PrivateLateBoundObject = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
                curObj = PrivateLateBoundObject;
            }
            else {
                throw new System.ArgumentException("Class name does not match.");
            }
        }
        
        public SystemEnclosure(System.Management.ManagementBaseObject theObject) {
            Initialize();
            if ((CheckIfProperClass(theObject) == true)) {
                embeddedObj = theObject;
                PrivateSystemProperties = new ManagementSystemProperties(theObject);
                curObj = embeddedObj;
                isEmbedded = true;
            }
            else {
                throw new System.ArgumentException("Class name does not match.");
            }
        }
        
        // Property returns the namespace of the WMI class.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace {
            get {
                return "root\\cimv2";
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName {
            get {
                string strRet = CreatedClassName;
                if ((curObj != null)) {
                    if ((curObj.ClassPath != null)) {
                        strRet = ((string)(curObj["__CLASS"]));
                        if (((strRet == null) 
                                    || (strRet == string.Empty))) {
                            strRet = CreatedClassName;
                        }
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
                return PrivateSystemProperties;
            }
        }
        
        // Property returning the underlying lateBound object.
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementBaseObject LateBoundObject {
            get {
                return curObj;
            }
        }
        
        // ManagementScope of the object.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Management.ManagementScope Scope {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Scope;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    PrivateLateBoundObject.Scope = value;
                }
            }
        }
        
        // Property to show the commit behavior for the WMI object. If true, WMI object will be automatically saved after each property modification.(ie. Put() is called after modification of a property).
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCommit {
            get {
                return AutoCommitProp;
            }
            set {
                AutoCommitProp = value;
            }
        }
        
        // The ManagementPath of the underlying WMI object.
        [Browsable(true)]
        public System.Management.ManagementPath Path {
            get {
                if ((isEmbedded == false)) {
                    return PrivateLateBoundObject.Path;
                }
                else {
                    return null;
                }
            }
            set {
                if ((isEmbedded == false)) {
                    if ((CheckIfProperClass(null, value, null) != true)) {
                        throw new System.ArgumentException("Class name does not match.");
                    }
                    PrivateLateBoundObject.Path = value;
                }
            }
        }
        
        // Public static scope property which is used by the various methods.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static System.Management.ManagementScope StaticScope {
            get {
                return statMgmtScope;
            }
            set {
                statMgmtScope = value;
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAudibleAlarmNull {
            get {
                if ((curObj["AudibleAlarm"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Boolean indicating whether the frame is equipped with an audible alarm.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool AudibleAlarm {
            get {
                if ((curObj["AudibleAlarm"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["AudibleAlarm"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("BreachDescription is a free-form string providing more information if the Securit" +
            "yBreach property indicates that a breach or some other security-related event oc" +
            "curred.")]
        public string BreachDescription {
            get {
                return ((string)(curObj["BreachDescription"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"CableManagementStrategy is a free-form string that contains information on how the various cables are connected and bundled for the frame. With many networking, storage-related and power cables, cable management can be a complex and challenging endeavor. This string property contains information to aid in assembly and service of the frame.")]
        public string CableManagementStrategy {
            get {
                return ((string)(curObj["CableManagementStrategy"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Caption property is a short textual description (one-line string) of the obje" +
            "ct.")]
        public string Caption {
            get {
                return ((string)(curObj["Caption"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("An enumerated, integer-valued array indicating the type of chassis.")]
        public ChassisTypesValues[] ChassisTypes {
            get {
                System.Array arrEnumVals = ((System.Array)(curObj["ChassisTypes"]));
                ChassisTypesValues[] enumToRet = new ChassisTypesValues[arrEnumVals.Length];
                int counter = 0;
                for (counter = 0; (counter < arrEnumVals.Length); counter = (counter + 1)) {
                    enumToRet[counter] = ((ChassisTypesValues)(System.Convert.ToInt32(arrEnumVals.GetValue(counter))));
                }
                return enumToRet;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("CreationClassName indicates the name of the class or the subclass used in the cre" +
            "ation of an instance. When used with the other key properties of this class, thi" +
            "s property allows all instances of this class and its subclasses to be uniquely " +
            "identified.")]
        public string CreationClassName {
            get {
                return ((string)(curObj["CreationClassName"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCurrentRequiredOrProducedNull {
            get {
                if ((curObj["CurrentRequiredOrProduced"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Current required by the chassis at 120V. If power is provided by the chassis (as " +
            "in the case of a UPS), this property may indicate the amperage produced, as a ne" +
            "gative number.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public short CurrentRequiredOrProduced {
            get {
                if ((curObj["CurrentRequiredOrProduced"] == null)) {
                    return System.Convert.ToInt16(0);
                }
                return ((short)(curObj["CurrentRequiredOrProduced"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDepthNull {
            get {
                if ((curObj["Depth"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The depth of the physical package in inches.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public float Depth {
            get {
                if ((curObj["Depth"] == null)) {
                    return System.Convert.ToSingle(0);
                }
                return ((float)(curObj["Depth"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Description property provides a textual description of the object. ")]
        public string Description {
            get {
                return ((string)(curObj["Description"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHeatGenerationNull {
            get {
                if ((curObj["HeatGeneration"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Amount of heat generated by the chassis in BTU/hour.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort HeatGeneration {
            get {
                if ((curObj["HeatGeneration"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["HeatGeneration"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHeightNull {
            get {
                if ((curObj["Height"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The height of the physical package in inches.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public float Height {
            get {
                if ((curObj["Height"] == null)) {
                    return System.Convert.ToSingle(0);
                }
                return ((float)(curObj["Height"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHotSwappableNull {
            get {
                if ((curObj["HotSwappable"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"A physical package can be hot swapped if it is possible to replace the element with a physically different but equivalent one while the containing package has power applied to it (i.e., is 'on').  For example, a disk drive package inserted using SCA connectors is removable and can be hot swapped. All packages that can be hot swapped are inherently removable and replaceable.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool HotSwappable {
            get {
                if ((curObj["HotSwappable"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["HotSwappable"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInstallDateNull {
            get {
                if ((curObj["InstallDate"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The InstallDate property is datetime value indicating when the object was install" +
            "ed. A lack of a value does not indicate that the object is not installed.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public System.DateTime InstallDate {
            get {
                if ((curObj["InstallDate"] != null)) {
                    return ToDateTime(((string)(curObj["InstallDate"])));
                }
                else {
                    return System.DateTime.MinValue;
                }
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsLockPresentNull {
            get {
                if ((curObj["LockPresent"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Boolean indicating whether the frame is protected with a lock.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool LockPresent {
            get {
                if ((curObj["LockPresent"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["LockPresent"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The name of the organization responsible for producing the physical element. This" +
            " may be the entity from whom the element is purchased, but this is not necessari" +
            "ly true. The latter information is contained in the Vendor property of CIM_Produ" +
            "ct.")]
        public string Manufacturer {
            get {
                return ((string)(curObj["Manufacturer"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The name by which the physical element is generally known.")]
        public string Model {
            get {
                return ((string)(curObj["Model"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Name property defines the label by which the object is known. When subclassed" +
            ", the Name property can be overridden to be a Key property.")]
        public string Name {
            get {
                return ((string)(curObj["Name"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumberOfPowerCordsNull {
            get {
                if ((curObj["NumberOfPowerCords"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Integer indicating the number of power cords which must be connected to the chass" +
            "is, for all the components to operate.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public ushort NumberOfPowerCords {
            get {
                if ((curObj["NumberOfPowerCords"] == null)) {
                    return System.Convert.ToUInt16(0);
                }
                return ((ushort)(curObj["NumberOfPowerCords"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"OtherIdentifyingInfo captures additional data, beyond asset tag information, that could be used to identify a physical element. One example is bar code data associated with an element that also has an asset tag. Note that if only bar code data is available and is unique/able to be used as an element key, this property would be NULL and the bar code data used as the class key, in the tag property.")]
        public string OtherIdentifyingInfo {
            get {
                return ((string)(curObj["OtherIdentifyingInfo"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The part number assigned by the organization responsible for producing or manufac" +
            "turing the physical element.")]
        public string PartNumber {
            get {
                return ((string)(curObj["PartNumber"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPoweredOnNull {
            get {
                if ((curObj["PoweredOn"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Boolean indicating that the physical element is powered on (TRUE), or is currentl" +
            "y off (FALSE).")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool PoweredOn {
            get {
                if ((curObj["PoweredOn"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["PoweredOn"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsRemovableNull {
            get {
                if ((curObj["Removable"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"A physical package is removable if it is designed to be taken in and out of the physical container in which it is normally found, without impairing the function of the overall packaging. A package can still be removable if power must be 'off' in order to perform the removal. If power can be 'on' and the package removed, then the element is removable and can be hot swapped. For example, an extra battery in a laptop is removable, as is a disk drive package inserted using SCA connectors. However, the latter can be hot swapped.  A laptop's display is not removable, nor is a non-redundant power supply.  Removing these components would impact the function of the overall packaging or is impossible due to the tight integration of the package.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool Removable {
            get {
                if ((curObj["Removable"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["Removable"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsReplaceableNull {
            get {
                if ((curObj["Replaceable"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"A physical package is replaceable  if it is possible to replace (FRU or upgrade) the element with a physically different one.  For example, some computer systems allow the main processor chip to be upgraded to one of a higher clock rating. In this case, the processor is said to be replaceable . Another example is a power supply package mounted on sliding rails. All removable packages are inherently replaceable .")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool Replaceable {
            get {
                if ((curObj["Replaceable"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["Replaceable"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSecurityBreachNull {
            get {
                if ((curObj["SecurityBreach"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("SecurityBreach is an enumerated, integer-valued property indicating whether a phy" +
            "sical breach of the frame was attempted but unsuccessful (value=4) or attempted " +
            "and successful (5). Also, the values, \"Unknown\", \"Other\" or \"No Breach\", can be " +
            "specified.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public SecurityBreachValues SecurityBreach {
            get {
                if ((curObj["SecurityBreach"] == null)) {
                    return ((SecurityBreachValues)(System.Convert.ToInt32(0)));
                }
                return ((SecurityBreachValues)(System.Convert.ToInt32(curObj["SecurityBreach"])));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSecurityStatusNull {
            get {
                if ((curObj["SecurityStatus"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SecurityStatus property indicates the security setting for external input (su" +
            "ch as a keyboard) to this computer.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public SecurityStatusValues SecurityStatus {
            get {
                if ((curObj["SecurityStatus"] == null)) {
                    return ((SecurityStatusValues)(System.Convert.ToInt32(0)));
                }
                return ((SecurityStatusValues)(System.Convert.ToInt32(curObj["SecurityStatus"])));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A manufacturer-allocated number used to identify the PhysicalElement.")]
        public string SerialNumber {
            get {
                return ((string)(curObj["SerialNumber"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("An array of free-form strings providing more detailed explanations for any of the" +
            " entries in the ServicePhilosophy array. Note, each entry of this array is relat" +
            "ed to the entry in ServicePhilosophy that is located at the same index.")]
        public string[] ServiceDescriptions {
            get {
                return ((string[])(curObj["ServiceDescriptions"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"ServicePhilosophy is an enumerated, integer-valued array that indicates whether the frame is serviced from the top (value=2), front (3), back (4) or side (5), whether it has sliding trays (6) or removable sides (7), and/or whether the frame is moveable (8), for example, having rollers.")]
        public ServicePhilosophyValues[] ServicePhilosophy {
            get {
                System.Array arrEnumVals = ((System.Array)(curObj["ServicePhilosophy"]));
                ServicePhilosophyValues[] enumToRet = new ServicePhilosophyValues[arrEnumVals.Length];
                int counter = 0;
                for (counter = 0; (counter < arrEnumVals.Length); counter = (counter + 1)) {
                    enumToRet[counter] = ((ServicePhilosophyValues)(System.Convert.ToInt32(arrEnumVals.GetValue(counter))));
                }
                return enumToRet;
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The stock keeping unit number for this physical element.")]
        public string SKU {
            get {
                return ((string)(curObj["SKU"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The SMBIOSAssetTag property indicates the asset tag number of the system enclosur" +
            "e.")]
        public string SMBIOSAssetTag {
            get {
                return ((string)(curObj["SMBIOSAssetTag"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The Status property is a string indicating the current status of the object. Various operational and non-operational statuses can be defined. Operational statuses are ""OK"", ""Degraded"" and ""Pred Fail"". ""Pred Fail"" indicates that an element may be functioning properly but predicting a failure in the near future. An example is a SMART-enabled hard drive. Non-operational statuses can also be specified. These are ""Error"", ""Starting"", ""Stopping"" and ""Service"". The latter, ""Service"", could apply during mirror-resilvering of a disk, reload of a user permissions list, or other administrative work. Not all such work is on-line, yet the managed element is neither ""OK"" nor in one of the other states.")]
        public string Status {
            get {
                return ((string)(curObj["Status"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Tag property is a string that uniquely identifies the system enclosure.\nExamp" +
            "le: System Enclosure 1")]
        public string Tag {
            get {
                return ((string)(curObj["Tag"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("An array of free-form strings providing more information on the ChassisTypes arra" +
            "y entries. Note, each entry of this array is related to the entry in ChassisType" +
            "s that is located at the same index.")]
        public string[] TypeDescriptions {
            get {
                return ((string[])(curObj["TypeDescriptions"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A string indicating the version of the physical element.")]
        public string Version {
            get {
                return ((string)(curObj["Version"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVisibleAlarmNull {
            get {
                if ((curObj["VisibleAlarm"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Boolean indicating that the equipment includes a visible alarm.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool VisibleAlarm {
            get {
                if ((curObj["VisibleAlarm"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["VisibleAlarm"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWeightNull {
            get {
                if ((curObj["Weight"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The weight of the physical package in pounds.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public float Weight {
            get {
                if ((curObj["Weight"] == null)) {
                    return System.Convert.ToSingle(0);
                }
                return ((float)(curObj["Weight"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWidthNull {
            get {
                if ((curObj["Width"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The width of the physical package in inches.")]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public float Width {
            get {
                if ((curObj["Width"] == null)) {
                    return System.Convert.ToSingle(0);
                }
                return ((float)(curObj["Width"]));
            }
        }
        
        private bool CheckIfProperClass(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions OptionsParam) {
            if (((path != null) 
                        && (string.Compare(path.ClassName, this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                return CheckIfProperClass(new System.Management.ManagementObject(mgmtScope, path, OptionsParam));
            }
        }
        
        private bool CheckIfProperClass(System.Management.ManagementBaseObject theObj) {
            if (((theObj != null) 
                        && (string.Compare(((string)(theObj["__CLASS"])), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0))) {
                return true;
            }
            else {
                System.Array parentClasses = ((System.Array)(theObj["__DERIVATION"]));
                if ((parentClasses != null)) {
                    int count = 0;
                    for (count = 0; (count < parentClasses.Length); count = (count + 1)) {
                        if ((string.Compare(((string)(parentClasses.GetValue(count))), this.ManagementClassName, true, System.Globalization.CultureInfo.InvariantCulture) == 0)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        private bool ShouldSerializeAudibleAlarm() {
            if ((this.IsAudibleAlarmNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeCurrentRequiredOrProduced() {
            if ((this.IsCurrentRequiredOrProducedNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeDepth() {
            if ((this.IsDepthNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeHeatGeneration() {
            if ((this.IsHeatGenerationNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeHeight() {
            if ((this.IsHeightNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeHotSwappable() {
            if ((this.IsHotSwappableNull == false)) {
                return true;
            }
            return false;
        }
        
        // Converts a given datetime in DMTF format to System.DateTime object.
        static System.DateTime ToDateTime(string dmtfDate) {
            System.DateTime initializer = System.DateTime.MinValue;
            int year = initializer.Year;
            int month = initializer.Month;
            int day = initializer.Day;
            int hour = initializer.Hour;
            int minute = initializer.Minute;
            int second = initializer.Second;
            long ticks = 0;
            string dmtf = dmtfDate;
            System.DateTime datetime = System.DateTime.MinValue;
            string tempString = string.Empty;
            if ((dmtf == null)) {
                throw new System.ArgumentOutOfRangeException();
            }
            if ((dmtf.Length == 0)) {
                throw new System.ArgumentOutOfRangeException();
            }
            if ((dmtf.Length != 25)) {
                throw new System.ArgumentOutOfRangeException();
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
                    ticks = (long.Parse(tempString) * ((long)((System.TimeSpan.TicksPerMillisecond / 1000))));
                }
                if (((((((((year < 0) 
                            || (month < 0)) 
                            || (day < 0)) 
                            || (hour < 0)) 
                            || (minute < 0)) 
                            || (minute < 0)) 
                            || (second < 0)) 
                            || (ticks < 0))) {
                    throw new System.ArgumentOutOfRangeException();
                }
            }
            catch (System.Exception e) {
                throw new System.ArgumentOutOfRangeException(null, e.Message);
            }
            datetime = new System.DateTime(year, month, day, hour, minute, second, 0);
            datetime = datetime.AddTicks(ticks);
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
            int UTCOffset = 0;
            int OffsetToBeAdjusted = 0;
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            tempString = dmtf.Substring(22, 3);
            if ((tempString != "******")) {
                tempString = dmtf.Substring(21, 4);
                try {
                    UTCOffset = int.Parse(tempString);
                }
                catch (System.Exception e) {
                    throw new System.ArgumentOutOfRangeException(null, e.Message);
                }
                OffsetToBeAdjusted = ((int)((OffsetMins - UTCOffset)));
                datetime = datetime.AddMinutes(((double)(OffsetToBeAdjusted)));
            }
            return datetime;
        }
        
        // Converts a given System.DateTime object to DMTF datetime format.
        static string ToDmtfDateTime(System.DateTime date) {
            string utcString = string.Empty;
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(date);
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            if ((System.Math.Abs(OffsetMins) > 999)) {
                date = date.ToUniversalTime();
                utcString = "+000";
            }
            else {
                if ((tickOffset.Ticks >= 0)) {
                    utcString = string.Concat("+", ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute))).ToString().PadLeft(3, '0'));
                }
                else {
                    string strTemp = ((long)(OffsetMins)).ToString();
                    utcString = string.Concat("-", strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
                }
            }
            string dmtfDateTime = ((int)(date.Year)).ToString().PadLeft(4, '0');
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Month)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Day)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Hour)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Minute)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((int)(date.Second)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ".");
            System.DateTime dtTemp = new System.DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
            long microsec = ((long)((((date.Ticks - dtTemp.Ticks) 
                        * 1000) 
                        / System.TimeSpan.TicksPerMillisecond)));
            string strMicrosec = ((long)(microsec)).ToString();
            if ((strMicrosec.Length > 6)) {
                strMicrosec = strMicrosec.Substring(0, 6);
            }
            dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, utcString);
            return dmtfDateTime;
        }
        
        private bool ShouldSerializeInstallDate() {
            if ((this.IsInstallDateNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeLockPresent() {
            if ((this.IsLockPresentNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeNumberOfPowerCords() {
            if ((this.IsNumberOfPowerCordsNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializePoweredOn() {
            if ((this.IsPoweredOnNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeRemovable() {
            if ((this.IsRemovableNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeReplaceable() {
            if ((this.IsReplaceableNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeSecurityBreach() {
            if ((this.IsSecurityBreachNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeSecurityStatus() {
            if ((this.IsSecurityStatusNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeVisibleAlarm() {
            if ((this.IsVisibleAlarmNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeWeight() {
            if ((this.IsWeightNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeWidth() {
            if ((this.IsWidthNull == false)) {
                return true;
            }
            return false;
        }
        
        [Browsable(true)]
        public void CommitObject() {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put();
            }
        }
        
        [Browsable(true)]
        public void CommitObject(System.Management.PutOptions putOptions) {
            if ((isEmbedded == false)) {
                PrivateLateBoundObject.Put(putOptions);
            }
        }
        
        private void Initialize() {
            AutoCommitProp = true;
            isEmbedded = false;
        }
        
        private static string ConstructPath(string keyTag) {
            string strPath = "root\\cimv2:Win32_SystemEnclosure";
            strPath = string.Concat(strPath, string.Concat(".Tag=", string.Concat("\"", string.Concat(keyTag, "\""))));
            return strPath;
        }
        
        private void InitializeObject(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            Initialize();
            if ((path != null)) {
                if ((CheckIfProperClass(mgmtScope, path, getOptions) != true)) {
                    throw new System.ArgumentException("Class name does not match.");
                }
            }
            PrivateLateBoundObject = new System.Management.ManagementObject(mgmtScope, path, getOptions);
            PrivateSystemProperties = new ManagementSystemProperties(PrivateLateBoundObject);
            curObj = PrivateLateBoundObject;
        }
        
        // Different overloads of GetInstances() help in enumerating instances of the WMI class.
        public static SystemEnclosureCollection GetInstances() {
            return GetInstances(null, null, null);
        }
        
        public static SystemEnclosureCollection GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }
        
        public static SystemEnclosureCollection GetInstances(string[] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }
        
        public static SystemEnclosureCollection GetInstances(string condition, string[] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }
        
        public static SystemEnclosureCollection GetInstances(System.Management.ManagementScope mgmtScope, System.Management.EnumerationOptions enumOptions) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\cimv2";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementPath pathObj = new System.Management.ManagementPath();
            pathObj.ClassName = "Win32_SystemEnclosure";
            pathObj.NamespacePath = "root\\cimv2";
            System.Management.ManagementClass clsObject = new System.Management.ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null)) {
                enumOptions = new System.Management.EnumerationOptions();
                enumOptions.EnsureLocatable = true;
            }
            return new SystemEnclosureCollection(clsObject.GetInstances(enumOptions));
        }
        
        public static SystemEnclosureCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }
        
        public static SystemEnclosureCollection GetInstances(System.Management.ManagementScope mgmtScope, string[] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }
        
        public static SystemEnclosureCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition, string[] selectedProperties) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\cimv2";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementObjectSearcher ObjectSearcher = new System.Management.ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_SystemEnclosure", condition, selectedProperties));
            System.Management.EnumerationOptions enumOptions = new System.Management.EnumerationOptions();
            enumOptions.EnsureLocatable = true;
            ObjectSearcher.Options = enumOptions;
            return new SystemEnclosureCollection(ObjectSearcher.Get());
        }
        
        [Browsable(true)]
        public static SystemEnclosure CreateInstance() {
            System.Management.ManagementScope mgmtScope = null;
            if ((statMgmtScope == null)) {
                mgmtScope = new System.Management.ManagementScope();
                mgmtScope.Path.NamespacePath = CreatedWmiNamespace;
            }
            else {
                mgmtScope = statMgmtScope;
            }
            System.Management.ManagementPath mgmtPath = new System.Management.ManagementPath(CreatedClassName);
            System.Management.ManagementClass tmpMgmtClass = new System.Management.ManagementClass(mgmtScope, mgmtPath, null);
            return new SystemEnclosure(tmpMgmtClass.CreateInstance());
        }
        
        [Browsable(true)]
        public void Delete() {
            PrivateLateBoundObject.Delete();
        }
        
        public uint IsCompatible(System.Management.ManagementPath ElementToCheck) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("IsCompatible");
                inParams["ElementToCheck"] = ((System.Management.ManagementPath)(ElementToCheck)).Path;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("IsCompatible", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public enum ChassisTypesValues {
            
            Other0 = 1,
            
            Unknown0 = 2,
            
            Desktop = 3,
            
            Low_Profile_Desktop = 4,
            
            Pizza_Box = 5,
            
            Mini_Tower = 6,
            
            Tower = 7,
            
            Portable = 8,
            
            Laptop = 9,
            
            Notebook = 10,
            
            Hand_Held = 11,
            
            Docking_Station = 12,
            
            All_in_One = 13,
            
            Sub_Notebook = 14,
            
            Space_Saving = 15,
            
            Lunch_Box = 16,
            
            Main_System_Chassis = 17,
            
            Expansion_Chassis = 18,
            
            SubChassis = 19,
            
            Bus_Expansion_Chassis = 20,
            
            Peripheral_Chassis = 21,
            
            Storage_Chassis = 22,
            
            Rack_Mount_Chassis = 23,
            
            Sealed_Case_PC = 24,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum SecurityBreachValues {
            
            Other0 = 1,
            
            Unknown0 = 2,
            
            No_Breach = 3,
            
            Breach_Attempted = 4,
            
            Breach_Successful = 5,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum SecurityStatusValues {
            
            Other0 = 1,
            
            Unknown0 = 2,
            
            None = 3,
            
            External_interface_locked_out = 4,
            
            External_interface_enabled = 5,
            
            NULL_ENUM_VALUE = 0,
        }
        
        public enum ServicePhilosophyValues {
            
            Unknown0 = 0,
            
            Other0 = 1,
            
            Service_From_Top = 2,
            
            Service_From_Front = 3,
            
            Service_From_Back = 4,
            
            Service_From_Side = 5,
            
            Sliding_Trays = 6,
            
            Removable_Sides = 7,
            
            Moveable = 8,
            
            NULL_ENUM_VALUE = 9,
        }
        
        // Enumerator implementation for enumerating instances of the class.
        public class SystemEnclosureCollection : object, ICollection {
            
            private ManagementObjectCollection privColObj;
            
            public SystemEnclosureCollection(ManagementObjectCollection objCollection) {
                privColObj = objCollection;
            }
            
            public virtual int Count {
                get {
                    return privColObj.Count;
                }
            }
            
            public virtual bool IsSynchronized {
                get {
                    return privColObj.IsSynchronized;
                }
            }
            
            public virtual object SyncRoot {
                get {
                    return this;
                }
            }
            
            public virtual void CopyTo(System.Array array, int index) {
                privColObj.CopyTo(array, index);
                int nCtr;
                for (nCtr = 0; (nCtr < array.Length); nCtr = (nCtr + 1)) {
                    array.SetValue(new SystemEnclosure(((System.Management.ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }
            
            public virtual System.Collections.IEnumerator GetEnumerator() {
                return new SystemEnclosureEnumerator(privColObj.GetEnumerator());
            }
            
            public class SystemEnclosureEnumerator : object, System.Collections.IEnumerator {
                
                private ManagementObjectCollection.ManagementObjectEnumerator privObjEnum;
                
                public SystemEnclosureEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum) {
                    privObjEnum = objEnum;
                }
                
                public virtual object Current {
                    get {
                        return new SystemEnclosure(((System.Management.ManagementObject)(privObjEnum.Current)));
                    }
                }
                
                public virtual bool MoveNext() {
                    return privObjEnum.MoveNext();
                }
                
                public virtual void Reset() {
                    privObjEnum.Reset();
                }
            }
        }
        
        // TypeConverter to handle null values for ValueType properties
        public class WMIValueTypeConverter : TypeConverter {
            
            private TypeConverter baseConverter;
            
            private System.Type baseType;
            
            public WMIValueTypeConverter(System.Type inBaseType) {
                baseConverter = TypeDescriptor.GetConverter(inBaseType);
                baseType = inBaseType;
            }
            
            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type srcType) {
                return baseConverter.CanConvertFrom(context, srcType);
            }
            
            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType) {
                return baseConverter.CanConvertTo(context, destinationType);
            }
            
            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
                return baseConverter.ConvertFrom(context, culture, value);
            }
            
            public override object CreateInstance(System.ComponentModel.ITypeDescriptorContext context, System.Collections.IDictionary dictionary) {
                return baseConverter.CreateInstance(context, dictionary);
            }
            
            public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetCreateInstanceSupported(context);
            }
            
            public override PropertyDescriptorCollection GetProperties(System.ComponentModel.ITypeDescriptorContext context, object value, System.Attribute[] attributeVar) {
                return baseConverter.GetProperties(context, value, attributeVar);
            }
            
            public override bool GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetPropertiesSupported(context);
            }
            
            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValues(context);
            }
            
            public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesExclusive(context);
            }
            
            public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context) {
                return baseConverter.GetStandardValuesSupported(context);
            }
            
            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType) {
                if ((baseType.BaseType == typeof(System.Enum))) {
                    if ((value.GetType() == destinationType)) {
                        return value;
                    }
                    if ((((value == null) 
                                && (context != null)) 
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return  "NULL_ENUM_VALUE" ;
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((baseType == typeof(bool)) 
                            && (baseType.BaseType == typeof(System.ValueType)))) {
                    if ((((value == null) 
                                && (context != null)) 
                                && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                        return "";
                    }
                    return baseConverter.ConvertTo(context, culture, value, destinationType);
                }
                if (((context != null) 
                            && (context.PropertyDescriptor.ShouldSerializeValue(context.Instance) == false))) {
                    return "";
                }
                return baseConverter.ConvertTo(context, culture, value, destinationType);
            }
        }
        
        // Embedded class to represent WMI system Properties.
        [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
        public class ManagementSystemProperties {
            
            private System.Management.ManagementBaseObject PrivateLateBoundObject;
            
            public ManagementSystemProperties(System.Management.ManagementBaseObject ManagedObject) {
                PrivateLateBoundObject = ManagedObject;
            }
            
            [Browsable(true)]
            public int GENUS {
                get {
                    return ((int)(PrivateLateBoundObject["__GENUS"]));
                }
            }
            
            [Browsable(true)]
            public string CLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__CLASS"]));
                }
            }
            
            [Browsable(true)]
            public string SUPERCLASS {
                get {
                    return ((string)(PrivateLateBoundObject["__SUPERCLASS"]));
                }
            }
            
            [Browsable(true)]
            public string DYNASTY {
                get {
                    return ((string)(PrivateLateBoundObject["__DYNASTY"]));
                }
            }
            
            [Browsable(true)]
            public string RELPATH {
                get {
                    return ((string)(PrivateLateBoundObject["__RELPATH"]));
                }
            }
            
            [Browsable(true)]
            public int PROPERTY_COUNT {
                get {
                    return ((int)(PrivateLateBoundObject["__PROPERTY_COUNT"]));
                }
            }
            
            [Browsable(true)]
            public string[] DERIVATION {
                get {
                    return ((string[])(PrivateLateBoundObject["__DERIVATION"]));
                }
            }
            
            [Browsable(true)]
            public string SERVER {
                get {
                    return ((string)(PrivateLateBoundObject["__SERVER"]));
                }
            }
            
            [Browsable(true)]
            public string NAMESPACE {
                get {
                    return ((string)(PrivateLateBoundObject["__NAMESPACE"]));
                }
            }
            
            [Browsable(true)]
            public string PATH {
                get {
                    return ((string)(PrivateLateBoundObject["__PATH"]));
                }
            }
        }
    }
}
