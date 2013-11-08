using System;
using System.ComponentModel;
using System.Globalization;
using System.Management;
using System.Collections;

namespace Org.InCommon.InCert.Engine.NativeCode.Wmi
{
    // Functions ShouldSerialize<PropertyName> are functions used by VS property browser to check if a particular property has to be serialized. These functions are added for all ValueType properties ( properties of type Int32, BOOL etc.. which cannot be set to null). These functions use Is<PropertyName>Null function. These functions are also used in the TypeConverter implementation for the properties to check for NULL value of property so that an empty value can be shown in Property browser in case of Drag and Drop in Visual studio.
    // Functions Is<PropertyName>Null() are used to check if a property is NULL.
    // Functions Reset<PropertyName> are added for Nullable Read/Write properties. These functions are used by VS designer in property browser to set a property to NULL.
    // Every property added to the class for WMI property has attributes set to define its behavior in Visual Studio designer and also to define a TypeConverter to be used.
    // Datetime conversion functions ToDateTime and ToDmtfDateTime are added to the class to convert DMTF datetime to DateTime and vice-versa.
    // An Early Bound class generated for the WMI class.Win32_NetworkAdapterConfiguration
    public class NetworkAdapterConfiguration : Component
    {

        // Private property to hold the WMI namespace in which the class resides.
        private const string CreatedWmiNamespace = "root\\cimv2";

        // Private property to hold the name of WMI class which created this class.
        private const string CreatedClassName = "Win32_NetworkAdapterConfiguration";

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

        // Flag to indicate if the instance is an embedded object.
        private bool _isEmbedded;

        // Below are different overloads of constructors to initialize an instance of the class with a WMI object.
        public NetworkAdapterConfiguration()
        {
            InitializeObject(null, null, null);
        }

        public NetworkAdapterConfiguration(uint keyIndex)
        {
            InitializeObject(null, new ManagementPath(ConstructPath(keyIndex)), null);
        }

        public NetworkAdapterConfiguration(ManagementScope mgmtScope, uint keyIndex)
        {
            InitializeObject(((mgmtScope)), new ManagementPath(ConstructPath(keyIndex)), null);
        }

        public NetworkAdapterConfiguration(ManagementPath path, ObjectGetOptions getOptions)
        {
            InitializeObject(null, path, getOptions);
        }

        public NetworkAdapterConfiguration(ManagementScope mgmtScope, ManagementPath path)
        {
            InitializeObject(mgmtScope, path, null);
        }

        public NetworkAdapterConfiguration(ManagementPath path)
        {
            InitializeObject(null, path, null);
        }

        public NetworkAdapterConfiguration(ManagementScope mgmtScope, ManagementPath path, ObjectGetOptions getOptions)
        {
            InitializeObject(mgmtScope, path, getOptions);
        }

        public NetworkAdapterConfiguration(ManagementObject theObject)
        {
            Initialize();
            if ((CheckIfProperClass(theObject)))
            {
                _privateLateBoundObject = theObject;
                _privateSystemProperties = new ManagementSystemProperties(_privateLateBoundObject);
                LateBoundObject = _privateLateBoundObject;
            }
            else
            {
                throw new ArgumentException("Class name does not match.");
            }
        }

        public NetworkAdapterConfiguration(ManagementBaseObject theObject)
        {
            Initialize();
            if ((!CheckIfProperClass(theObject)))
            {
                throw new ArgumentException("Class name does not match.");
            }
            _embeddedObj = theObject;
            _privateSystemProperties = new ManagementSystemProperties(theObject);
            LateBoundObject = _embeddedObj;
            _isEmbedded = true;
        }

        // Property returns the namespace of the WMI class.
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OriginatingNamespace
        {
            get
            {
                return "root\\cimv2";
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ManagementClassName
        {
            get
            {
                const string strRet = CreatedClassName;
                if ((LateBoundObject != null))
                {
                    return ((string)(LateBoundObject["__CLASS"]));
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManagementBaseObject LateBoundObject { get; private set; }

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
        public bool IsArpAlwaysSourceRouteNull
        {
            get
            {
                return (LateBoundObject["ArpAlwaysSourceRoute"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The ArpAlwaysSourceRoute property indicates whether the Address Resolution Protocol (ARP) must always use source routing. If this property is TRUE, TCP/IP will transmit ARP queries with source routing enabled on Token Ring networks. By default, ARP first queries without source routing, and retries with source routing enabled if no reply was received. Source routing allows the routing of network packets across different types of networks. Default: FALSE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool ArpAlwaysSourceRoute
        {
            get
            {
                if ((LateBoundObject["ArpAlwaysSourceRoute"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["ArpAlwaysSourceRoute"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsArpUseEtherSnapNull
        {
            get
            {
                return (LateBoundObject["ArpUseEtherSNAP"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The ArpUseEtherSNAP property indicates whether Ethernet packets follow the IEEE 802.3 Sub-Network Access Protocol (SNAP) encoding. Setting this parameter to 1 will force TCP/IP to transmit Ethernet packets using 802.3 SNAP encoding. By default, the stack transmits packets in DIX Ethernet format. Windows NT/Windows 2000 systems are able to receive both formats. Default: FALSE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool ArpUseEtherSnap
        {
            get
            {
                if ((LateBoundObject["ArpUseEtherSNAP"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["ArpUseEtherSNAP"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A short textual description (one-line string) of the CIM_Setting object.")]
        public string Caption
        {
            get
            {
                return ((string)(LateBoundObject["Caption"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DatabasePath property indicates a valid Windows file path to standard Internet database files (HOSTS, LMHOSTS, NETWORKS, PROTOCOLS).  The file path is used by the Windows Sockets interface. This property is only available on Windows NT/Windows 2000 systems.")]
        public string DatabasePath
        {
            get
            {
                return ((string)(LateBoundObject["DatabasePath"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDeadGwDetectEnabledNull
        {
            get
            {
                return (LateBoundObject["DeadGWDetectEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DeadGWDetectEnabled property indicates whether dead gateway detection occurs. Setting this parameter to TRUE causes TCP to perform Dead Gateway Detection. With this feature enabled, TCP will ask IP to change to a backup gateway if it retransmits a segment several times without receiving a response. Default: TRUE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool DeadGwDetectEnabled
        {
            get
            {
                if ((LateBoundObject["DeadGWDetectEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["DeadGWDetectEnabled"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DefaultIPGateway property contains a list of IP addresses of default gateways" +
            " used by the computer system.\nExample: 194.161.12.1 194.162.46.1")]
        public string[] DefaultIpGateway
        {
            get
            {
                return (string[])((LateBoundObject["DefaultIPGateway"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDefaultTosNull
        {
            get
            {
                return (LateBoundObject["DefaultTOS"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DefaultTOS property indicates the default Type Of Service (TOS) value set in " +
            "the header of outgoing IP packets. RFC 791 defines the values. Default: 0, Valid" +
            " Range: 0 - 255.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public byte DefaultTos
        {
            get
            {
                if ((LateBoundObject["DefaultTOS"] == null))
                {
                    return Convert.ToByte(0);
                }
                return ((byte)(LateBoundObject["DefaultTOS"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDefaultTtlNull
        {
            get
            {
                return (LateBoundObject["DefaultTTL"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DefaultTTL property indicates the default Time To Live (TTL) value set in the header of outgoing IP packets. The TTL specifies the number of routers an IP packet may pass through to reach its destination before being discarded. Each router decrements the TTL count of a packet by one as it passes through and discards the packets if the TTL is 0. Default: 32, Valid Range: 1 - 255.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public byte DefaultTtl
        {
            get
            {
                if ((LateBoundObject["DefaultTTL"] == null))
                {
                    return Convert.ToByte(0);
                }
                return ((byte)(LateBoundObject["DefaultTTL"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("A textual description of the CIM_Setting object.")]
        public string Description
        {
            get
            {
                return ((string)(LateBoundObject["Description"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDhcpEnabledNull
        {
            get
            {
                return (LateBoundObject["DHCPEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DHCPEnabled property indicates whether the dynamic host configuration protoco" +
            "l  (DHCP) server automatically assigns an IP address to the computer system when" +
            " establishing a network connection.\nValues: TRUE or FALSE. If TRUE, DHCP is enab" +
            "led.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool DhcpEnabled
        {
            get
            {
                if ((LateBoundObject["DHCPEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["DHCPEnabled"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDhcpLeaseExpiresNull
        {
            get
            {
                return (LateBoundObject["DHCPLeaseExpires"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DHCPLeaseExpires property indicates the expiration date and time for a leased" +
            " IP address that was assigned to the computer by the dynamic host configuration " +
            "protocol (DHCP) server.\nExample: 20521201000230.000000000")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public DateTime DhcpLeaseExpires
        {
            get
            {
                return (LateBoundObject["DHCPLeaseExpires"] != null) ? ToDateTime(((string)(LateBoundObject["DHCPLeaseExpires"]))) : DateTime.MinValue;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDhcpLeaseObtainedNull
        {
            get
            {
                return (LateBoundObject["DHCPLeaseObtained"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DHCPLeaseObtained property indicates the date and time the lease was obtained" +
            " for the IP address assigned to the computer by the dynamic host configuration p" +
            "rotocol (DHCP) server. \nExample: 19521201000230.000000000")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public DateTime DhcpLeaseObtained
        {
            get
            {
                return (LateBoundObject["DHCPLeaseObtained"] != null) ? ToDateTime(((string)(LateBoundObject["DHCPLeaseObtained"]))) : DateTime.MinValue;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DHCPServer property indicates the IP address of the dynamic host configuratio" +
            "n protocol (DHCP) server.\nExample: 154.55.34")]
        public string DhcpServer
        {
            get
            {
                return ((string)(LateBoundObject["DHCPServer"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DNSDomain property indicates an organization name followed by a period and an extension that indicates the type of organization, such as microsoft.com. The name can be any combination of the letters A through Z, the numerals 0 through 9, and the hyphen (-), plus the period (.) character used as a separator.
Example: microsoft.com")]
        public string DnsDomain
        {
            get
            {
                return ((string)(LateBoundObject["DNSDomain"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DNSDomainSuffixSearchOrder property specifies the DNS domain suffixes to be appended to the end of host names during name resolution. When attempting to resolve a fully qualified domain name (FQDN) from a host only name, the system will first append the local domain name. If this is not successful, the system will use the domain suffix list to create additional FQDNs in the order listed and query DNS servers for each.
Example: samples.microsoft.com example.microsoft.com")]
        public string[] DnsDomainSuffixSearchOrder
        {
            get
            {
                return (string[])((LateBoundObject["DNSDomainSuffixSearchOrder"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDnsEnabledForWinsResolutionNull
        {
            get
            {
                return (LateBoundObject["DNSEnabledForWINSResolution"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DNSEnabledForWINSResolution property indicates whether the Domain Name System (DNS) is enabled for name resolution over Windows Internet Naming Service (WINS) resolution. If the name cannot be resolved using DNS, the name request is forwarded to WINS for resolution.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool DnsEnabledForWinsResolution
        {
            get
            {
                if ((LateBoundObject["DNSEnabledForWINSResolution"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["DNSEnabledForWINSResolution"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DNSHostName property indicates the host name used to identify the local computer for authentication by some utilities. Other TCP/IP-based utilities can use this value to acquire the name of the local computer. Host names are stored on DNS servers in a table that maps names to IP addresses for use by DNS. The name can be any combination of the letters A through Z, the numerals 0 through 9, and the hyphen (-), plus the period (.) character used as a separator. By default, this value is the Microsoft networking computer name, but the network administrator can assign another host name without affecting the computer name.
Example: corpdns")]
        public string DnsHostName
        {
            get
            {
                return ((string)(LateBoundObject["DNSHostName"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The DNSServerSearchOrder property indicates an ordered list of server IP addresse" +
            "s to be used in querying for DNS Servers.")]
        public string[] DnsServerSearchOrder
        {
            get
            {
                return (string[])((LateBoundObject["DNSServerSearchOrder"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDomainDnsRegistrationEnabledNull
        {
            get
            {
                return (LateBoundObject["DomainDNSRegistrationEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The DomainDNSRegistrationEnabled property specifies whether the IP addresses for this connection are registered in DNS under the domain name of this connection, in addition to registering under the computer's full DNS name. The domain name of this connection is either set via the method SetDNSDomain() or assigned by DHCP. The registered name is the host name of the computer with the domain name appended. Windows 2000 only.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool DomainDnsRegistrationEnabled
        {
            get
            {
                if ((LateBoundObject["DomainDNSRegistrationEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["DomainDNSRegistrationEnabled"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsForwardBufferMemoryNull
        {
            get
            {
                return (LateBoundObject["ForwardBufferMemory"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The ForwardBufferMemory property indicates how much memory IP allocates to store packet data in the router packet queue. When this buffer space is filled, the router begins discarding packets at random from its queue. Packet queue data buffers are 256 bytes in length, so the value of this parameter should be a multiple of 256. Multiple buffers are chained together for larger packets. The IP header for a packet is stored separately. This parameter is ignored and no buffers are allocated if the IP router is not enabled. The buffer size can range from the network MTU to the a value smaller than 0xFFFFFFFF. Default: 74240 (fifty 1480-byte packets, rounded to a multiple of 256).")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint ForwardBufferMemory
        {
            get
            {
                if ((LateBoundObject["ForwardBufferMemory"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["ForwardBufferMemory"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFullDnsRegistrationEnabledNull
        {
            get
            {
                return (LateBoundObject["FullDNSRegistrationEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The FullDNSRegistrationEnabled property specifies whether the IP addresses for this connection are registered in DNS under the computer's full DNS name. The full DNS name of the computer is displayed on the Network Identification tab of the System Control Panel. Windows 2000 only.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool FullDnsRegistrationEnabled
        {
            get
            {
                if ((LateBoundObject["FullDNSRegistrationEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["FullDNSRegistrationEnabled"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The GatewayCostMetric reflects an integer cost metric (ranging from 1 to 9999) to" +
            " be used in calculating the fastest, most reliable, and/or least expensive route" +
            "s. This argument has a one to one correspondence with the DefaultIPGateway. Wind" +
            "ows 2000 only.")]
        public ushort[] GatewayCostMetric
        {
            get
            {
                return ((ushort[])(LateBoundObject["GatewayCostMetric"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIgmpLevelNull
        {
            get
            {
                return (LateBoundObject["IGMPLevel"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IGMPLevel property indicates the extent to which the system supports IP multicast and participates in the Internet Group Management Protocol. At level 0, the system provides no multicast support. At level 1, the system may only send IP multicast packets. At level 2, the system may send IP multicast packets and fully participate in IGMP to receive multicast packets. Default: 2")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public IgmpLevelValues IgmpLevel
        {
            get
            {
                if ((LateBoundObject["IGMPLevel"] == null))
                {
                    return ((IgmpLevelValues)(Convert.ToInt32(3)));
                }
                return ((IgmpLevelValues)(Convert.ToInt32(LateBoundObject["IGMPLevel"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIndexNull
        {
            get
            {
                return (LateBoundObject["Index"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The Index property specifies the index number of the Win32 network adapter config" +
            "uration. The index number is used when there is more than one configuration avai" +
            "lable.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint Index
        {
            get
            {
                if ((LateBoundObject["Index"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["Index"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInterfaceIndexNull
        {
            get
            {
                return (LateBoundObject["InterfaceIndex"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The InterfaceIndex property contains the index value that uniquely identifies the" +
            " local interface.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint InterfaceIndex
        {
            get
            {
                if ((LateBoundObject["InterfaceIndex"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["InterfaceIndex"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPAddress property contains a list of all of the IP addresses associated with" +
            " the current network adapter.\nExample: 155.34.22.0")]
        public string[] IpAddress
        {
            get
            {
                return (string[])((LateBoundObject["IPAddress"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIpConnectionMetricNull
        {
            get
            {
                return (LateBoundObject["IPConnectionMetric"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPConnectionMetric indicates the cost of using the configured routes for this IP bound adapter and is the weighted value for those routes in the IP routing table. If there are multiple routes to a destination in the IP routing table, the route with the lowest metric is used. The default value is 1.Windows 2000 only.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint IpConnectionMetric
        {
            get
            {
                if ((LateBoundObject["IPConnectionMetric"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["IPConnectionMetric"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIpEnabledNull
        {
            get
            {
                return (LateBoundObject["IPEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPEnabled property indicates whether TCP/IP is bound and enabled on this netw" +
            "ork adapt.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool IpEnabled
        {
            get
            {
                if ((LateBoundObject["IPEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["IPEnabled"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIpFilterSecurityEnabledNull
        {
            get
            {
                return (LateBoundObject["IPFilterSecurityEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPFilterSecurityEnabled property indicates whether IP port security is enabled globally across all IP-bound network adapters. This property is used in conjunction with IPSecPermitTCPPorts, IPSecPermitUDPPorts, and IPSecPermitIPProtocols. A value of TRUE indicates that IP port security is enabled and that the security values associated with individual network adapters are in effect. A value of FALSE indicates IP filter security is disabled across all network adapters and allows all port and protocol traffic to flow unfiltered.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool IpFilterSecurityEnabled
        {
            get
            {
                if ((LateBoundObject["IPFilterSecurityEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["IPFilterSecurityEnabled"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIpPortSecurityEnabledNull
        {
            get
            {
                return (LateBoundObject["IPPortSecurityEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPPortSecurityEnabled property indicates whether IP port security is enabled " +
            "globally across all IP-bound network adapters. This property has been deprecated" +
            " in favor of IPFilterSecurityEnabled.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool IpPortSecurityEnabled
        {
            get
            {
                if ((LateBoundObject["IPPortSecurityEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["IPPortSecurityEnabled"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPSecPermitIPProtocols property lists the protocols permitted to run over the IP. The list of protocols is defined using the EnableIPSec method. The list will either be empty or contain numeric values. A numeric value of zero indicates access permission is granted for all protocols. An empty string indicates that no protocols are permitted to run when IPFilterSecurityEnabled is TRUE.")]
        public string[] IpSecPermitIpProtocols
        {
            get
            {
                return (string[])((LateBoundObject["IPSecPermitIPProtocols"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPSecPermitTCPPorts property lists the ports that will be granted access permission for TCP. The list of protocols is defined using the EnableIPSec method. The list will either be empty or contain numeric values. A numeric value of zero indicates access permission is granted for all ports. An empty string indicates that no ports are granted access permission when IPFilterSecurityEnabled is TRUE.")]
        public string[] IpSecPermitTcpPorts
        {
            get
            {
                return (string[])((LateBoundObject["IPSecPermitTCPPorts"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPSecPermitUDPPorts property lists the ports that will be granted User Datagram Protocol (UDP) access permission. The list of protocols is defined using the EnableIPSec method. The list will either be empty or contain numeric values. A numeric value of zero indicates access permission is granted for all ports. An empty string indicates that no ports are granted access permission when IPFilterSecurityEnabled is TRUE.")]
        public string[] IpSecPermitUdpPorts
        {
            get
            {
                return (string[])((LateBoundObject["IPSecPermitUDPPorts"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPSubnet property contains a list of all the subnet masks associated with the" +
            " current network adapter.\nExample: 255.255.0")]
        public string[] IpSubnet
        {
            get
            {
                return (string[])((LateBoundObject["IPSubnet"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIpUseZeroBroadcastNull
        {
            get
            {
                return (LateBoundObject["IPUseZeroBroadcast"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPUseZeroBroadcast property indicates whether IP zeros-broadcasts are used. If this parameter is set TRUE, then IP uses zeros-broadcasts (0.0.0.0), and the system uses ones-broadcasts (255.255.255.255). Computer systems generally use ones-broadcasts, but those derived from BSD implementations use zeros-broadcasts. Systems that do not use that same broadcasts will not interoperate on the same network. Default: FALSE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool IpUseZeroBroadcast
        {
            get
            {
                if ((LateBoundObject["IPUseZeroBroadcast"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["IPUseZeroBroadcast"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPXAddress property indicates the Internetworking Packet Exchange (IPX) addre" +
            "ss of the network adapter. The IPX address identifies a computer system on a net" +
            "work using the IPX protocol.")]
        public string IpxAddress
        {
            get
            {
                return ((string)(LateBoundObject["IPXAddress"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIpxEnabledNull
        {
            get
            {
                return (LateBoundObject["IPXEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPXEnabled property determines whether the or Internetwork Packet Exchange (I" +
            "PX) protocol is bound and enabled for this adapter.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool IpxEnabled
        {
            get
            {
                if ((LateBoundObject["IPXEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["IPXEnabled"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPXFrameType property represents an integer array of frame type identifiers. " +
            "The values in this array correspond to the elements in the IPXNetworkNumber.")]
        public IpxFrameTypeValues[] IpxFrameType
        {
            get
            {
                var arrEnumVals = ((Array)(LateBoundObject["IPXFrameType"]));
                var enumToRet = new IpxFrameTypeValues[arrEnumVals.Length];
                int counter;
                for (counter = 0; (counter < arrEnumVals.Length); counter = (counter + 1))
                {
                    enumToRet[counter] = ((IpxFrameTypeValues)(Convert.ToInt32(arrEnumVals.GetValue(counter))));
                }
                return enumToRet;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsIpxMediaTypeNull
        {
            get
            {
                return (LateBoundObject["IPXMediaType"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The IPXMediaType property represents an Internetworking Packet Exchange (IPX) med" +
            "ia type identifier.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public IpxMediaTypeValues IpxMediaType
        {
            get
            {
                if ((LateBoundObject["IPXMediaType"] == null))
                {
                    return ((IpxMediaTypeValues)(Convert.ToInt32(0)));
                }
                return ((IpxMediaTypeValues)(Convert.ToInt32(LateBoundObject["IPXMediaType"])));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPXNetworkNumber property represents an array of characters that uniquely identifies a frame/network adapter combination on the computer system. The NetWare Link (NWLink) IPX/SPX-compatible transport in Windows 2000 and Windows NT 4.0 and greater uses two distinctly different types of network numbers. This number is sometimes referred to as the external network number. It must be unique for each network segment. The order in this string list will correspond item-for-item with the elements in the IPXFrameType property.")]
        public string[] IpxNetworkNumber
        {
            get
            {
                return (string[])((LateBoundObject["IPXNetworkNumber"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The IPXVirtualNetNumber property uniquely identifies the computer system on the network. It is represented in the form of an eight-character hexadecimal digit. Windows NT/2000 uses the virtual network number (also known as an internal network number) for internal routing.")]
        public string IpxVirtualNetNumber
        {
            get
            {
                return ((string)(LateBoundObject["IPXVirtualNetNumber"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsKeepAliveIntervalNull
        {
            get
            {
                return (LateBoundObject["KeepAliveInterval"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The KeepAliveInterval property indicates the interval separating Keep Alive Retransmissions until a response is received. Once a response is received, the delay until the next Keep Alive Transmission is again controlled by the value of KeepAliveTime. The connection will be aborted after the number of retransmissions specified by TcpMaxDataRetransmissions have gone unanswered. Default: 1000, Valid Range: 1 - 0xFFFFFFFF.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint KeepAliveInterval
        {
            get
            {
                if ((LateBoundObject["KeepAliveInterval"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["KeepAliveInterval"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsKeepAliveTimeNull
        {
            get
            {
                return (LateBoundObject["KeepAliveTime"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The KeepAliveTime property indicates how often the TCP attempts to verify that an idle connection is still intact by sending a Keep Alive Packet. A remote system that is reachable will acknowledge the keep alive transmission. Keep Alive packets are not sent by default. This feature may be enabled in a connection by an application. Default: 7,200,000 (two hours)")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint KeepAliveTime
        {
            get
            {
                if ((LateBoundObject["KeepAliveTime"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["KeepAliveTime"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The MACAddress property indicates the Media Access Control (MAC) address of the n" +
            "etwork adapter. A MAC address is assigned by the manufacturer to uniquely identi" +
            "fy the network adapter.\nExample: 00:80:C7:8F:6C:96")]
        public string MacAddress
        {
            get
            {
                return ((string)(LateBoundObject["MACAddress"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsMtuNull
        {
            get
            {
                return (LateBoundObject["MTU"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The MTU property overrides the default Maximum Transmission Unit (MTU) for a network interface. The MTU is the maximum packet size (including the transport header) that the transport will transmit over the underlying network. The IP datagram can span multiple packets. The range of this value spans the minimum packet size (68) to the MTU supported by the underlying network.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint Mtu
        {
            get
            {
                if ((LateBoundObject["MTU"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["MTU"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsNumForwardPacketsNull
        {
            get
            {
                return (LateBoundObject["NumForwardPackets"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The NumForwardPackets property indicates the number of IP packet headers allocated for the router packet queue. When all headers are in use, the router will begin to discard packets from the queue at random. This value should be at least as large as the ForwardBufferMemory value divided by the maximum IP data size of the networks connected to the router. It should be no larger than the ForwardBufferMemory value divided by 256, since at least 256 bytes of forward buffer memory are used for each packet. The optimal number of forward packets for a given ForwardBufferMemory size depends on the type of traffic carried on the network. It will lie somewhere between these two values. If the router is not enabled, this parameter is ignored and no headers are allocated. Default: 50, Valid Range: 1 - 0xFFFFFFFE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint NumForwardPackets
        {
            get
            {
                if ((LateBoundObject["NumForwardPackets"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["NumForwardPackets"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPmtubhDetectEnabledNull
        {
            get
            {
                return (LateBoundObject["PMTUBHDetectEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The PMTUBHDetectEnabled property indicates whether detection of black hole routers occurs. Setting this parameter to TRUE causes TCP to try to detect black hole routers while discovering the path of the Maximum Transmission Unit. A black hole router does not return ICMP Destination Unreachable messages when it needs to fragment an IP datagram with the Don't Fragment bit set. TCP depends on receiving these messages to perform Path MTU Discovery. With this feature enabled, TCP will try to send segments without the Don't Fragment bit set if several retransmissions of a segment go unacknowledged. If the segment is acknowledged as a result, the MSS will be decreased and the Don't Fragment bit will be set in future packets on the connection. Enabling black hole detection increases the maximum number of retransmissions performed for a given segment. The default value of this property is FALSE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool PmtubhDetectEnabled
        {
            get
            {
                if ((LateBoundObject["PMTUBHDetectEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["PMTUBHDetectEnabled"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPmtuDiscoveryEnabledNull
        {
            get
            {
                return (LateBoundObject["PMTUDiscoveryEnabled"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The PMTUDiscoveryEnabled property indicates whether the Maximum Transmission Unit (MTU) path is discovered. Setting this parameter to TRUE causes TCP to attempt to discover the MTU (the largest packet size) over the path to a remote host. By discovering the MTU path and limiting TCP segments to this size, TCP can eliminate fragmentation at routers along the path that connect networks with different MTUs. Fragmentation adversely affects TCP throughput and network congestion. Setting this parameter to FALSE causes an MTU of 576 bytes to be used for all connections that are not to machines on the local subnet. Default: TRUE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool PmtuDiscoveryEnabled
        {
            get
            {
                if ((LateBoundObject["PMTUDiscoveryEnabled"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["PMTUDiscoveryEnabled"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The ServiceName property indicates the service name of the network adapter. This " +
            "name is usually shorter than the full product name. \nExample: Elnkii.")]
        public string ServiceName
        {
            get
            {
                return ((string)(LateBoundObject["ServiceName"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The identifier by which the CIM_Setting object is known.")]
        public string SettingId
        {
            get
            {
                return ((string)(LateBoundObject["SettingID"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTcpipNetbiosOptionsNull
        {
            get
            {
                return (LateBoundObject["TcpipNetbiosOptions"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The TcpipNetbiosOptions property specifies a bitmap of the possible settings rela" +
            "ted to NetBIOS over TCP/IP. Windows 2000 only.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public TcpipNetbiosOptionsValues TcpipNetbiosOptions
        {
            get
            {
                if ((LateBoundObject["TcpipNetbiosOptions"] == null))
                {
                    return ((TcpipNetbiosOptionsValues)(Convert.ToInt32(3)));
                }
                return ((TcpipNetbiosOptionsValues)(Convert.ToInt32(LateBoundObject["TcpipNetbiosOptions"])));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTcpMaxConnectRetransmissionsNull
        {
            get
            {
                return (LateBoundObject["TcpMaxConnectRetransmissions"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The TcpMaxConnectRetransmissions property indicates the number of times TCP will attempt to retransmit a Connect Request before terminating the connection. The initial retransmission timeout is 3 seconds. The retransmission timeout doubles for each attempt. Default: 3, Valid Range: 0 - 0xFFFFFFFF.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint TcpMaxConnectRetransmissions
        {
            get
            {
                if ((LateBoundObject["TcpMaxConnectRetransmissions"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["TcpMaxConnectRetransmissions"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTcpMaxDataRetransmissionsNull
        {
            get
            {
                return (LateBoundObject["TcpMaxDataRetransmissions"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The TcpMaxDataRetransmissions property indicates the number of times TCP will retransmit an individual data segment (non-connect segment) before terminating the connection. The retransmission timeout doubles with each successive retransmission on a connection. Default: 5, Valid Range: 0 - 0xFFFFFFFF.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint TcpMaxDataRetransmissions
        {
            get
            {
                if ((LateBoundObject["TcpMaxDataRetransmissions"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["TcpMaxDataRetransmissions"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTcpNumConnectionsNull
        {
            get
            {
                return (LateBoundObject["TcpNumConnections"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The TcpNumConnections property indicates the maximum number of connections that T" +
            "CP can have open simultaneously. Default: 0xFFFFFE, Valid Range: 0 - 0xFFFFFE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public uint TcpNumConnections
        {
            get
            {
                if ((LateBoundObject["TcpNumConnections"] == null))
                {
                    return Convert.ToUInt32(0);
                }
                return ((uint)(LateBoundObject["TcpNumConnections"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTcpUseRfc1122UrgentPointerNull
        {
            get
            {
                return (LateBoundObject["TcpUseRFC1122UrgentPointer"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The TcpUseRFC1122UrgentPointer property indicates whether TCP uses the RFC 1122 specification or the mode used by Berkeley Software Design (BSD) derived systems, for urgent data. The two mechanisms interpret the urgent pointer differently and are not interoperable. Windows 2000 and Windows NT version 3.51 and higher defaults to BSD mode. If TRUE, urgent data is sent in RFC 1122 mode. Default: FALSE.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool TcpUseRfc1122UrgentPointer
        {
            get
            {
                if ((LateBoundObject["TcpUseRFC1122UrgentPointer"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["TcpUseRFC1122UrgentPointer"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsTcpWindowSizeNull
        {
            get
            {
                return (LateBoundObject["TcpWindowSize"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The TcpWindowSize property contains the maximum TCP Receive  Window size offered by the system. The Receive Window specifies the number of bytes a sender may transmit without receiving an acknowledgment. In general, larger receiving windows will improve performance over high delay and high bandwidth networks. For efficiency, the receiving window should be an even multiple of the TCP Maximum Segment Size (MSS). Default: Four times the maximum TCP data size or an even multiple of TCP data size rounded up to the nearest multiple of 8192. Ethernet networks default to 8760. Valid Range: 0 - 65535.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public ushort TcpWindowSize
        {
            get
            {
                if ((LateBoundObject["TcpWindowSize"] == null))
                {
                    return Convert.ToUInt16(0);
                }
                return ((ushort)(LateBoundObject["TcpWindowSize"]));
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWinsEnableLmHostsLookupNull
        {
            get
            {
                return (LateBoundObject["WINSEnableLMHostsLookup"] == null);
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The WINSEnableLMHostsLookup property indicates whether local lookup files are use" +
            "d. Lookup files will contain a map of IP addresses to host names. If they exist " +
            "on the local system, they will be found in %SystemRoot%\\system32\\drivers\\etc.")]
        [TypeConverter(typeof(WmiValueTypeConverter))]
        public bool WinsEnableLmHostsLookup
        {
            get
            {
                if ((LateBoundObject["WINSEnableLMHostsLookup"] == null))
                {
                    return Convert.ToBoolean(0);
                }
                return (bool)((LateBoundObject["WINSEnableLMHostsLookup"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The WINSHostLookupFile property contains a path to a WINS lookup file on the local system. This file will contain a map of IP addresses to host names. If the file specified in this property is found, it will be copied to the %SystemRoot%\system32\drivers\etc folder of the local system. Valid only if the WINSEnableLMHostsLookup property is TRUE.")]
        public string WinsHostLookupFile
        {
            get
            {
                return ((string)(LateBoundObject["WINSHostLookupFile"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The WINSPrimaryServer property indicates the IP address for the primary WINS serv" +
            "er. ")]
        public string WinsPrimaryServer
        {
            get
            {
                return ((string)(LateBoundObject["WINSPrimaryServer"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(@"The WINSScopeID property provides a way to isolate a group of computer systems that communicate with each other only. The Scope ID is a character string value that is appended to the end of the NetBIOS name. It is used for all NetBIOS transactions  over TCP/IP communications from that computer system. Computers configured with identical Scope IDs are able to communicate with this computer. TCP/IP clients with different Scope IDs disregard packets from computers with this Scope ID. Valid only when the EnableWINS method executes successfully.")]
        public string WinsScopeId
        {
            get
            {
                return ((string)(LateBoundObject["WINSScopeID"]));
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("The WINSSecondaryServer property indicates the IP address for the secondary WINS " +
            "server. ")]
        public string WinsSecondaryServer
        {
            get
            {
                return ((string)(LateBoundObject["WINSSecondaryServer"]));
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

            if (((string.Compare(((string)(theObj["__CLASS"])), ManagementClassName, true, CultureInfo.InvariantCulture) == 0)))
            {
                return true;
            }
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
            return false;
        }

        private bool ShouldSerializeArpAlwaysSourceRoute()
        {
            return (IsArpAlwaysSourceRouteNull == false);
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

        private bool ShouldSerializeDhcpLeaseExpires()
        {
            return (IsDhcpLeaseExpiresNull == false);
        }

        private bool ShouldSerializeDhcpLeaseObtained()
        {
            return (IsDhcpLeaseObtainedNull == false);
        }

        private bool ShouldSerializeDnsEnabledForWinsResolution()
        {
            return (IsDnsEnabledForWinsResolutionNull == false);
        }

        private bool ShouldSerializeDomainDnsRegistrationEnabled()
        {
            return (IsDomainDnsRegistrationEnabledNull == false);
        }

        private bool ShouldSerializeForwardBufferMemory()
        {
            return (IsForwardBufferMemoryNull == false);
        }

        private bool ShouldSerializeFullDnsRegistrationEnabled()
        {
            return (IsFullDnsRegistrationEnabledNull == false);
        }

        private bool ShouldSerializeIgmpLevel()
        {
            return (IsIgmpLevelNull == false);
        }

        private bool ShouldSerializeIndex()
        {
            return (IsIndexNull == false);
        }

        private bool ShouldSerializeInterfaceIndex()
        {
            return (IsInterfaceIndexNull == false);
        }

        private bool ShouldSerializeIpConnectionMetric()
        {
            return (IsIpConnectionMetricNull == false);
        }

        private bool ShouldSerializeIpEnabled()
        {
            return (IsIpEnabledNull == false);
        }

        private bool ShouldSerializeIpFilterSecurityEnabled()
        {
            return (IsIpFilterSecurityEnabledNull == false);
        }

        private bool ShouldSerializeIpPortSecurityEnabled()
        {
            return (IsIpPortSecurityEnabledNull == false);
        }

        private bool ShouldSerializeIpUseZeroBroadcast()
        {
            return (IsIpUseZeroBroadcastNull == false);
        }

        private bool ShouldSerializeIpxEnabled()
        {
            return (IsIpxEnabledNull == false);
        }

        private bool ShouldSerializeIpxMediaType()
        {
            return (IsIpxMediaTypeNull == false);
        }

        private bool ShouldSerializeKeepAliveInterval()
        {
            return (IsKeepAliveIntervalNull == false);
        }

        private bool ShouldSerializeKeepAliveTime()
        {
            return (IsKeepAliveTimeNull == false);
        }

        private bool ShouldSerializeMtu()
        {
            return (IsMtuNull == false);
        }

        private bool ShouldSerializeNumForwardPackets()
        {
            return (IsNumForwardPacketsNull == false);
        }

        private bool ShouldSerializePmtubhDetectEnabled()
        {
            return (IsPmtubhDetectEnabledNull == false);
        }

        private bool ShouldSerializePmtuDiscoveryEnabled()
        {
            return (IsPmtuDiscoveryEnabledNull == false);
        }

        private bool ShouldSerializeTcpipNetbiosOptions()
        {
            return (IsTcpipNetbiosOptionsNull == false);
        }

        private bool ShouldSerializeTcpMaxConnectRetransmissions()
        {
            return (IsTcpMaxConnectRetransmissionsNull == false);
        }

        private bool ShouldSerializeTcpMaxDataRetransmissions()
        {
            return (IsTcpMaxDataRetransmissionsNull == false);
        }

        private bool ShouldSerializeTcpNumConnections()
        {
            return (IsTcpNumConnectionsNull == false);
        }

        private bool ShouldSerializeTcpUseRfc1122UrgentPointer()
        {
            return (IsTcpUseRfc1122UrgentPointerNull == false);
        }

        private bool ShouldSerializeTcpWindowSize()
        {
            return (IsTcpWindowSizeNull == false);
        }

        private bool ShouldSerializeWinsEnableLmHostsLookup()
        {
            return (IsWinsEnableLmHostsLookupNull == false);
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

        private static string ConstructPath(uint keyIndex)
        {
            var strPath = "root\\cimv2:Win32_NetworkAdapterConfiguration";
            strPath = string.Concat(strPath, string.Concat(".Index=", keyIndex.ToString(CultureInfo.InvariantCulture)));
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
            LateBoundObject = _privateLateBoundObject;
        }

        // Different overloads of GetInstances() help in enumerating instances of the WMI class.
        public static NetworkAdapterConfigurationCollection GetInstances()
        {
            return GetInstances(null, null, null);
        }

        public static NetworkAdapterConfigurationCollection GetInstances(string condition)
        {
            return GetInstances(null, condition, null);
        }

        public static NetworkAdapterConfigurationCollection GetInstances(string[] selectedProperties)
        {
            return GetInstances(null, null, selectedProperties);
        }

        public static NetworkAdapterConfigurationCollection GetInstances(string condition, string[] selectedProperties)
        {
            return GetInstances(null, condition, selectedProperties);
        }

        public static NetworkAdapterConfigurationCollection GetInstances(ManagementScope mgmtScope, EnumerationOptions enumOptions)
        {
            if ((mgmtScope == null))
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = "root\\cimv2" } };
            }
            var pathObj = new ManagementPath { ClassName = "Win32_NetworkAdapterConfiguration", NamespacePath = "root\\cimv2" };
            var clsObject = new ManagementClass(mgmtScope, pathObj, null);
            if ((enumOptions == null))
                enumOptions = new EnumerationOptions { EnsureLocatable = true };

            return new NetworkAdapterConfigurationCollection(clsObject.GetInstances(enumOptions));
        }

        public static NetworkAdapterConfigurationCollection GetInstances(ManagementScope mgmtScope, string condition)
        {
            return GetInstances(mgmtScope, condition, null);
        }

        public static NetworkAdapterConfigurationCollection GetInstances(ManagementScope mgmtScope, string[] selectedProperties)
        {
            return GetInstances(mgmtScope, null, selectedProperties);
        }

        public static NetworkAdapterConfigurationCollection GetInstances(ManagementScope mgmtScope, string condition, string[] selectedProperties)
        {
            if ((mgmtScope == null))
            {
                mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = "root\\cimv2" } };
            }
            var objectSearcher = new ManagementObjectSearcher(mgmtScope, new SelectQuery("Win32_NetworkAdapterConfiguration", condition, selectedProperties));
            var enumOptions = new EnumerationOptions { EnsureLocatable = true };
            objectSearcher.Options = enumOptions;
            return new NetworkAdapterConfigurationCollection(objectSearcher.Get());
        }

        [Browsable(true)]
        public static NetworkAdapterConfiguration CreateInstance()
        {
            var mgmtScope = _statMgmtScope ?? new ManagementScope { Path = { NamespacePath = CreatedWmiNamespace } };
            var mgmtPath = new ManagementPath(CreatedClassName);
            var tmpMgmtClass = new ManagementClass(mgmtScope, mgmtPath, null);
            return new NetworkAdapterConfiguration(tmpMgmtClass.CreateInstance());
        }

        [Browsable(true)]
        public void Delete()
        {
            _privateLateBoundObject.Delete();
        }

        public uint DisableIpSec()
        {
            if ((_isEmbedded == false))
            {
                var outParams = _privateLateBoundObject.InvokeMethod("DisableIPSec", null, null);
                if (outParams != null) return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            return Convert.ToUInt32(0);
        }

        public uint EnableDhcp()
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var outParams = _privateLateBoundObject.InvokeMethod("EnableDHCP", null, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint EnableDns(string dnsDomain, string[] dnsDomainSuffixSearchOrder, string dnsHostName, string[] dnsServerSearchOrder)
        {

            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("EnableDNS");
            inParams["DNSDomain"] = dnsDomain;
            inParams["DNSDomainSuffixSearchOrder"] = dnsDomainSuffixSearchOrder;
            inParams["DNSHostName"] = dnsHostName;
            inParams["DNSServerSearchOrder"] = dnsServerSearchOrder;
            var outParams = classObj.InvokeMethod("EnableDNS", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint EnableIpFilterSec(bool ipFilterSecurityEnabled)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("EnableIPFilterSec");
            inParams["IPFilterSecurityEnabled"] = ipFilterSecurityEnabled;
            var outParams = classObj.InvokeMethod("EnableIPFilterSec", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public uint EnableIpSec(string[] ipSecPermitIpProtocols, string[] ipSecPermitTcpPorts, string[] ipSecPermitUdpPorts)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("EnableIPSec");
            inParams["IPSecPermitIPProtocols"] = ipSecPermitIpProtocols;
            inParams["IPSecPermitTCPPorts"] = ipSecPermitTcpPorts;
            inParams["IPSecPermitUDPPorts"] = ipSecPermitUdpPorts;
            var outParams = _privateLateBoundObject.InvokeMethod("EnableIPSec", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public uint EnableStatic(string[] ipAddress, string[] subnetMask)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("EnableStatic");
            inParams["IPAddress"] = ipAddress;
            inParams["SubnetMask"] = subnetMask;
            var outParams = _privateLateBoundObject.InvokeMethod("EnableStatic", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint EnableWins(bool dnsEnabledForWinsResolution, bool winsEnableLmHostsLookup, string winsHostLookupFile, string winsScopeId)
        {
            {
                var mgmtPath = new ManagementPath(CreatedClassName);
                var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
                var inParams = classObj.GetMethodParameters("EnableWINS");
                inParams["DNSEnabledForWINSResolution"] = dnsEnabledForWinsResolution;
                inParams["WINSEnableLMHostsLookup"] = winsEnableLmHostsLookup;
                inParams["WINSHostLookupFile"] = winsHostLookupFile;
                inParams["WINSScopeID"] = winsScopeId;
                var outParams = classObj.InvokeMethod("EnableWINS", inParams, null);
                return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
            }
        }

        public uint ReleaseDhcpLease()
        {
            if ((_isEmbedded == false))
            {
                var outParams = _privateLateBoundObject.InvokeMethod("ReleaseDHCPLease", null, null);
                if (outParams != null) return Convert.ToUInt32(outParams.Properties["ReturnValue"].Value);
            }
            return Convert.ToUInt32(0);
        }

        public static uint ReleaseDhcpLeaseAll()
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var outParams = classObj.InvokeMethod("ReleaseDHCPLeaseAll", null, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public uint RenewDhcpLease()
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var outParams = _privateLateBoundObject.InvokeMethod("RenewDHCPLease", null, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint RenewDhcpLeaseAll()
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var outParams = classObj.InvokeMethod("RenewDHCPLeaseAll", null, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetArpAlwaysSourceRoute(bool arpAlwaysSourceRoute)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetArpAlwaysSourceRoute");
            inParams["ArpAlwaysSourceRoute"] = arpAlwaysSourceRoute;
            var outParams = classObj.InvokeMethod("SetArpAlwaysSourceRoute", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetArpUseEtherSnap(bool arpUseEtherSnap)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetArpUseEtherSNAP");
            inParams["ArpUseEtherSNAP"] = arpUseEtherSnap;
            var outParams = classObj.InvokeMethod("SetArpUseEtherSNAP", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetDatabasePath(string databasePath)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetDatabasePath");
            inParams["DatabasePath"] = databasePath;
            var outParams = classObj.InvokeMethod("SetDatabasePath", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetDeadGwDetect(bool deadGwDetectEnabled)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetDeadGWDetect");
            inParams["DeadGWDetectEnabled"] = deadGwDetectEnabled;
            var outParams = classObj.InvokeMethod("SetDeadGWDetect", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetDefaultTos(byte defaultTos)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetDefaultTOS");
            inParams["DefaultTOS"] = defaultTos;
            var outParams = classObj.InvokeMethod("SetDefaultTOS", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetDefaultTtl(byte defaultTtl)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetDefaultTTL");
            inParams["DefaultTTL"] = defaultTtl;
            var outParams = classObj.InvokeMethod("SetDefaultTTL", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public uint SetDnsDomain(string dnsDomain)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetDNSDomain");
            inParams["DNSDomain"] = dnsDomain;
            var outParams = _privateLateBoundObject.InvokeMethod("SetDNSDomain", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public uint SetDnsServerSearchOrder(string[] dnsServerSearchOrder)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetDNSServerSearchOrder");
            inParams["DNSServerSearchOrder"] = dnsServerSearchOrder;
            var outParams = _privateLateBoundObject.InvokeMethod("SetDNSServerSearchOrder", inParams, null);
            return outParams != null
                       ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value)
                       : Convert.ToUInt32(0);
        }

        public static uint SetDnsSuffixSearchOrder(string[] dnsDomainSuffixSearchOrder)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetDNSSuffixSearchOrder");
            inParams["DNSDomainSuffixSearchOrder"] = dnsDomainSuffixSearchOrder;
            var outParams = classObj.InvokeMethod("SetDNSSuffixSearchOrder", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public uint SetDynamicDnsRegistration(bool domainDnsRegistrationEnabled, bool fullDnsRegistrationEnabled)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetDynamicDNSRegistration");
            inParams["DomainDNSRegistrationEnabled"] = domainDnsRegistrationEnabled;
            inParams["FullDNSRegistrationEnabled"] = ((fullDnsRegistrationEnabled));
            var outParams = _privateLateBoundObject.InvokeMethod("SetDynamicDNSRegistration", inParams, null);
            return outParams != null
                       ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value)
                       : Convert.ToUInt32(0);
        }

        public static uint SetForwardBufferMemory(uint forwardBufferMemory)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetForwardBufferMemory");
            inParams["ForwardBufferMemory"] = forwardBufferMemory;
            var outParams = classObj.InvokeMethod("SetForwardBufferMemory", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public uint SetGateways(string[] defaultIpGateway, ushort[] gatewayCostMetric)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetGateways");
            inParams["DefaultIPGateway"] = ((defaultIpGateway));
            inParams["GatewayCostMetric"] = gatewayCostMetric;
            var outParams = _privateLateBoundObject.InvokeMethod("SetGateways", inParams, null);
            return outParams != null
                       ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value)
                       : Convert.ToUInt32(0);
        }

        public static uint SetIgmpLevel(byte igmpLevel)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetIGMPLevel");
            inParams["IGMPLevel"] = igmpLevel;
            var outParams = classObj.InvokeMethod("SetIGMPLevel", inParams, null);
            return outParams != null
                       ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value)
                       : Convert.ToUInt32(0);

        }

        public uint SetIpConnectionMetric(uint ipConnectionMetric)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetIPConnectionMetric");
            inParams["IPConnectionMetric"] = ipConnectionMetric;
            var outParams = _privateLateBoundObject.InvokeMethod("SetIPConnectionMetric", inParams, null);
            return outParams != null
                       ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value)
                       : Convert.ToUInt32(0);
        }

        public static uint SetIpUseZeroBroadcast(bool ipUseZeroBroadcast)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetIPUseZeroBroadcast");
            inParams["IPUseZeroBroadcast"] = ((ipUseZeroBroadcast));
            var outParams = classObj.InvokeMethod("SetIPUseZeroBroadcast", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public uint SetIpxFrameTypeNetworkPairs(uint[] ipxFrameType, string[] ipxNetworkNumber)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetIPXFrameTypeNetworkPairs");
            inParams["IPXFrameType"] = ((ipxFrameType));
            inParams["IPXNetworkNumber"] = ((ipxNetworkNumber));
            var outParams = _privateLateBoundObject.InvokeMethod("SetIPXFrameTypeNetworkPairs", inParams, null);
            return outParams != null
                       ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value)
                       : Convert.ToUInt32(0);
        }

        public static uint SetIpxVirtualNetworkNumber(string ipxVirtualNetNumber)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetIPXVirtualNetworkNumber");
            inParams["IPXVirtualNetNumber"] = ipxVirtualNetNumber;
            var outParams = classObj.InvokeMethod("SetIPXVirtualNetworkNumber", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint SetKeepAliveInterval(uint keepAliveInterval)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetKeepAliveInterval");
            inParams["KeepAliveInterval"] = keepAliveInterval;
            var outParams = classObj.InvokeMethod("SetKeepAliveInterval", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint SetKeepAliveTime(uint keepAliveTime)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetKeepAliveTime");
            inParams["KeepAliveTime"] = keepAliveTime;
            var outParams = classObj.InvokeMethod("SetKeepAliveTime", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint SetMtu(uint mtu)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetMTU");
            inParams["MTU"] = mtu;
            var outParams = classObj.InvokeMethod("SetMTU", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint SetNumForwardPackets(uint numForwardPackets)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetNumForwardPackets");
            inParams["NumForwardPackets"] = numForwardPackets;
            var outParams = classObj.InvokeMethod("SetNumForwardPackets", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint SetPmtubhDetect(bool pmtubhDetectEnabled)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetPMTUBHDetect");
            inParams["PMTUBHDetectEnabled"] = ((pmtubhDetectEnabled));
            var outParams = classObj.InvokeMethod("SetPMTUBHDetect", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint SetPmtuDiscovery(bool pmtuDiscoveryEnabled)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetPMTUDiscovery");
            inParams["PMTUDiscoveryEnabled"] = ((pmtuDiscoveryEnabled));
            var outParams = classObj.InvokeMethod("SetPMTUDiscovery", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public uint SetTcpipNetbios(uint tcpipNetbiosOptions)
        {
            if (_isEmbedded)
            {
                return Convert.ToUInt32(0);
            }
            var inParams = _privateLateBoundObject.GetMethodParameters("SetTcpipNetbios");
            inParams["TcpipNetbiosOptions"] = tcpipNetbiosOptions;
            var outParams = _privateLateBoundObject.InvokeMethod("SetTcpipNetbios", inParams, null);
            return outParams != null
                       ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value)
                       : Convert.ToUInt32(0);
        }

        public static uint SetTcpMaxConnectRetransmissions(uint tcpMaxConnectRetransmissions)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetTcpMaxConnectRetransmissions");
            inParams["TcpMaxConnectRetransmissions"] = tcpMaxConnectRetransmissions;
            var outParams = classObj.InvokeMethod("SetTcpMaxConnectRetransmissions", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public static uint SetTcpMaxDataRetransmissions(uint tcpMaxDataRetransmissions)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetTcpMaxDataRetransmissions");
            inParams["TcpMaxDataRetransmissions"] = tcpMaxDataRetransmissions;
            var outParams = classObj.InvokeMethod("SetTcpMaxDataRetransmissions", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetTcpNumConnections(uint tcpNumConnections)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetTcpNumConnections");
            inParams["TcpNumConnections"] = tcpNumConnections;
            var outParams = classObj.InvokeMethod("SetTcpNumConnections", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetTcpUseRfc1122UrgentPointer(bool tcpUseRfc1122UrgentPointer)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetTcpUseRFC1122UrgentPointer");
            inParams["TcpUseRFC1122UrgentPointer"] = ((tcpUseRfc1122UrgentPointer));
            var outParams = classObj.InvokeMethod("SetTcpUseRFC1122UrgentPointer", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public static uint SetTcpWindowSize(ushort tcpWindowSize)
        {
            var mgmtPath = new ManagementPath(CreatedClassName);
            var classObj = new ManagementClass(_statMgmtScope, mgmtPath, null);
            var inParams = classObj.GetMethodParameters("SetTcpWindowSize");
            inParams["TcpWindowSize"] = tcpWindowSize;
            var outParams = classObj.InvokeMethod("SetTcpWindowSize", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);
        }

        public uint SetWinsServer(string winsPrimaryServer, string winsSecondaryServer)
        {
            var inParams = _privateLateBoundObject.GetMethodParameters("SetWINSServer");
            inParams["WINSPrimaryServer"] = winsPrimaryServer;
            inParams["WINSSecondaryServer"] = winsSecondaryServer;
            var outParams = _privateLateBoundObject.InvokeMethod("SetWINSServer", inParams, null);
            return outParams != null ? Convert.ToUInt32(outParams.Properties["ReturnValue"].Value) : Convert.ToUInt32(0);

        }

        public enum IgmpLevelValues
        {

            NoMulticast = 0,

            IpMulticast = 1,

            IpIgmpMulticast = 2,

            NullEnumValue = 3,
        }

        public enum IpxFrameTypeValues
        {

            Ethernet2 = 0,

            Ethernet8023 = 1,

            Ethernet8022 = 2,

            EthernetSnap = 3,

            Auto = 255,

            NullEnumValue = 256,
        }

        public enum IpxMediaTypeValues
        {

            Ethernet = 1,

            TokenRing = 2,

            Fddi = 3,

            Arcnet = 8,

            NullEnumValue = 0,
        }

        public enum TcpipNetbiosOptionsValues
        {

            EnableNetbiosViaDhcp = 0,

            EnableNetbios = 1,

            DisableNetbios = 2,

            NullEnumValue = 3,
        }

        // Enumerator implementation for enumerating instances of the class.
        public class NetworkAdapterConfigurationCollection : object, ICollection
        {

            private readonly ManagementObjectCollection _privColObj;

            public NetworkAdapterConfigurationCollection(ManagementObjectCollection objCollection)
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
                    array.SetValue(new NetworkAdapterConfiguration(((ManagementObject)(array.GetValue(nCtr)))), nCtr);
                }
            }

            public virtual IEnumerator GetEnumerator()
            {
                return new NetworkAdapterConfigurationEnumerator(_privColObj.GetEnumerator());
            }

            public class NetworkAdapterConfigurationEnumerator : object, IEnumerator
            {

                private readonly ManagementObjectCollection.ManagementObjectEnumerator _privObjEnum;

                public NetworkAdapterConfigurationEnumerator(ManagementObjectCollection.ManagementObjectEnumerator objEnum)
                {
                    _privObjEnum = objEnum;
                }

                public virtual object Current
                {
                    get
                    {
                        return new NetworkAdapterConfiguration(((ManagementObject)(_privObjEnum.Current)));
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
                if (_baseType.BaseType == typeof(Enum))
                    return (value.GetType() == destinationType) ? value : _baseConverter.ConvertTo(context, culture, value, destinationType);

                if (_baseType == destinationType & _baseType.BaseType == typeof(ValueType))
                {
                    if (context.PropertyDescriptor != null && (value == null
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

            private readonly ManagementBaseObject _lateBoundObject;

            public ManagementSystemProperties(ManagementBaseObject managedObject)
            {
                _lateBoundObject = managedObject;
            }

            [Browsable(true)]
            public int Genus
            {
                get
                {
                    return ((int)(_lateBoundObject["__GENUS"]));
                }
            }

            [Browsable(true)]
            public string Class
            {
                get
                {
                    return ((string)(_lateBoundObject["__CLASS"]));
                }
            }

            [Browsable(true)]
            public string Superclass
            {
                get
                {
                    return ((string)(_lateBoundObject["__SUPERCLASS"]));
                }
            }

            [Browsable(true)]
            public string Dynasty
            {
                get
                {
                    return ((string)(_lateBoundObject["__DYNASTY"]));
                }
            }

            [Browsable(true)]
            public string Relpath
            {
                get
                {
                    return ((string)(_lateBoundObject["__RELPATH"]));
                }
            }

            [Browsable(true)]
            public int PropertyCount
            {
                get
                {
                    return ((int)(_lateBoundObject["__PROPERTY_COUNT"]));
                }
            }

            [Browsable(true)]
            public string[] Derivation
            {
                get
                {
                    return (string[])((_lateBoundObject["__DERIVATION"]));
                }
            }

            [Browsable(true)]
            public string Server
            {
                get
                {
                    return ((string)(_lateBoundObject["__SERVER"]));
                }
            }

            [Browsable(true)]
            public string Namespace
            {
                get
                {
                    return ((string)(_lateBoundObject["__NAMESPACE"]));
                }
            }

            [Browsable(true)]
            public string PATH
            {
                get
                {
                    return ((string)(_lateBoundObject["__PATH"]));
                }
            }
        }
    }
}
