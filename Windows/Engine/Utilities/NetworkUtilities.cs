using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using log4net;
using NativeWifi = Org.InCommon.InCert.Engine.NativeCode.NativeWifi.NativeMethods;

namespace Org.InCommon.InCert.Engine.Utilities
{



    public static class NetworkUtilities
    {
        internal static class NativeMethods
        {
            internal const int WlanInterfaceStateOperationCode = 6;
            internal const int DicsFlagGlobal = 0x00000001;
            internal const int DiregDrv = 0x00000002;
            internal const int MaxAdapterName = 128;
            internal const int KeyRead = 0x20019;
            internal const uint FormatMessageAllocateBuffer = 0x00000100;
            internal const uint FormatMessageIgnoreInserts = 0x00000200;
            internal const uint FormatMessageFromSystem = 0x00001000;
            internal const uint FormatMessageArgumentArray = 0x00002000;
            internal const uint FormatMessageFromHmodule = 0x00000800;
            internal const uint FormatMessageFromString = 0x00000400;

            [StructLayout(LayoutKind.Sequential)]
            internal struct DeviceInfoData
            {
                public Int32 cbSize;
                public Guid classGuid;
                public uint DevInst;
                public IntPtr Reserved;
            }


            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            internal struct IpAdapterIndexMap
            {
                public int Index;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxAdapterName)]
                public string Name;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            internal struct IpInterfaceInfo
            {
                public int NumAdapters;
                public IpAdapterIndexMap[] Adapters;

                public static IpInterfaceInfo FromByteArray(byte[] buffer)
                {
                    var handle = GCHandle.Alloc(0, GCHandleType.Pinned);
                    try
                    {
                        var returnValue = new IpInterfaceInfo();
                        var handleAddress = handle.AddrOfPinnedObject();
                        Marshal.Copy(buffer, 0, handleAddress, 4);
                        var adapterCount = Marshal.ReadInt32(handleAddress);
                        var offset = Marshal.SizeOf(adapterCount);
                        returnValue.NumAdapters = adapterCount;
                        returnValue.Adapters = new IpAdapterIndexMap[adapterCount];
                        for (var i = 0; i <= adapterCount - 1; i++)
                        {
                            var map = new IpAdapterIndexMap();
                            var mapPointer = Marshal.AllocHGlobal(Marshal.SizeOf(map));
                            Marshal.Copy(buffer, offset, mapPointer, Marshal.SizeOf(map));
                            map = (IpAdapterIndexMap)Marshal.PtrToStructure(mapPointer, typeof(IpAdapterIndexMap));
                            Marshal.FreeHGlobal(mapPointer);
                            returnValue.Adapters[i] = map;
                            offset = offset + Marshal.SizeOf(map);
                        }
                        return returnValue;
                    }
                    finally
                    {
                        handle.Free();
                    }
                }

                public IpAdapterIndexMap? GetIndexForNetworkInterface(NetworkInterface networkInterface)
                {
                    foreach (var adapter in Adapters.Where(adapter => adapter.Name.EndsWith(networkInterface.Id, StringComparison.InvariantCultureIgnoreCase)))
                        return adapter;

                    return null;
                }
            }

            [DllImport("advapi32.dll", EntryPoint = "RegQueryValueExW", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int RegQueryValueExW(
                [In] IntPtr hKey,
                [In] [MarshalAsAttribute(UnmanagedType.LPWStr)] string valueName,
                IntPtr reserved,
                ref int valueType,
                StringBuilder buffer,
                ref int bufferSize);

            [DllImport("advapi32.dll", EntryPoint = "RegCloseKey")]
            public static extern int RegCloseKey([In] IntPtr handle);

            [DllImport("Iphlpapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int GetInterfaceInfo(byte[] buffer, ref int bufferSize);

            [DllImport("Iphlpapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int IpReleaseAddress(ref IpAdapterIndexMap adapterIndexMap);

            [DllImport("Iphlpapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern int IpRenewAddress(ref IpAdapterIndexMap adapterIndexMap);

            [DllImport("iphlpapi.dll", SetLastError = true)]
            internal static extern int GetBestInterface(UInt32 destinationAddress, out int interfaceIndex);

            [DllImport("iphlpapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern int GetAdapterIndex(string adapter, out int index);

            [DllImport("setupapi.dll", EntryPoint = "SetupDiGetClassDevsW", SetLastError = true, CharSet = CharSet.Unicode,
                ExactSpelling = true, PreserveSig = true, CallingConvention = CallingConvention.Winapi)]
            internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, int flags);

            [DllImport("setupapi.dll", EntryPoint = "SetupDiDestroyDeviceInfoList", SetLastError = true,
                CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true,
                CallingConvention = CallingConvention.Winapi)]
            internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

            [DllImport("setupapi.dll", SetLastError = true)]
            internal static extern bool SetupDiGetDeviceRegistryProperty(
                IntPtr deviceInfoSet,
                ref DeviceInfoData deviceInfoData,
                int propertyId,
                out UInt32 propertyRegDataType,
                IntPtr buffer,
                int bufferSize,
                out UInt32 requiredSize);

            [DllImport("Setupapi", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr SetupDiOpenDevRegKey(
                IntPtr hDeviceInfoSet,
                ref DeviceInfoData deviceInfoData,
                int scope,
                int profile,
                int parameterRegistryValueKind,
                int samDesired);

            [DllImport("setupapi.dll", EntryPoint = "SetupDiEnumDeviceInfo", SetLastError = true, CharSet = CharSet.Unicode,
                ExactSpelling = true, PreserveSig = true, CallingConvention = CallingConvention.Winapi)]
            internal static extern bool SetupDiEnumDeviceInfo(
                IntPtr deviceInfoSet,
                int memberIndex,
                ref DeviceInfoData deviceInfoData);


        }

        internal static readonly ILog Log = Logger.Create();

        /// [summary>
        /// Returns the adapter that has the best connectivity to a given address
        /// [/summary>
        /// [param name="address">The address to try to connect to[/param>
        /// [returns>NetworkInterface object if found; null otherwise of if issue occurs[/returns>
        /// [see cref="http://www.pinvoke.net/default.aspx/iphlpapi.getbestinterface" />
        /// [seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/aa365922(v=vs.85).aspx" />
        public static NetworkInterface GetPrimaryAdapter(string address)
        {
            try
            {
                var hostInfo = Dns.GetHostEntry(address);

                var addressList = hostInfo.AddressList;
                if (!addressList.Any())
                {
                    Log.Warn("Could not resolve address for " + address);
                    return null;
                }

                var addressAsUint32 = ConvertIpAddressToUint32(addressList[0]);
                int index;
                var result = NativeMethods.GetBestInterface(addressAsUint32, out index);
                if (result != 0)
                {
                    var issue = new Win32Exception(Marshal.GetLastWin32Error());
                    Log.Warn("Unable to determine primary adapter: " + issue.Message);
                    return null;
                }

                return GetAdapterForApiIndex(index);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return null;
            }
        }

        public static NetworkInterface GetPrimaryAdapter(IEndpointManager endpointManager)
        {
            return GetPrimaryAdapter(endpointManager.GetDefaultHost());
        }

        public static IPAddress GetActiveIpAddress(IEndpointManager endpointManager)
        {
            try
            {
                var primaryAdapter = GetPrimaryAdapter(endpointManager.GetDefaultHost());
                if (primaryAdapter == null)
                    return GetIpAddressFromDns();

                foreach (var entry in primaryAdapter.GetIPProperties().UnicastAddresses
                    .Where(entry => entry.Address.AddressFamily == AddressFamily.InterNetwork))
                {
                    return entry.Address;
                }

                throw new Exception("Could not determine client ip address");
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to get this computer's ip address from primary adapter: {0}", e.Message);
                return null;
            }
        }

        public static IPAddress GetIpAddressFromDns()
        {
            try
            {
                foreach (var address in Dns.GetHostEntry(Dns.GetHostName()).AddressList
                    .Where(address => address.AddressFamily == AddressFamily.InterNetwork))
                {
                    return address;
                }

                throw new Exception("Could not determine IP address from DNS");
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to get this computer's ip address from DNS: {0}", e.Message);
                return null;
            }
        }

        public static PhysicalAddress GetActiveMacAddress(IEndpointManager endpointManager)
        {
            var address = GetActiveIpAddress(endpointManager);
            var adapter = GetAdapterForIpAddress(address);
            return adapter == null ? null : adapter.GetPhysicalAddress();
        }

        // from http://stackoverflow.com/questions/7384211/formatting-mac-address-in-c-sharp
        public static string ToDelimitedText(this PhysicalAddress address, string delimiter)
        {
            return address == null ? "" :
                 string.Join(":", (from z in address.GetAddressBytes() select z.ToString("X2")).ToArray());
        }

        private static NetworkInterface GetAdapterForIpAddress(IPAddress value)
        {
            try
            {
                if (value == null)
                    return null;

                foreach (var instance in from instance in NetworkInterface.GetAllNetworkInterfaces()
                                         where instance.GetIPProperties().UnicastAddresses.Any(address => address.Address.Equals(value))
                                         select instance)
                {
                    return instance;
                }

                throw new Exception("Could not find eligible instance");
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to get the network interface corresponding to the ip address {0}: {1}", value, e.Message);
                return null;
            }

        }

        /// [summary>
        /// Converts an IP address to it's uint32 equivalent
        /// [/summary>
        /// [param name="address">IPAddress to convert[/param>
        /// [returns>uint32 equivalent of IP Address[/returns>
        internal static UInt32 ConvertIpAddressToUint32(IPAddress address)
        {
            return BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray(), 0);
        }

        /// [summary>
        /// Returns a NetworkInterface that is assigned an API index that matches the specified index
        /// [/summary>
        /// [param name="index">The index of the adapter in question[/param>
        /// [returns>The matching NetworkInterface, or null if no matching interface is found[/returns>
        /// [remarks>This function uses the GetAdapterIndex api function to get the adapter index that is assigned to a network adapter. It then compares that index to the one specified.[/remarks>
        /// [see cref="http://msdn.microsoft.com/en-us/library/windows/desktop/aa365909(v=vs.85).aspx" />
        internal static NetworkInterface GetAdapterForApiIndex(int index)
        {
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                int adapterIndex;
                var result = NativeMethods.GetAdapterIndex(@"\DEVICE\TCPIP_" + adapter.Id, out adapterIndex);
                if (result != 0 || adapterIndex != index)
                    continue;

                return adapter;
            }

            return null;
        }

        /// <summary>
        /// Gets a list of all network adapters that are physical adapters.
        /// </summary>
        /// <returns>List of NetworkInterface objects for all physical adapters on the system</returns>
        /// <remarks>This function first excludes adapters with invalid network interface types. It then uses the Windows API to exclude adapters that have not been assigned to a system bus</remarks>
        public static List<NetworkInterface> GetActualAdapters()
        {
            try
            {
                return (from adapter in NetworkInterface.GetAllNetworkInterfaces()
                        where adapter.NetworkInterfaceType != NetworkInterfaceType.Unknown
                        where adapter.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                        where adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback
                        where IsPhysicalAdapter(adapter)
                        select adapter).ToList();
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new List<NetworkInterface>();
            }
        }

        private static bool IsPhysicalAdapter(NetworkInterface adapter)
        {
            var busPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            var flagsPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            var bufferPointer = Marshal.AllocHGlobal(256);
            var handle = IntPtr.Zero;
            try
            {
                var networkDevicesGuid = new Guid(0x4d36e972, 0xe325, 0x11ce, 0xbf, 0xc1, 0x8, 0x0, 0x2b, 0xe1, 0x3, 0x18);
                handle = NativeMethods.SetupDiGetClassDevs(ref networkDevicesGuid, IntPtr.Zero, IntPtr.Zero, 2);
                var issue = new Win32Exception(Marshal.GetLastWin32Error());
                if (handle.ToInt64() == -1)
                {
                    Log.WarnFormat("Could not retrieve handle to network device information: {0}", issue.Message);
                    return false;
                }

                var count = 0;
                var exitLoop = false;
                while (!exitLoop)
                {
                    var data = new NativeMethods.DeviceInfoData { cbSize = Marshal.SizeOf(typeof(NativeMethods.DeviceInfoData)) };
                    if (!NativeMethods.SetupDiEnumDeviceInfo(handle, count, ref data))
                    {
                        exitLoop = true;
                        continue;
                    }

                    count = count + 1;
                    UInt32 requiredSize;
                    UInt32 dataType;
                    if (!NativeMethods.SetupDiGetDeviceRegistryProperty(handle, ref data, 0, out dataType, bufferPointer, 256, out requiredSize))
                        continue;

                    var netCfgInstanceId = GetNetCfgInstanceIdForDevice(handle, data);
                    if (!netCfgInstanceId.Equals(adapter.Id, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    return NativeMethods.SetupDiGetDeviceRegistryProperty(handle, ref data, 21, out dataType, busPointer,
                                                           Marshal.SizeOf(typeof(int)), out requiredSize);
                }



                return false;

            }
            catch (Exception e)
            {
                Log.Warn(e);
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(busPointer);
                Marshal.FreeHGlobal(flagsPointer);
                Marshal.FreeHGlobal(bufferPointer);
                NativeMethods.SetupDiDestroyDeviceInfoList(handle);
            }
        }

        private static string GetNetCfgInstanceIdForDevice(IntPtr handle, NativeMethods.DeviceInfoData deviceInfoData)
        {
            var registryHandle = IntPtr.Zero;
            try
            {
                registryHandle = NativeMethods.SetupDiOpenDevRegKey(
                    handle,
                    ref deviceInfoData,
                    NativeMethods.DicsFlagGlobal,
                    0,
                    NativeMethods.DiregDrv,
                    NativeMethods.KeyRead);

                var size = 1024;
                var valueType = 0;
                var buffer = new StringBuilder(size);
                if (0 != NativeMethods.RegQueryValueExW(registryHandle, "NetCfgInstanceId",
                                                        IntPtr.Zero, ref valueType, buffer, ref size))
                {
                    Log.DebugFormat(
                        "An issue occurred while attempting to get the NetCfgInstanceId value for an adapter instance: {0}",
                        new Win32Exception().Message);
                    return "";
                }

                return buffer.ToString();
            }
            finally
            {
                if (registryHandle != IntPtr.Zero)
                    NativeMethods.RegCloseKey(registryHandle);
            }
        }

        /// <summary>
        /// Returns true if the adapter is question is a wireless interface
        /// </summary>
        /// <param name="adapter">The adapter to check</param>
        /// <returns></returns>
        public static bool IsAdapterWireless(NetworkInterface adapter)
        {
            var handle = IntPtr.Zero;
            var dataPointer = IntPtr.Zero;
            try
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    return true;

                // tunnel and loopback are obviously not wireless adapters
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    return false;

                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    return false;

                return IsAdapterWirelessApi(adapter);
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return false;
            }
            finally
            {
                if (dataPointer != IntPtr.Zero)
                    NativeWifi.WlanFreeMemory(dataPointer);

                if (handle != IntPtr.Zero)
                    NativeWifi.WlanCloseHandle(handle, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Uses the Windows Wlan API to determine where a network interface is a wireless adapter
        /// </summary>
        /// <param name="adapter">The network interface in question</param>
        /// <returns></returns>
        /// <remarks>sometimes Windows XP machines return ethernet at the interface type for wireless adapters.  The following uses the Wlan API to verify that the interface is a wireless adapter.</remarks>
        public static bool IsAdapterWirelessApi(NetworkInterface adapter)
        {
            var handle = IntPtr.Zero;
            var dataPointer = IntPtr.Zero;
            try
            {
                uint version;
                var result = NativeWifi.WlanOpenHandle(GetWlanClientVersion(), IntPtr.Zero, out version, out handle);
                if (result != 0)
                    return false;

                var adapterGuid = new Guid(adapter.Id);

                int dataSize;
                NativeWifi.WlanOpcodeValueType wlanType;
                result = NativeWifi.WlanQueryInterface(
                    handle,
                    adapterGuid,
                    NativeWifi.WlanIntfOpcode.InterfaceState,
                    IntPtr.Zero,
                    out dataSize,
                    out dataPointer,
                    out wlanType);

                return result == 0;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return false;
            }
            finally
            {
                if (dataPointer != IntPtr.Zero)
                    NativeWifi.WlanFreeMemory(dataPointer);

                if (handle != IntPtr.Zero)
                    NativeWifi.WlanCloseHandle(handle, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Releases and renews IP Leases for valid interfaces 
        /// </summary>
        /// <returns></returns>
        public static void ReleaseAndRenewInterfaces()
        {
            var interfaceInfo = GetIpInterfaceInfo();
            foreach (var adapter in GetActualAdapters())
            {
                var adapterIndexMap = interfaceInfo.GetIndexForNetworkInterface(adapter);
                if (!adapterIndexMap.HasValue)
                {
                    Log.WarnFormat("Could not determine Api adapter index for adapter {0}", adapter.Description);
                    continue;
                }

                var releaseTask = Task<int>.Factory.StartNew(() => ReleaseIpAddress(adapterIndexMap.Value, 0));
                releaseTask.WaitUntilExited();

                UserInterfaceUtilities.WaitForSeconds(DateTime.UtcNow, 1);

                var renewTask = Task<int>.Factory.StartNew(() => RenewIpAddress(adapterIndexMap.Value, 3));
                renewTask.WaitUntilExited();

                if (renewTask.Result != 0)
                    Log.WarnFormat(
                        "Could not renew ip lease for {0}: {1}",
                        adapter.Description,
                        new Win32Exception(renewTask.Result).Message);
            }

            if (AnyActualAdapterWithValidLease()) return;

            Log.Warn("No adapters present with valid ip release; attempting to renew leases with ipconfig /renew");
            RenewAllAdaptersWithIpConfig();
        }

        internal static int ReleaseIpAddress(NativeMethods.IpAdapterIndexMap adapterIndexMap, int retries)
        {
            int result;
            var count = 0;
            do
            {
                result = NativeMethods.IpReleaseAddress(ref adapterIndexMap);
                if (result == 0)
                    break;

                UserInterfaceUtilities.WaitForSeconds(DateTime.UtcNow, 1);
                count++;
            } while (count < retries);

            return result;
        }

        internal static int RenewIpAddress(NativeMethods.IpAdapterIndexMap adapterIndexMap, int retries)
        {
            int result;
            var count = 0;
            do
            {
                result = NativeMethods.IpRenewAddress(ref adapterIndexMap);
                if (result == 0)
                    break;

                UserInterfaceUtilities.WaitForSeconds(DateTime.UtcNow, 1);
                count++;
            } while (count < retries);

            return result;

        }



        private static NativeMethods.IpInterfaceInfo GetIpInterfaceInfo()
        {
            try
            {
                var bufferSize = 0;
                NativeMethods.GetInterfaceInfo(null, ref bufferSize);
                var buffer = new byte[bufferSize];
                if (0 != NativeMethods.GetInterfaceInfo(buffer, ref bufferSize))
                    throw new Win32Exception();

                return NativeMethods.IpInterfaceInfo.FromByteArray(buffer);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to get IpInterfaceInfo for Windows Api: {0}", e.Message);
                return new NativeMethods.IpInterfaceInfo();
            }
        }

        private static uint GetWlanClientVersion()
        {
            return (uint)(SystemUtilities.IsWindowsXp() ? 1 : 2);
        }

        public static List<NetworkInterface> GetWirelessAdapters()
        {
            return GetActualAdapters().Where(IsAdapterWireless).ToList();
        }

        public static List<NetworkInterface> GetWiredAdapters()
        {
            return GetActualAdapters().Where(adapter => !IsAdapterWireless(adapter)).ToList();
        }

        public static List<MacAddress> GetMacAddresses(IEnumerable<NetworkInterface> adapters)
        {
            var results = new List<MacAddress>();
            foreach (var adapter in adapters)
            {
                var value = NormalizeMacAddress(adapter);
                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.Warn("Could not get mac address for " + adapter.Description);
                    continue;
                }

                results.Add(new MacAddress { Address = value, Description = adapter.Description });
            }

            return results;
        }

        private static string NormalizeMacAddress(NetworkInterface adapter)
        {
            var value = adapter.GetPhysicalAddress().ToString();
            return string.IsNullOrWhiteSpace(value) ? "" : value.Replace(":", "");
        }

        public static bool AnyActualAdapterWithValidLease()
        {
            var adapters = GetActualAdapters();
            if (!adapters.Any())
            {
                Log.WarnFormat("Can't find actual adapters; assuming valid lease.");
                return true;
            }
            
            return adapters.Any(adapter => adapter.HasValidLease());
        }

        public static bool HasValidLease(this NetworkInterface instance)
        {
            // test if address is ipv4, isn't 0.0.0.0 and doesn't start with 169
            return instance.GetIPProperties().UnicastAddresses
                .Where(addressInfo => addressInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                .Where(addressInfo => !addressInfo.Address.ToString().Equals("0.0.0.0", StringComparison.InvariantCultureIgnoreCase))
                .Any(addressInfo => !addressInfo.Address.ToString().StartsWith("169", StringComparison.InvariantCultureIgnoreCase));
        }

        public static void RenewAllAdaptersWithIpConfig()
        {
            try
            {

                var info = new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("cmd.exe"))
                    {
                        Arguments = "/Cipconfig.exe /renew",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false
                    };

                Log.Debug("Renewing all ip leases with IpConfig /renew");
                using (var process = Process.Start(info))
                    process.WaitUntilExited();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to renew all adapters with IpConfig: {0}", e.Message);
            }
        }

        public static BooleanReason AddWirelessProfile(NetworkInterface adapter, string profile)
        {
            var handle = IntPtr.Zero;
            try
            {
                if (string.IsNullOrWhiteSpace(profile))
                {
                    Log.Warn("Cannot add wireless profile: profile text is empty");
                    return new BooleanReason(false, "Cannot add wireless profile: profile text is empty");
                }

                UInt32 negotiatedVersion;
                UInt32 version = 2;
                if (SystemUtilities.IsWindowsXp())
                    version = 1;

                var result = NativeWifi.WlanOpenHandle(version, IntPtr.Zero, out negotiatedVersion, out handle);
                if (result != 0)
                {
                    Log.WarnFormat("Could not open handle to native wifi interface: {0}", result);
                    return new BooleanReason(false, "Could not open handle to native wifi interface: {0}", result);
                }

                var identifier = new Guid(adapter.Id);
                NativeWifi.WlanReasonCode issueCode;

                result = NativeWifi.WlanSetProfile(
                    handle, identifier, NativeWifi.WlanProfileFlags.AllUser,
                    profile, null, true, IntPtr.Zero, out issueCode);

                if ((result != 0) && (result != 183))
                {
                    var issueText = GetTextForIssue(
                        "An issue occurred while attempting to set a wireless profile: {0}", issueCode);

                    Log.Warn(issueText);
                    return new BooleanReason(false, issueText);
                }

                return new BooleanReason(true, "");
            }

            catch (Exception e)
            {
                Log.Warn(e);
                return new BooleanReason(false, "An exception occurred while setting the wireless profile: {0}", e);
            }
            finally
            {
                if (handle != IntPtr.Zero)
                    NativeWifi.WlanCloseHandle(handle, IntPtr.Zero);
            }
        }

        public static BooleanReason HostPingable(string host, int timeout)
        {
            var pingTask = Task<PingReply>.Factory.StartNew(() => new Ping().Send(host, timeout));
            pingTask.WaitUntilExited();

            if (pingTask.IsFaulted)
            {
                if (pingTask.Exception != null)
                    return new BooleanReason(false, "An issue occurred while pinging {0}: {1}", host, pingTask.Exception);

                return new BooleanReason(false, "An unknown issue occurred while attempting to ping {0}", host);
            }

            if (pingTask.Result.Status != IPStatus.Success)
                return new BooleanReason(false, "{0}", pingTask.Result.Status);

            return new BooleanReason(true, "");
        }

        private static string GetTextForIssue(string text, NativeWifi.WlanReasonCode issueCode)
        {
            var defaultValue = string.Format(text, issueCode);
            try
            {
                var buffer = new StringBuilder(256);
                return NativeWifi.WlanReasonCodeToString(issueCode, 256, buffer, IntPtr.Zero) != 0 
                    ? defaultValue 
                    : string.Format(text, buffer);
            }
            catch (Exception)
            {
                return defaultValue;
            }

        }

        public static string GetConnectedWirelessNetwork(this NetworkInterface instance)
        {
            var handle = IntPtr.Zero;
            var dataPointer = IntPtr.Zero;
            try
            {
                if (instance.OperationalStatus != OperationalStatus.Up)
                    return "";

                uint version;
                var result = NativeWifi.WlanOpenHandle(GetWlanClientVersion(), IntPtr.Zero, out version, out handle);
                if (result != 0)
                {
                    Log.WarnFormat("An issue occurred while attempting to open a wlan api handle: {0}", result);
                    return "";
                }

                var guid = new Guid(instance.Id);
                int size;
                NativeWifi.WlanOpcodeValueType valueType;
                result = NativeWifi.WlanQueryInterface(handle, guid, NativeWifi.WlanIntfOpcode.CurrentConnection,
                    IntPtr.Zero, out size, out dataPointer, out valueType);
                if (result != 0)
                {
                    Log.WarnFormat("An issue occurred while attempting to query wireless interface: {0}", result);
                    return "";
                }

                var attributes = (NativeWifi.WlanConnectionAttributes)Marshal.PtrToStructure(dataPointer, typeof(NativeWifi.WlanConnectionAttributes));
                if (attributes.isState != NativeWifi.WlanInterfaceState.Connected)
                    return "";

                var ssid = attributes.wlanAssociationAttributes.dot11Ssid;
                var network = Encoding.ASCII.GetString(ssid.SSID);
                if (string.IsNullOrWhiteSpace(network))
                {
                    Log.Warn("Could not convert ssid to valid string");
                    return "";
                }

                if (network.Length > ssid.SSIDLength)
                    network = network.Substring(0, (int)ssid.SSIDLength);

                return network;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine connected wireless networks: {0}", e.Message);
                return "";
            }
            finally
            {
                if (handle != IntPtr.Zero)
                    NativeWifi.WlanCloseHandle(handle, IntPtr.Zero);

                if (dataPointer != IntPtr.Zero)
                    NativeWifi.WlanFreeMemory(dataPointer);
            }


        }

        public static string GetTextForProfile(this NetworkInterface instance, string profileName)
        {
            var handle = IntPtr.Zero;
            try
            {
                if (string.IsNullOrWhiteSpace(profileName))
                    return "";

                uint version;
                var result = NativeWifi.WlanOpenHandle(GetWlanClientVersion(), IntPtr.Zero, out version, out handle);
                if (result != 0)
                {
                    Log.WarnFormat("An issue occurred while attempting to open a wlan api handle: {0}", result);
                    return "";
                }

                var guid = new Guid(instance.Id);
                IntPtr textPointer;
                NativeWifi.WlanProfileFlags profileFlags;
                NativeWifi.WlanAccess accessFlags;
                result = NativeWifi.WlanGetProfile(handle, guid, profileName, IntPtr.Zero,
                    out textPointer, out profileFlags, out accessFlags);
                if (result != 0)
                {
                    if (result != 1168)
                        Log.WarnFormat("An issue occurred while attempting to get {0} profile text: {1}", profileName, result);

                    return "";
                }

                return Marshal.PtrToStringUni(textPointer);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to retrieve wireless profile text: {0}", e.Message);
                return "";
            }
            finally
            {
                if (handle != IntPtr.Zero)
                    NativeWifi.WlanCloseHandle(handle, IntPtr.Zero);
            }
        }

    }




}
