using System.ComponentModel;
using System.Management;
using System.Collections;

namespace Org.InCommon.InCert.Engine.NativeCode.Wmi {
   
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // An Early Bound class generated for the WMI class.Win32_EncryptableVolume

    public class EncryptableVolume : System.ComponentModel.Component {
        
        // Private property to hold the WMI namespace in which the class resides.
        private static string CreatedWmiNamespace = "root\\cimv2\\security\\MicrosoftVolumeEncryption";
        
        // Private property to hold the name of WMI class which created this class.
        private static string CreatedClassName = "Win32_EncryptableVolume";
        
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
        public EncryptableVolume() {
            this.InitializeObject(null, null, null);
        }
        
        public EncryptableVolume(string keyDeviceID) {
            this.InitializeObject(null, new System.Management.ManagementPath(EncryptableVolume.ConstructPath(keyDeviceID)), null);
        }
        
        public EncryptableVolume(System.Management.ManagementScope mgmtScope, string keyDeviceID) {
            this.InitializeObject(((System.Management.ManagementScope)(mgmtScope)), new System.Management.ManagementPath(EncryptableVolume.ConstructPath(keyDeviceID)), null);
        }
        
        public EncryptableVolume(System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(null, path, getOptions);
        }
        
        public EncryptableVolume(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path) {
            this.InitializeObject(mgmtScope, path, null);
        }
        
        public EncryptableVolume(System.Management.ManagementPath path) {
            this.InitializeObject(null, path, null);
        }
        
        public EncryptableVolume(System.Management.ManagementScope mgmtScope, System.Management.ManagementPath path, System.Management.ObjectGetOptions getOptions) {
            this.InitializeObject(mgmtScope, path, getOptions);
        }
        
        public EncryptableVolume(System.Management.ManagementObject theObject) {
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
        
        public EncryptableVolume(System.Management.ManagementBaseObject theObject) {
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
                return "root\\cimv2\\security\\MicrosoftVolumeEncryption";
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
        public bool IsConversionStatusNull {
            get {
                if ((curObj["ConversionStatus"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ConversionStatus {
            get {
                if ((curObj["ConversionStatus"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["ConversionStatus"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DeviceID {
            get {
                return ((string)(curObj["DeviceID"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DriveLetter {
            get {
                return ((string)(curObj["DriveLetter"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEncryptionMethodNull {
            get {
                if ((curObj["EncryptionMethod"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint EncryptionMethod {
            get {
                if ((curObj["EncryptionMethod"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["EncryptionMethod"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIsVolumeInitializedForProtectionNull {
            get {
                if ((curObj["IsVolumeInitializedForProtection"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public bool IsVolumeInitializedForProtection {
            get {
                if ((curObj["IsVolumeInitializedForProtection"] == null)) {
                    return System.Convert.ToBoolean(0);
                }
                return ((bool)(curObj["IsVolumeInitializedForProtection"]));
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PersistentVolumeID {
            get {
                return ((string)(curObj["PersistentVolumeID"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsProtectionStatusNull {
            get {
                if ((curObj["ProtectionStatus"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public uint ProtectionStatus {
            get {
                if ((curObj["ProtectionStatus"] == null)) {
                    return System.Convert.ToUInt32(0);
                }
                return ((uint)(curObj["ProtectionStatus"]));
            }
        }
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsVolumeTypeNull {
            get {
                if ((curObj["VolumeType"] == null)) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(WMIValueTypeConverter))]
        public VolumeTypeValues VolumeType {
            get {
                if ((curObj["VolumeType"] == null)) {
                    return ((VolumeTypeValues)(System.Convert.ToInt32(3)));
                }
                return ((VolumeTypeValues)(System.Convert.ToInt32(curObj["VolumeType"])));
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
        
        private bool ShouldSerializeConversionStatus() {
            if ((this.IsConversionStatusNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeEncryptionMethod() {
            if ((this.IsEncryptionMethodNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeIsVolumeInitializedForProtection() {
            if ((this.IsIsVolumeInitializedForProtectionNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeProtectionStatus() {
            if ((this.IsProtectionStatusNull == false)) {
                return true;
            }
            return false;
        }
        
        private bool ShouldSerializeVolumeType() {
            if ((this.IsVolumeTypeNull == false)) {
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
        
        private static string ConstructPath(string keyDeviceID) {
            string strPath = "root\\cimv2\\security\\MicrosoftVolumeEncryption:Win32_EncryptableVolume";
            strPath = string.Concat(strPath, string.Concat(".DeviceID=", string.Concat("\"", string.Concat(keyDeviceID, "\""))));
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
        public static EncryptableVolumeCollection GetInstances() {
            return GetInstances(null, null, null);
        }
        
        public static EncryptableVolumeCollection GetInstances(string condition) {
            return GetInstances(null, condition, null);
        }
        
        public static EncryptableVolumeCollection GetInstances(string[] selectedProperties) {
            return GetInstances(null, null, selectedProperties);
        }
        
        public static EncryptableVolumeCollection GetInstances(string condition, string[] selectedProperties) {
            return GetInstances(null, condition, selectedProperties);
        }
        
        public static EncryptableVolumeCollection GetInstances(System.Management.ManagementScope mgmtScope, System.Management.EnumerationOptions enumOptions) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\cimv2\\security\\MicrosoftVolumeEncryption";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementPath pathObj = new System.Management.ManagementPath();
            pathObj.ClassName = "Win32_EncryptableVolume";
            pathObj.NamespacePath = "root\\cimv2\\security\\MicrosoftVolumeEncryption";
            System.Management.ManagementClass clsObject = new System.Management.ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null)) {
                enumOptions = new System.Management.EnumerationOptions();
                enumOptions.EnsureLocatable = true;
            }
            return new EncryptableVolumeCollection(clsObject.GetInstances(enumOptions));
        }
        
        public static EncryptableVolumeCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition) {
            return GetInstances(mgmtScope, condition, null);
        }
        
        public static EncryptableVolumeCollection GetInstances(System.Management.ManagementScope mgmtScope, string[] selectedProperties) {
            return GetInstances(mgmtScope, null, selectedProperties);
        }
        
        public static EncryptableVolumeCollection GetInstances(System.Management.ManagementScope mgmtScope, string condition, string[] selectedProperties) {
            if ((mgmtScope == null)) {
                if ((statMgmtScope == null)) {
                    mgmtScope = new System.Management.ManagementScope();
                    mgmtScope.Path.NamespacePath = "root\\cimv2\\security\\MicrosoftVolumeEncryption";
                }
                else {
                    mgmtScope = statMgmtScope;
                }
            }
            System.Management.ManagementObjectSearcher ObjectSearcher = new System.Management.ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_EncryptableVolume", condition, selectedProperties));
            System.Management.EnumerationOptions enumOptions = new System.Management.EnumerationOptions();
            enumOptions.EnsureLocatable = true;
            ObjectSearcher.Options = enumOptions;
            return new EncryptableVolumeCollection(ObjectSearcher.Get());
        }
        
        [Browsable(true)]
        public static EncryptableVolume CreateInstance() {
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
            return new EncryptableVolume(tmpMgmtClass.CreateInstance());
        }
        
        [Browsable(true)]
        public void Delete() {
            PrivateLateBoundObject.Delete();
        }
        
        public uint BackupRecoveryInformationToActiveDirectory(string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("BackupRecoveryInformationToActiveDirectory");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("BackupRecoveryInformationToActiveDirectory", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ChangeExternalKey(byte[] ExternalKey, string VolumeKeyProtectorID, out string NewVolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ChangeExternalKey");
                inParams["ExternalKey"] = ((byte[])(ExternalKey));
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ChangeExternalKey", inParams, null);
                NewVolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["NewVolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                NewVolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ChangePassPhrase(string NewPassPhrase, string VolumeKeyProtectorID, out string NewProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ChangePassPhrase");
                inParams["NewPassPhrase"] = ((string)(NewPassPhrase));
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ChangePassPhrase", inParams, null);
                NewProtectorID = System.Convert.ToString(outParams.Properties["NewProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                NewProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ChangePIN(string NewPIN, string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ChangePIN");
                inParams["NewPIN"] = ((string)(NewPIN));
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ChangePIN", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ClearAllAutoUnlockKeys() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ClearAllAutoUnlockKeys", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint Decrypt() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Decrypt", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint DeleteKeyProtector(string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("DeleteKeyProtector");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("DeleteKeyProtector", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint DeleteKeyProtectors() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("DeleteKeyProtectors", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint DisableAutoUnlock() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("DisableAutoUnlock", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint DisableKeyProtectors(uint DisableCount) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("DisableKeyProtectors");
                inParams["DisableCount"] = ((uint)(DisableCount));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("DisableKeyProtectors", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint EnableAutoUnlock(string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("EnableAutoUnlock");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("EnableAutoUnlock", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint EnableKeyProtectors() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("EnableKeyProtectors", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint Encrypt(uint EncryptionFlags, uint EncryptionMethod) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("Encrypt");
                inParams["EncryptionFlags"] = ((uint)(EncryptionFlags));
                inParams["EncryptionMethod"] = ((uint)(EncryptionMethod));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Encrypt", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint EncryptAfterHardwareTest(uint EncryptionFlags, uint EncryptionMethod) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("EncryptAfterHardwareTest");
                inParams["EncryptionFlags"] = ((uint)(EncryptionFlags));
                inParams["EncryptionMethod"] = ((uint)(EncryptionMethod));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("EncryptAfterHardwareTest", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint FindValidCertificates(out string[] CertThumbprint) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("FindValidCertificates", inParams, null);
                CertThumbprint = ((string[])(outParams.Properties["CertThumbprint"].Value));
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                CertThumbprint = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetConversionStatus(uint PrecisionFactor, out uint ConversionStatus, out uint EncryptionFlags, out uint EncryptionPercentage, out uint WipingPercentage, out uint WipingStatus) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetConversionStatus");
                inParams["PrecisionFactor"] = ((uint)(PrecisionFactor));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetConversionStatus", inParams, null);
                ConversionStatus = System.Convert.ToUInt32(outParams.Properties["ConversionStatus"].Value);
                EncryptionFlags = System.Convert.ToUInt32(outParams.Properties["EncryptionFlags"].Value);
                EncryptionPercentage = System.Convert.ToUInt32(outParams.Properties["EncryptionPercentage"].Value);
                WipingPercentage = System.Convert.ToUInt32(outParams.Properties["WipingPercentage"].Value);
                WipingStatus = System.Convert.ToUInt32(outParams.Properties["WipingStatus"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                ConversionStatus = System.Convert.ToUInt32(0);
                EncryptionFlags = System.Convert.ToUInt32(0);
                EncryptionPercentage = System.Convert.ToUInt32(0);
                WipingPercentage = System.Convert.ToUInt32(0);
                WipingStatus = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetEncryptionMethod(out uint EncryptionMethod, out string SelfEncryptionDriveEncryptionMethod) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetEncryptionMethod", inParams, null);
                EncryptionMethod = System.Convert.ToUInt32(outParams.Properties["EncryptionMethod"].Value);
                SelfEncryptionDriveEncryptionMethod = System.Convert.ToString(outParams.Properties["SelfEncryptionDriveEncryptionMethod"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                EncryptionMethod = System.Convert.ToUInt32(0);
                SelfEncryptionDriveEncryptionMethod = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetExternalKeyFileName(string VolumeKeyProtectorID, out string FileName) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetExternalKeyFileName");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetExternalKeyFileName", inParams, null);
                FileName = System.Convert.ToString(outParams.Properties["FileName"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                FileName = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetExternalKeyFromFile(string PathWithFileName, out byte[] ExternalKey) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetExternalKeyFromFile");
                inParams["PathWithFileName"] = ((string)(PathWithFileName));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetExternalKeyFromFile", inParams, null);
                ExternalKey = ((byte[])(outParams.Properties["ExternalKey"].Value));
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                ExternalKey = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetHardwareEncryptionStatus(out uint HardwareEncryptionStatus) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetHardwareEncryptionStatus", inParams, null);
                HardwareEncryptionStatus = System.Convert.ToUInt32(outParams.Properties["HardwareEncryptionStatus"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                HardwareEncryptionStatus = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetHardwareTestStatus(out uint TestError, out uint TestStatus) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetHardwareTestStatus", inParams, null);
                TestError = System.Convert.ToUInt32(outParams.Properties["TestError"].Value);
                TestStatus = System.Convert.ToUInt32(outParams.Properties["TestStatus"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                TestError = System.Convert.ToUInt32(0);
                TestStatus = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetIdentificationField(out string IdentificationField) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetIdentificationField", inParams, null);
                IdentificationField = System.Convert.ToString(outParams.Properties["IdentificationField"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                IdentificationField = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyPackage(string VolumeKeyProtectorID, out byte[] KeyPackage) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyPackage");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyPackage", inParams, null);
                KeyPackage = ((byte[])(outParams.Properties["KeyPackage"].Value));
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                KeyPackage = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectorAdSidInformation(string VolumeKeyProtectorID, out uint Flags, out string SidString) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectorAdSidInformation");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectorAdSidInformation", inParams, null);
                Flags = System.Convert.ToUInt32(outParams.Properties["Flags"].Value);
                SidString = System.Convert.ToString(outParams.Properties["SidString"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                Flags = System.Convert.ToUInt32(0);
                SidString = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectorCertificate(string VolumeKeyProtectorID, out string CertThumbprint, out uint CertType, out byte[] PublicKey) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectorCertificate");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectorCertificate", inParams, null);
                CertThumbprint = System.Convert.ToString(outParams.Properties["CertThumbprint"].Value);
                CertType = System.Convert.ToUInt32(outParams.Properties["CertType"].Value);
                PublicKey = ((byte[])(outParams.Properties["PublicKey"].Value));
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                CertThumbprint = null;
                CertType = System.Convert.ToUInt32(0);
                PublicKey = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectorExternalKey(string VolumeKeyProtectorID, out byte[] ExternalKey) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectorExternalKey");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectorExternalKey", inParams, null);
                ExternalKey = ((byte[])(outParams.Properties["ExternalKey"].Value));
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                ExternalKey = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectorFriendlyName(string VolumeKeyProtectorID, out string FriendlyName) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectorFriendlyName");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectorFriendlyName", inParams, null);
                FriendlyName = System.Convert.ToString(outParams.Properties["FriendlyName"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                FriendlyName = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectorNumericalPassword(string VolumeKeyProtectorID, out string NumericalPassword) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectorNumericalPassword");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectorNumericalPassword", inParams, null);
                NumericalPassword = System.Convert.ToString(outParams.Properties["NumericalPassword"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                NumericalPassword = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectorPlatformValidationProfile(string VolumeKeyProtectorID, out byte[] PlatformValidationProfile) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectorPlatformValidationProfile");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectorPlatformValidationProfile", inParams, null);
                PlatformValidationProfile = ((byte[])(outParams.Properties["PlatformValidationProfile"].Value));
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                PlatformValidationProfile = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectors(uint KeyProtectorType, out string[] VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectors");
                inParams["KeyProtectorType"] = ((uint)(KeyProtectorType));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectors", inParams, null);
                VolumeKeyProtectorID = ((string[])(outParams.Properties["VolumeKeyProtectorID"].Value));
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetKeyProtectorType(string VolumeKeyProtectorID, out uint KeyProtectorType) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("GetKeyProtectorType");
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetKeyProtectorType", inParams, null);
                KeyProtectorType = System.Convert.ToUInt32(outParams.Properties["KeyProtectorType"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                KeyProtectorType = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetLockStatus(out uint LockStatus) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetLockStatus", inParams, null);
                LockStatus = System.Convert.ToUInt32(outParams.Properties["LockStatus"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                LockStatus = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetProtectionStatus(out uint ProtectionStatus) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetProtectionStatus", inParams, null);
                ProtectionStatus = System.Convert.ToUInt32(outParams.Properties["ProtectionStatus"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                ProtectionStatus = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetSuspendCount(out uint SuspendCount) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetSuspendCount", inParams, null);
                SuspendCount = System.Convert.ToUInt32(outParams.Properties["SuspendCount"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                SuspendCount = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint GetVersion(out uint Version) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("GetVersion", inParams, null);
                Version = System.Convert.ToUInt32(outParams.Properties["Version"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                Version = System.Convert.ToUInt32(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint IsAutoUnlockEnabled(out bool IsAutoUnlockEnabled, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("IsAutoUnlockEnabled", inParams, null);
                IsAutoUnlockEnabled = System.Convert.ToBoolean(outParams.Properties["IsAutoUnlockEnabled"].Value);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                IsAutoUnlockEnabled = System.Convert.ToBoolean(0);
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint IsAutoUnlockKeyStored(out bool IsAutoUnlockKeyStored) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("IsAutoUnlockKeyStored", inParams, null);
                IsAutoUnlockKeyStored = System.Convert.ToBoolean(outParams.Properties["IsAutoUnlockKeyStored"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                IsAutoUnlockKeyStored = System.Convert.ToBoolean(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint IsKeyProtectorAvailable(uint KeyProtectorType, out bool IsKeyProtectorAvailable) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("IsKeyProtectorAvailable");
                inParams["KeyProtectorType"] = ((uint)(KeyProtectorType));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("IsKeyProtectorAvailable", inParams, null);
                IsKeyProtectorAvailable = System.Convert.ToBoolean(outParams.Properties["IsKeyProtectorAvailable"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                IsKeyProtectorAvailable = System.Convert.ToBoolean(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public static uint IsNumericalPasswordValid(string NumericalPassword, out bool IsNumericalPasswordValid) {
            bool IsMethodStatic = true;
            if ((IsMethodStatic == true)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementPath mgmtPath = new System.Management.ManagementPath(CreatedClassName);
                System.Management.ManagementClass classObj = new System.Management.ManagementClass(statMgmtScope, mgmtPath, null);
                inParams = classObj.GetMethodParameters("IsNumericalPasswordValid");
                inParams["NumericalPassword"] = ((string)(NumericalPassword));
                System.Management.ManagementBaseObject outParams = classObj.InvokeMethod("IsNumericalPasswordValid", inParams, null);
                IsNumericalPasswordValid = System.Convert.ToBoolean(outParams.Properties["IsNumericalPasswordValid"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                IsNumericalPasswordValid = System.Convert.ToBoolean(0);
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint Lock(bool ForceDismount) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("Lock");
                inParams["ForceDismount"] = ((bool)(ForceDismount));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("Lock", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint PauseConversion() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("PauseConversion", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint PrepareVolume(string DiscoveryVolumeType, uint ForceEncryptionType) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("PrepareVolume");
                inParams["DiscoveryVolumeType"] = ((string)(DiscoveryVolumeType));
                inParams["ForceEncryptionType"] = ((uint)(ForceEncryptionType));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("PrepareVolume", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithAdSid(uint Flags, string FriendlyName, string SidString, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithAdSid");
                inParams["Flags"] = ((uint)(Flags));
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["SidString"] = ((string)(SidString));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithAdSid", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithCertificateFile(string FriendlyName, string PathWithFileName, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithCertificateFile");
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["PathWithFileName"] = ((string)(PathWithFileName));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithCertificateFile", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithCertificateThumbprint(string CertThumbprint, string FriendlyName, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithCertificateThumbprint");
                inParams["CertThumbprint"] = ((string)(CertThumbprint));
                inParams["FriendlyName"] = ((string)(FriendlyName));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithCertificateThumbprint", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithExternalKey(byte[] ExternalKey, string FriendlyName, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithExternalKey");
                inParams["ExternalKey"] = ((byte[])(ExternalKey));
                inParams["FriendlyName"] = ((string)(FriendlyName));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithExternalKey", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithNumericalPassword(string FriendlyName, string NumericalPassword, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithNumericalPassword");
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["NumericalPassword"] = ((string)(NumericalPassword));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithNumericalPassword", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithPassPhrase(string FriendlyName, string PassPhrase, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithPassPhrase");
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["PassPhrase"] = ((string)(PassPhrase));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithPassPhrase", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithTPM(string FriendlyName, byte[] PlatformValidationProfile, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithTPM");
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["PlatformValidationProfile"] = ((byte[])(PlatformValidationProfile));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithTPM", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithTPMAndPIN(string FriendlyName, string PIN, byte[] PlatformValidationProfile, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithTPMAndPIN");
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["PIN"] = ((string)(PIN));
                inParams["PlatformValidationProfile"] = ((byte[])(PlatformValidationProfile));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithTPMAndPIN", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithTPMAndPINAndStartupKey(byte[] ExternalKey, string FriendlyName, string PIN, byte[] PlatformValidationProfile, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithTPMAndPINAndStartupKey");
                inParams["ExternalKey"] = ((byte[])(ExternalKey));
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["PIN"] = ((string)(PIN));
                inParams["PlatformValidationProfile"] = ((byte[])(PlatformValidationProfile));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithTPMAndPINAndStartupKey", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ProtectKeyWithTPMAndStartupKey(byte[] ExternalKey, string FriendlyName, byte[] PlatformValidationProfile, out string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("ProtectKeyWithTPMAndStartupKey");
                inParams["ExternalKey"] = ((byte[])(ExternalKey));
                inParams["FriendlyName"] = ((string)(FriendlyName));
                inParams["PlatformValidationProfile"] = ((byte[])(PlatformValidationProfile));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ProtectKeyWithTPMAndStartupKey", inParams, null);
                VolumeKeyProtectorID = System.Convert.ToString(outParams.Properties["VolumeKeyProtectorID"].Value);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                VolumeKeyProtectorID = null;
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint ResumeConversion() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("ResumeConversion", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint SaveExternalKeyToFile(string Path, string VolumeKeyProtectorID) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("SaveExternalKeyToFile");
                inParams["Path"] = ((string)(Path));
                inParams["VolumeKeyProtectorID"] = ((string)(VolumeKeyProtectorID));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("SaveExternalKeyToFile", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint SetIdentificationField(string IdentificationField) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("SetIdentificationField");
                inParams["IdentificationField"] = ((string)(IdentificationField));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("SetIdentificationField", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UnlockWithAdSid(string SidString) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("UnlockWithAdSid");
                inParams["SidString"] = ((string)(SidString));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UnlockWithAdSid", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UnlockWithCertificateFile(string PathWithFileName, string Pin) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("UnlockWithCertificateFile");
                inParams["PathWithFileName"] = ((string)(PathWithFileName));
                inParams["Pin"] = ((string)(Pin));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UnlockWithCertificateFile", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UnlockWithCertificateThumbprint(string CertThumbprint, string Pin) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("UnlockWithCertificateThumbprint");
                inParams["CertThumbprint"] = ((string)(CertThumbprint));
                inParams["Pin"] = ((string)(Pin));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UnlockWithCertificateThumbprint", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UnlockWithExternalKey(byte[] ExternalKey) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("UnlockWithExternalKey");
                inParams["ExternalKey"] = ((byte[])(ExternalKey));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UnlockWithExternalKey", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UnlockWithNumericalPassword(string NumericalPassword) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("UnlockWithNumericalPassword");
                inParams["NumericalPassword"] = ((string)(NumericalPassword));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UnlockWithNumericalPassword", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UnlockWithPassPhrase(string PassPhrase) {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                inParams = PrivateLateBoundObject.GetMethodParameters("UnlockWithPassPhrase");
                inParams["PassPhrase"] = ((string)(PassPhrase));
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UnlockWithPassPhrase", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public uint UpgradeVolume() {
            if ((isEmbedded == false)) {
                System.Management.ManagementBaseObject inParams = null;
                System.Management.ManagementBaseObject outParams = PrivateLateBoundObject.InvokeMethod("UpgradeVolume", inParams, null);
                return System.Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            else {
                return System.Convert.ToUInt32(0);
            }
        }
        
        public enum VolumeTypeValues {
            
            OSVolume = 0,
            
            FixedDataVolume = 1,
            
            PortableDataVolume = 2,
            
            NULL_ENUM_VALUE = 3,
        }
        
        // Enumerator implementation for enumerating instances of the class.
        public class EncryptableVolumeCollection : object, ICollection {
            
            private ManagementObjectCollection privColObj;
            
            public EncryptableVolumeCollection(ManagementObjectCollection objCollection) {
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
                    array.SetValue(new EncryptableVolume(((System.Management.ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }
            
            public virtual System.Collections.IEnumerator GetEnumerator() {
                return new EncryptableVolumeEnumerator(privColObj.GetEnumerator());
            }
            
            public class EncryptableVolumeEnumerator : object, System.Collections.IEnumerator {
                
                private ManagementObjectCollection.ManagementObjectEnumerator privObjEnum;
                
                public EncryptableVolumeEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum) {
                    privObjEnum = objEnum;
                }
                
                public virtual object Current {
                    get {
                        return new EncryptableVolume(((System.Management.ManagementObject)(privObjEnum.Current)));
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
