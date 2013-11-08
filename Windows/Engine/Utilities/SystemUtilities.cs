using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;
using Org.InCommon.InCert.Engine.Results.Misc;
using log4net;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class SystemUtilities
    {

        [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
        internal sealed class VersionInfo : Attribute
        {
            public VersionInfo(string name)
            {
                Name = name;
                IsServer = false;
            }
            public VersionInfo(string name, bool isServer)
            {
                Name = name;
                IsServer = isServer;
            }

            public string Name { get; private set; }
            public bool IsServer { get; private set; }
        }

        internal static string FriendlyName(this NativeMethods.KnownProductTypes productType)
        {
            try
            {
                var type = typeof (NativeMethods.KnownProductTypes);
                var info = type.GetField(productType.ToString());
                if (info == null)
                    return productType.ToString();

                var attribute = info.GetCustomAttributes(typeof(VersionInfo), true).FirstOrDefault() as VersionInfo;
                if (attribute == null)
                    return productType.ToString();

                return attribute.Name.ToStringOrDefault(productType.ToString());
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to get the friendly name for {0}: {1}", productType, e.Message);
                return productType.ToString();
            }
        }

        internal static bool IsFlaggedAsServer(this NativeMethods.KnownProductTypes productType)
        {
            try
            {
                var type = typeof(NativeMethods.KnownProductTypes);
                var info = type.GetField(productType.ToString());
                if (info == null)
                    return false;

                var attribute = info.GetCustomAttributes(typeof(VersionInfo), true).FirstOrDefault() as VersionInfo;
                if (attribute == null)
                    return false;

                return attribute.IsServer;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to the IsServer value for {0}: {1}", productType, e.Message);
                return false;
            }
        }
       

     

        public enum ControlPanelNames
        {
            None,
            DateTime,
            UserAccounts,
            SecurityCenter,
            Network,
            Firewall
        }

        private static readonly ILog Log = Logger.Create();
        private static readonly Dictionary<ControlPanelNames, Delegate> ControlPanelEntries = new Dictionary<ControlPanelNames, Delegate>
            {
               {ControlPanelNames.DateTime, new Func<ProcessStartInfo>(GetDateTimeControlPanelInfo) }, 
               {ControlPanelNames.UserAccounts, new Func<ProcessStartInfo>(GetUserAccountsControlPanelInfo) },
               {ControlPanelNames.SecurityCenter, new Func<ProcessStartInfo>(GetSecurityCenterControlPanelInfo)},
               {ControlPanelNames.Network, new Func<ProcessStartInfo>(GetNetworkControlPanelInfo)},
               {ControlPanelNames.Firewall, new Func<ProcessStartInfo>(GetFirewallControlPanelInfo)}
            };

        internal static class NativeMethods
        {
            
            
            [StructLayout(LayoutKind.Sequential)]
            public struct SystemPowerStatus
            {
                public byte ACLineStatus;
                public byte BatteryFlag;
                public byte BatteryLifePercent;
                public byte Reserved1;
                public uint BatteryLifeTime;
                public uint BatteryFullLifeTime;
            }


            internal enum NetJoinStatus
            {
                NetSetupUnknownStatus = 0,
                NetSetupUnjoined,
                NetSetupWorkgroupName,
                NetSetupDomainName
            }

            internal enum LogonType
            {
                Interactive = 2,
                Network = 3,
                Batch = 4,
                Service = 5,
                Unlock = 7,
                NetworkClearText = 8,
                NewCredentials = 9
            }

            internal enum LogonProvider
            {
                DefaultProvider = 0
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct OsVersionInfoEx
            {
                public int Size;
                public int MajorVersion;
                public int MinorVersion;
                public int BuildNumber;
                public int PlatformId;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
                public char[] Version;

                public short ServicePackMajor;
                public short ServicePackMinor;
                public short SuiteMask;
                public byte ProductType;
                public byte Reserved;
            }

            [Flags]
            public enum ExitWindows : uint
            {
                // ONE of the following five:
                LogOff = 0x00,
                ShutDown = 0x01,
                Reboot = 0x02,
                PowerOff = 0x08,
                RestartApps = 0x40,
                // plus AT MOST ONE of the following two:
                Force = 0x04,
                ForceIfHung = 0x10,
            }

            [Flags]
            internal enum ShutdownReason : uint
            {
                MajorApplication = 0x00040000,
                MajorHardware = 0x00010000,
                MajorLegacyApi = 0x00070000,
                MajorOperatingSystem = 0x00020000,
                MajorOther = 0x00000000,
                MajorPower = 0x00060000,
                MajorSoftware = 0x00030000,
                MajorSystem = 0x00050000,

                MinorBlueScreen = 0x0000000F,
                MinorCordUnplugged = 0x0000000b,
                MinorDisk = 0x00000007,
                MinorEnvironment = 0x0000000c,
                MinorHardwareDriver = 0x0000000d,
                MinorHotfix = 0x00000011,
                MinorHung = 0x00000005,
                MinorInstallation = 0x00000002,
                MinorMaintenance = 0x00000001,
                MinorMmc = 0x00000019,
                MinorNetworkConnectivity = 0x00000014,
                MinorNetworkCard = 0x00000009,
                MinorOther = 0x00000000,
                MinorOtherDriver = 0x0000000e,
                MinorPowerSupply = 0x0000000a,
                MinorProcessor = 0x00000008,
                MinorReconfig = 0x00000004,
                MinorSecurity = 0x00000013,
                MinorSecurityFix = 0x00000012,
                MinorSecurityFixUninstall = 0x00000018,
                MinorServicePack = 0x00000010,
                MinorServicePackUninstall = 0x00000016,
                MinorTermSrv = 0x00000020,
                MinorUnstable = 0x00000006,
                MinorUpgrade = 0x00000003,
                MinorWmi = 0x00000015,

                FlagUserDefined = 0x40000000,
                FlagPlanned = 0x80000000
            }

            // from http://msdn.microsoft.com/en-us/library/ms724358(VS.85).aspx
            // revised 1/14/2008 to make flags os version independant
            // revised 1/21/2008 to make enum public
            // revised 6/6/2009 to add new windows 7-specific types
            // revised 6/3/2013 to add new win 8 types and to convert to c#
            public enum KnownProductTypes
            {
                [VersionInfo("Business")]
                Business = 0x00000006, //Business
                
                [VersionInfo("Business")]
                BusinessN = 0x00000010, //Business N
                
                [VersionInfo("Cluster Server", true)]
                ClusterServer = 0x00000012, //Hpc Edition
                [VersionInfo("Cluster Server", true)]
                ClusterServerV = 0x00000040, //Server Hyper Core V
                [VersionInfo("Core")]
                Core = 0x00000065, //Windows 8
                [VersionInfo("Core")]
                CoreN = 0x00000062, //Windows 8 N
                [VersionInfo("Core China")]
                ChinaSpecific = 0x00000063, //Windows 8 China
                [VersionInfo("Core Single Language")]
                Singlelanguage = 0x00000064, //Windows 8 Single Language
                [VersionInfo("Data Center Server", true)]
                DatacenterEvaluationServer = 0x00000050, //Server Datacenter (Evaluation Installation)
                [VersionInfo("Data Center Server", true)]
                DatacenterServer = 0x00000008, //Server Datacenter (Full Installation)
                [VersionInfo("Data Center Server", true)]
                DatacenterServerCore = 0x0000000c, //Server Datacenter (Core Installation)
                [VersionInfo("Data Center Server", true)]
                DatacenterServerCoreV = 0x00000027, //Server Datacenter Without Hyper-V (Core Installation)
                [VersionInfo("Data Center Server", true)]
                DatacenterServerV = 0x00000025, //Server Datacenter Without Hyper-V (Full Installation)
                [VersionInfo("Enterprise")]
                Enterprise = 0x00000004, //Enterprise
                [VersionInfo("Enterprise")]
                EnterpriseE = 0x00000046, //Not Supported
                [VersionInfo("Enterprise")]
                EnterpriseNEvaluation = 0x00000054, //Enterprise N (Evaluation Installation)
                [VersionInfo("Enterprise")]
                EnterpriseN = 0x0000001b, //Enterprise N
                [VersionInfo("Enterprise")]
                EnterpriseEvaluation = 0x00000048, //Server Enterprise (Evaluation Installation)
                [VersionInfo("Enterprise Server", true)]
                EnterpriseServer = 0x0000000a, //Server Enterprise (Full Installation)
                [VersionInfo("Enterprise Server", true)]
                EnterpriseServerCore = 0x0000000e, //Server Enterprise (Core Installation)
                [VersionInfo("Enterprise Server", true)]
                EnterpriseServerCoreV = 0x00000029, //Server Enterprise Without Hyper-V (Core Installation) 
                [VersionInfo("Enterprise Server", true)]
                EnterpriseServerIa64 = 0x0000000f, //Server Enterprise For Itanium-Based Systems
                [VersionInfo("Enterprise Server", true)]
                EnterpriseServerV = 0x00000026, //Server Enterprise Without Hyper-V (Full Installation)
                [VersionInfo("Essential Business Server", true)]
                EssentialbusinessServerMgmt = 0x0000003b, //Windows Essential Server Solution Management
                [VersionInfo("Essential Business Server", true)]
                EssentialbusinessServerAddl = 0x0000003c, //Windows Essential Server Solution Additional
                [VersionInfo("Essential Business Server", true)]
                EssentialbusinessServerMgmtService = 0x0000003d, //Windows Essential Server Solution Management Svc
                [VersionInfo("Essential Business Server", true)]
                EssentialbusinessServerAddlService = 0x0000003e, //Windows Essential Server Solution Additional Svc
                [VersionInfo("Home Basic")]
                HomeBasic = 0x00000002, //Home Basic 
                [VersionInfo("Home Basic")]
                HomeBasicE = 0x00000043, //Not Supported
                [VersionInfo("Home Basic")]
                HomeBasicN = 0x00000005, //Home Basic N
                [VersionInfo("Home Premium")]
                HomePremium = 0x00000003, //Home Premium
                [VersionInfo("Home Premium")]
                HomePremiumE = 0x00000044, //Not Supported
                [VersionInfo("Home Premium")]
                HomePremiumN = 0x0000001a, //Home Premium N
                [VersionInfo("Home Premium Server", true)]
                HomePremiumServer = 0x00000022, //Windows Home Server 2011
                [VersionInfo("Home Server", true)]
                HomeServer = 0x00000013, //Windows Storage Server 2008 R2 Essentials
                [VersionInfo("HyperV Server", true)]
                HyperVServer = 0x0000002a, //Microsoft Hyper-V Server
                [VersionInfo("Business Server Management", true)]
                MediumbusinessServerManagement = 0x0000001e, //Windows Essential Business Server Management Server
                [VersionInfo("Business Server Messaging", true)]
                MediumbusinessServerMessaging = 0x00000020, //Windows Essential Business Server Messaging Server
                [VersionInfo("Business Server Security", true)]
                MediumbusinessServerSecurity = 0x0000001f, // Windows Essential Business Server Security Server
                [VersionInfo("Multipoint Standard Server", true)]
                MultipointStandardServer = 0x0000004c, //Windows Multipoint Server Standard (Full Installation)
                [VersionInfo("Multipoint Premium Server", true)]
                MultipointPremiumServer = 0x0000004d, //Windows Multipoint Server Premium (Full Installation)
                [VersionInfo("Professional")]
                Professional = 0x00000030, // Professional
                [VersionInfo("Professional")]
                ProfessionalE = 0x00000045, // Not Supported
                [VersionInfo("Professional")]
                ProfessionalN = 0x00000031, // Professional N
                [VersionInfo("Professional")]
                ProfessionalWmc = 0x00000067, //Professional With Media Center
                [VersionInfo("Small Business Server", true)]
                SmallBusinessSolutionServerEm = 0x00000036, //Server For Sb Solutions Em
                [VersionInfo("Small Business Server", true)]
                ServerForSmallBusinessSolutions = 0x00000033, //Server For Sb Solutions
                [VersionInfo("Small Business Server", true)]
                ServerForSmallBusinessSolutionsEm = 0x00000037, //Server For Sb Solutions Em
                [VersionInfo("Small Business Server", true)]
                ServerForSmallbusiness = 0x00000018, //Windows Server 2008 For Windows Essential Server Solutions
                [VersionInfo("Small Business Server", true)]
                ServerForSmallbusinessV = 0x00000023, //Windows Server 2008 Without Hyper-V For Windows Essential Server Solutions
                [VersionInfo("Server Foundation", true)]
                ServerFoundation = 0x00000021, //Server Foundation
                [VersionInfo("Small Business Server", true)]
                SmallBusinessSolutionServer = 0x00000032, //Windows Small Business Server 2011 Essentials
                [VersionInfo("Small Business Server", true)]
                SmallBusinessServer = 0x00000009, //Windows Small Business Server
                [VersionInfo("Small Business Server", true)]
                SmallBusinessServerPremium = 0x00000019, //Small Business Server Premium
                [VersionInfo("Small Business Server", true)]
                SmallBusinessServerPremiumCore = 0x0000003f, //Small Business Server Premium (Core Installation)
                [VersionInfo("Multipoint Server", true)]
                SolutionEmbeddedserver = 0x00000038, //Windows Multipoint Server
                [VersionInfo("Standard Server", true)]
                StandardEvaluationServer = 0x0000004f, //Server Standard (Evaluation Installation)
                [VersionInfo("Standard Server", true)]
                StandardServer = 0x00000007, //Server Standard
                [VersionInfo("Standard Server", true)]
                StandardServerCore = 0x0000000d, //Server Standard (Core Installation)
                [VersionInfo("Standard Server", true)]
                StandardServerV = 0x00000024, //Server Standard Without Hyper-V
                [VersionInfo("Standard Server", true)]
                StandardServerCoreV = 0x00000028, //Server Standard Without Hyper-V (Core Installation)
                [VersionInfo("Standard Server", true)]
                StandardServerSolutions = 0x00000034, //Server Solutions Premium 
                [VersionInfo("Standard Server", true)]
                StandardServerSolutionsCore = 0x00000035, //Server Solutions Premium (Core Installation)
                [VersionInfo("Starter")]
                Starter = 0x0000000b, //Starter
                [VersionInfo("Starter")]
                StarterE = 0x00000042, //Not Supported
                [VersionInfo("Starter")]
                StarterN = 0x0000002f, //Starter N
                [VersionInfo("Enterprise Storage Server", true)]
                StorageEnterpriseServer = 0x00000017, //Storage Server Enterprise
                [VersionInfo("Enterprise Storage Server", true)]
                StorageEnterpriseServerCore = 0x0000002e, //Storage Server Enterprise (Core Installation)
                [VersionInfo("Enterprise Storage Server", true)]
                StorageExpressServer = 0x00000014, //Storage Server Express
                [VersionInfo("Enterprise Storage Server", true)]
                StorageExpressServerCore = 0x0000002b, //Storage Server Express (Core Installation)
                [VersionInfo("Enterprise Storage Server", true)]
                StorageStandardEvaluationServer = 0x00000060, //Storage Server Standard (Evaluation Installation)
                [VersionInfo("Enterprise Storage Server", true)]
                StorageStandardServer = 0x00000015, //Storage Server Standard
                [VersionInfo("Enterprise Storage Server", true)]
                StorageStandardServerCore = 0x0000002c, //Storage Server Standard (Core Installation)
                [VersionInfo("Workgroup Storage Server", true)]
                StorageWorkgroupEvaluationServer = 0x0000005f, //Storage Server Workgroup (Evaluation Installation)
                [VersionInfo("Workgroup Storage Server", true)]
                StorageWorkgroupServer = 0x00000016, //Storage Server Workgroup
                [VersionInfo("Workgroup Storage Server", true)]
                StorageWorkgroupServerCore = 0x0000002d, //Storage Server Workgroup (Core Installation)
                [VersionInfo("Unknown")]
                Undefined = 0x00000000, //An Unknown 
                [VersionInfo("Ultimate")]
                Ultimate = 0x00000001, //Ultimate
                [VersionInfo("Ultimate")]
                UltimateE = 0x00000047, //Not Supported
                [VersionInfo("Ultimate")]
                UltimateN = 0x0000001c, //Ultimate N
                [VersionInfo("Web Server", true)]
                WebServer = 0x00000011, //Web Server (Full Installation)
                [VersionInfo("Web Server", true)]
                WebServerCore = 0x0000001d //Web Server (Core Installation)
            }

            internal static readonly List<KnownProductTypes> ProfessionalProductTypes = new List<KnownProductTypes>
                {
                    KnownProductTypes.Business,
                    KnownProductTypes.BusinessN,
                    KnownProductTypes.Enterprise,
                    KnownProductTypes.EnterpriseE,
                    KnownProductTypes.EnterpriseNEvaluation,
                    KnownProductTypes.Professional,
                    KnownProductTypes.ProfessionalN,
                    KnownProductTypes.ProfessionalWmc,
                    KnownProductTypes.Ultimate,
                    KnownProductTypes.UltimateE,
                    KnownProductTypes.UltimateN
                };


            [DllImport("kernel32.dll", EntryPoint = "GetSystemPowerStatus")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetSystemPowerStatus([Out] out SystemPowerStatus systemPowerStatus);

            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool GetVersionEx([MarshalAs(UnmanagedType.Struct)] ref OsVersionInfoEx versionInfo);

            [DllImport("Kernel32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static internal extern bool GetProductInfo(int osMajorVersion, int osMinorVersion, int spMajorVersion, int spMinorVersion, ref KnownProductTypes edition);

            [DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int NetGetJoinInformation(
              string server,
              out IntPtr domain,
              out NetJoinStatus status);

            [DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int NetApiBufferFree(IntPtr buffer);

            [DllImport("advapi32.dll", EntryPoint = "LogonUserW", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool LogonUserW(
                [In] [MarshalAs(UnmanagedType.LPWStr)] string username,
                [In] [MarshalAs(UnmanagedType.LPWStr)] string domain,
                [In] IntPtr password,
                LogonType logonType,
                LogonProvider logonProvider,
                out IntPtr token);

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ExitWindowsEx(ExitWindows flags, ShutdownReason reason);

            [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CloseHandle([In] IntPtr handle);
        }

        /// <summary>
        /// Determines whether the computer is running Windows XP
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsXp()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            if (Environment.OSVersion.Version.Major != 5)
                return false;

            // 32-bit Windows XP
            if (Environment.OSVersion.Version.Minor == 1)
                return true;

            // something other than server version; return false
            if (Environment.OSVersion.Version.Minor != 2)
                return false;

            if (!Environment.Is64BitOperatingSystem)
                return false;

            var info = new NativeMethods.OsVersionInfoEx();
            info.Size = Marshal.SizeOf(info);
            if (!NativeMethods.GetVersionEx(ref info))
                return false;

            return info.ProductType != 11;
        }

        public static int GetServicePackVersion()
        {
            try
            {
                var info = new NativeMethods.OsVersionInfoEx();
                info.Size = Marshal.SizeOf(info);
                if (!NativeMethods.GetVersionEx(ref info))
                    throw new Win32Exception();

                return Convert.ToInt32(info.ServicePackMajor);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to determine this computer's service-pack version: {0}", e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Determines if the computer is running Windows Vista
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsVista()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            if (Environment.OSVersion.Version.Major != 6)
                return false;

            if (Environment.OSVersion.Version.Minor != 0)
                return false;

            return true;
        }

        /// <summary>
        /// Determines if the computer is running Windows 7
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows7()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            if (Environment.OSVersion.Version.Major != 6)
                return false;

            if (Environment.OSVersion.Version.Minor != 1)
                return false;

            return true;
        }

        /// <summary>
        /// Determines if the computer is running Windows 8
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows8()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;

            if (Environment.OSVersion.Version.Major != 6)
                return false;

            if (Environment.OSVersion.Version.Minor != 2)
                return false;

            return true;
        }


        /// <summary>
        /// Verifies that the utility is being run with admin privileges 
        /// </summary>
        /// <returns>true is the utility is being run as an administrator; false if not or if an issue occurs.</returns>
        public static bool IsRunningInAdminContext()
        {
            try
            {
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                var user = WindowsIdentity.GetCurrent();
                if (user == null)
                {
                    Log.Warn("Cannot get user from Windows Identity object; assuming not admin");
                    return false;
                }

                var principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception e)
            {
                Log.Warn("an exception occurred while attempting to verify that the utility is running in an admin context", e);
                return false;
            }
        }

        /// <summary>
        /// Determines whether the current user's account is a local account 
        /// by comparing the current user account's domain to the computer's domain
        /// </summary>
        /// <returns></returns>
        public static bool IsCurrentUserLocalAccount()
        {
            var domain = GetCurrentUserDomain();
            return domain.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Retrieves the domain associated with the user that is currently running the utility
        /// </summary>
        /// <returns>current domain or a null string if an issue occurs.</returns>
        public static string GetCurrentUserDomain()
        {
            try
            {
                var user = WindowsIdentity.GetCurrent();
                if (user == null)
                {
                    Log.Warn("Cannot determine current user's domain; returned user object is null.");
                    return "";
                }

                return Path.GetDirectoryName(user.Name);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to determine the current user's domain: {0}", e);
                return "";
            }
        }

        /// <summary>
        /// Retrieves the username (without the domain) associated with the user that is currently running the utility
        /// </summary>
        /// <returns>current domain or a null string if an issue occurs.</returns>
        public static string GetCurrentUserUsername()
        {
            try
            {
                var user = WindowsIdentity.GetCurrent();
                if (user == null)
                {
                    Log.Warn("Cannot determine current user's username; returned user object is null.");
                    return "";
                }

                return Path.GetFileName(user.Name);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to determine the current user's username: {0}", e);
                return "";
            }
        }

        /// <summary>
        /// Retrieves the username and domain associated with the user that is currently running the utility
        /// </summary>
        /// <returns>current username and domain  (in format [domain]\[username] or a null string if an issue occurs.</returns>
        public static string GetCurrentUserUsernameAndDomain()
        {
            try
            {
                var user = WindowsIdentity.GetCurrent();
                if (user == null)
                {
                    Log.Warn("Cannot determine current user's username and domain; returned user object is null.");
                    return "";
                }

                return user.Name;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to determine the current user's username and domain: {0}", e);
                return "";
            }
        }

        /// <summary>
        /// Determines whether the computer is joined to a domain
        /// </summary>
        /// <returns>BooleanReason class with result set to true and reason set to domain if computer is joined to a domain and no issues occur.  Otherwise, the result is a false BooleanReason result.</returns>
        public static BooleanReason IsComputerInAnyDomain()
        {
            var buffer = IntPtr.Zero;
            try
            {
                NativeMethods.NetJoinStatus status;
                if (NativeMethods.NetGetJoinInformation(null, out buffer, out status) != 0)
                    throw new Win32Exception();

                if (status != NativeMethods.NetJoinStatus.NetSetupDomainName)
                    return new BooleanReason(false, "This computer is not joined to a domain");

                var domain = Marshal.PtrToStringAuto(buffer);
                if (string.IsNullOrWhiteSpace(domain))
                    domain = "[unknown]";

                return new BooleanReason(true, "This computer is in the {0} domain.", domain);
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while determining whether this computer is in a domain: {0}", e.Message);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                {
                    NativeMethods.NetApiBufferFree(buffer);
                }
            }
        }

        /// <summary>
        /// Authenticates a user using the LogonUserW Api call.
        /// </summary>
        /// <param name="username">username of user in question</param>
        /// <param name="domain">domain of user in question. Use "." for local account.</param>
        /// <param name="password">password (as SecureString) of user in question</param>
        /// <returns>0 if the call succeeds; otherwise the Win32 issue code indicating why the call failed</returns>
        public static int AuthenticateUser(string username, string domain, SecureString password)
        {
            var passwordPointer = IntPtr.Zero;
            var token = IntPtr.Zero;
            try
            {
                if (password != null)
                    passwordPointer = Marshal.SecureStringToGlobalAllocUnicode(password);

                if (!NativeMethods.LogonUserW(
                    username, domain, passwordPointer, NativeMethods.LogonType.Batch,
                    NativeMethods.LogonProvider.DefaultProvider, out token))
                {
                    return new Win32Exception().NativeErrorCode;
                }
                return 0;
            }
            finally
            {
                if (passwordPointer != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(passwordPointer);

                if (token != IntPtr.Zero)
                    NativeMethods.CloseHandle(token);
            }
        }

        public static string GetDisplayNameForAccount(string username)
        {
            try
            {
                using (var machineEntry = new DirectoryEntry(@"WinNT://" + Environment.MachineName + ",computer"))
                {
                    using (var userEntry = machineEntry.Children.Find(username, "user"))
                    {
                        var rawValue = userEntry.Properties["FullName"].Value;
                        if (rawValue == null)
                            return username;

                        var displayName = rawValue.ToString();
                        return string.IsNullOrWhiteSpace(displayName) ? username : displayName;
                    }
                }
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to get the display name for the account {0}: {1}", username, e.Message);
                return username;
            }

        }

        public static void OpenControlPanel(ControlPanelNames value)
        {
            if (!ControlPanelEntries.ContainsKey(value))
                return;

            var info = ControlPanelEntries[value].DynamicInvoke() as ProcessStartInfo;
            if (info == null)
                return;

            Process.Start(info);
        }

        private static ProcessStartInfo GetDateTimeControlPanelInfo()
        {
            return new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("control.exe"))
                {
                    Arguments = "timedate.cpl",
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = false
                };

        }

        private static ProcessStartInfo GetUserAccountsControlPanelInfo()
        {
            return new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("control.exe"))
            {
                Arguments = "nusrmgr.cpl",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = false
            };
        }

        private static ProcessStartInfo GetSecurityCenterControlPanelInfo()
        {
            return new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("control.exe"))
            {
                Arguments = "wscui.cpl",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = false
            };
        }

        private static ProcessStartInfo GetNetworkControlPanelInfo()
        {
            return new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("control.exe"))
                {
                    Arguments = "ncpa.cpl",
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = false
                };
        }

        private static ProcessStartInfo GetFirewallControlPanelInfo()
        {
            return new ProcessStartInfo(PathUtilities.GetSystemUtilityPath("control.exe"))
                {
                    Arguments = "firewall.cpl",
                    WindowStyle = ProcessWindowStyle.Normal,
                    UseShellExecute = false
                };
        }

        public static void RestartComputer()
        {
            try
            {
                var lastState = new ProcessExtensions.TokenPrivileges();
                if (!Process.GetCurrentProcess().SetPrivilegeTokens("SeShutdownPrivilege", ref lastState))
                    throw new Exception("Could not restart computer: unable to set appropriate privileges for process");

                if (!NativeMethods.ExitWindowsEx(NativeMethods.ExitWindows.Reboot, 0))
                    throw new Win32Exception();

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to restart this computer: {0}", e.Message);
            }
        }

        public static bool IsProfessional()
        {
            try
            {
                var info = new NativeMethods.OsVersionInfoEx();
                info.Size = Marshal.SizeOf(info);
                if (!NativeMethods.GetVersionEx(ref info))
                    throw new Win32Exception();

                var productType = NativeMethods.KnownProductTypes.Undefined;
                if (!NativeMethods.GetProductInfo(info.MajorVersion, info.MajorVersion, info.ServicePackMajor, info.ServicePackMinor, ref productType))
                    throw new Win32Exception();


                return NativeMethods.ProfessionalProductTypes.Contains(productType);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine whether this is a professional os: {0}", e.Message);
                return false;
            }

        }

        public static string GetChassisType()
        {
            var result = GetChassisTypeFromWmi();
            if (result == SystemEnclosure.ChassisTypesValues.Other0 ||
                result == SystemEnclosure.ChassisTypesValues.NULL_ENUM_VALUE)
                result = GuessChassisTypeFromPowerSavings();

            return result.ToChassisString();
        }

        private static SystemEnclosure.ChassisTypesValues GetChassisTypeFromWmi()
        {
            try
            {
                var result = SystemEnclosure.ChassisTypesValues.NULL_ENUM_VALUE;
                foreach (SystemEnclosure instance in SystemEnclosure.GetInstances())
                {
                    if (instance.ChassisTypes == null)
                        continue;

                    if (instance.ChassisTypes.Length < 1)
                        continue;

                    result = instance.ChassisTypes[0];
                }

                return result;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to get this computer's chassis type from wmi: {0}", e.Message);
                return SystemEnclosure.ChassisTypesValues.NULL_ENUM_VALUE;
            }
        }

        private static SystemEnclosure.ChassisTypesValues GuessChassisTypeFromPowerSavings()
        {
            try
            {
                NativeMethods.SystemPowerStatus status;
                if (!NativeMethods.GetSystemPowerStatus(out status))
                    throw new Win32Exception();

                if (status.BatteryFlag.CompareTo(128) != 0 && status.BatteryFlag.CompareTo(255) != 0)
                    return SystemEnclosure.ChassisTypesValues.Laptop;

                return SystemEnclosure.ChassisTypesValues.Desktop;

            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to guess this computer's chassis type from its power status value: {0}", e.Message);
                return SystemEnclosure.ChassisTypesValues.NULL_ENUM_VALUE;
            }
        }

        private static string ToChassisString(this SystemEnclosure.ChassisTypesValues value)
        {
            if (value == SystemEnclosure.ChassisTypesValues.Other0)
                return "unknown";

            if (value == SystemEnclosure.ChassisTypesValues.NULL_ENUM_VALUE)
                return "unknown";

            return value.ToStringOrDefault("unknown").Replace("_", " ");
        }

        public static string GetOperatingSystemVersionName()
        {
            var productName = "other";
            var productType = GetProductType().FriendlyName();
            if (IsWindows8())
                productName = "Windows 8";

            if (IsWindows7())
                productName = "Windows 7";

            if (IsWindowsVista())
                productName = "Windows Vista";

            return string.Format("{0} {1}", productName, productType);
        }

        private static NativeMethods.KnownProductTypes GetProductType()
        {
            try
            {
                var productType = NativeMethods.KnownProductTypes.Undefined;
                NativeMethods.GetProductInfo(
                    Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor, 0, 0,
                    ref productType);

                return productType;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to get the windows product type {0}", e.Message);
                return NativeMethods.KnownProductTypes.Undefined;
            }

        }
    }
}
