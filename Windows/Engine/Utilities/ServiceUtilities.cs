using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.Services;
using Org.InCommon.InCert.Engine.Results.Misc;
using log4net;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class ServiceUtilities
    {
        internal static class NativeMethods
        {
            internal struct ServiceDelayedAutoStartInfo
            {
                public bool DelayedAutoStart;
            }

            internal const uint ServiceNoChange = 0xffffffff;

            [StructLayout(LayoutKind.Sequential)]
            internal struct QueryServiceConfigInfo
            {
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 ServiceType;
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 StartType;
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 ErrorControl;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string BinaryPathName;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string LoadOrderGroup;
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 TagId;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string Dependencies;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string ServiceStartName;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string DisplayName;
            }

            /// Return Type: BOOL->int
            ///hService: SC_HANDLE->SC_HANDLE__*
            ///dwServiceType: DWORD->unsigned int
            ///dwStartType: DWORD->unsigned int
            ///dwErrorControl: DWORD->unsigned int
            ///lpBinaryPathName: LPCWSTR->WCHAR*
            ///lpLoadOrderGroup: LPCWSTR->WCHAR*
            ///lpdwTagId: LPDWORD->DWORD*
            ///lpDependencies: LPCWSTR->WCHAR*
            ///lpServiceStartName: LPCWSTR->WCHAR*
            ///lpPassword: LPCWSTR->WCHAR*
            ///lpDisplayName: LPCWSTR->WCHAR*
            [DllImportAttribute("advapi32.dll", EntryPoint = "ChangeServiceConfigW", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern bool ChangeServiceConfigW(
                [In] SafeHandle serviceHandle, 
                int serviceType, 
                int startType, 
                int errorControl, 
                [In] [MarshalAs(UnmanagedType.LPWStr)] string binaryPathName,
                [In] [MarshalAs(UnmanagedType.LPWStr)] string loadOrderGroup, 
                IntPtr tagId, 
                [In] [MarshalAs(UnmanagedType.LPWStr)] string dependencies,
                [In] [MarshalAs(UnmanagedType.LPWStr)] string serviceStartName, 
                [In] [MarshalAs(UnmanagedType.LPWStr)] string password, 
                [In] [MarshalAs(UnmanagedType.LPWStr)] string displayName);

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern Boolean ChangeServiceConfig(
                SafeHandle serviceHandle,
                UInt32 serviceType,
                UInt32 startType,
                UInt32 errorControl,
                string binaryPathName,
                string loadOrderGroup,
                IntPtr tagId,
                [In] char[] lpDependencies,
                string serviceStartName,
                string password,
                string displayName);
            
        /*    [DllImport("AdvApi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern bool ChangeServiceConfig(
                SafeHandle handle, int serviceType,
                ServiceStartupValues startupValue,
                int errorControl,
                string binaryPathName,
                string loadOrderGroup,
                IntPtr tagIdPointer,
                string dependencies,
                string serviceStartName,
                string password,
                string displayName);*/

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            static internal extern int QueryServiceConfig(SafeHandle handle, IntPtr configPointer, int bufferSize, ref int bytesNeeded);

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            static internal extern int QueryServiceConfig2(SafeHandle handle, int infoLevel, IntPtr buffer, int bufferSize, ref int bytesNeeded);

            [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ChangeServiceConfig2(SafeHandle handle, int level, IntPtr infoPointer);
        }

        public enum ServiceStartupValues
        {
            Unknown = -1,
            Boot = 0,
            System = 1,
            Automatic = 2,
            Manual = 3,
            Disabled = 4,
            Delayed = 5
        }

        private static readonly ILog Log = Logger.Create();

        public static bool IsServiceRunning(string name)
        {
            using (var controller = GetServiceInstance(name))
            {
                if (controller == null)
                {
                    Log.WarnFormat("Service instance not found for service {0}", name);
                    return false;
                }

                return IsServiceRunning(controller);
            }
        }

        public static bool IsServiceRunning(ServiceController instance)
        {
            try
            {
                if (instance == null)
                {
                    Log.Warn("Service instance not found");
                    return false;
                }

                if (instance.Status != ServiceControllerStatus.Running)
                {
                    Log.WarnFormat("Service {0} reports status of {1}", instance.DisplayName,
                                   Enum.GetName(typeof(ServiceControllerStatus), instance.Status));
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Error("an issue occurred while attempting to determine is a service is running; assuming true", e);
                return true;
            }
        }

        public static bool IsServiceInfoInRegistry(string serviceName)
        {
            return
                RegistryUtilities.RegistryRootValues.LocalMachine.KeyExists(
                    Path.Combine(new[] { "System", "CurrentControlSet", "Services", serviceName }));
        }

        public static ServiceController GetServiceInstance(string name)
        {
            try
            {
                return
                    ServiceController.GetServices().FirstOrDefault(
                        i => i.ServiceName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public static BooleanReason SetServiceStartupType(ServiceController controller, ServiceStartupValues value)
        {
            try
            {
                if (controller == null)
                    return new BooleanReason(false, "could not determine service instance");

                if (value == ServiceStartupValues.Unknown)
                    return new BooleanReason(false, "Unknown is not a supported service auto-start value");

                if (value == ServiceStartupValues.Boot)
                    return new BooleanReason(false, "Boot is not a supported service auto-start value");


                if (value == ServiceStartupValues.System)
                    return new BooleanReason(false, "System is not a supported service auto-start value");

                if (GetServiceStartupValue(controller) == value)
                    return new BooleanReason(true, "");

                var enableDelayedStartup = false;
                if (value == ServiceStartupValues.Delayed)
                {
                    enableDelayedStartup = true;
                    value = ServiceStartupValues.Automatic;
                }

                var serviceHandle = controller.ServiceHandle;
                var result = NativeMethods.ChangeServiceConfig(
                    serviceHandle,
                    NativeMethods.ServiceNoChange,
                    (UInt32)value,
                    NativeMethods.ServiceNoChange,
                    null,
                    null,
                    IntPtr.Zero,
                    null,
                    null,
                    null,
                    null);

                if (!result)
                {
                    var issue = new Win32Exception();
                    var message = string.Format(
                        "Unable to start service {0}; Windows returned {1} ({2})", controller.DisplayName, issue.Message, issue.NativeErrorCode);
                    Log.Warn(message);
                    return new BooleanReason(false, message);
                }

                if (enableDelayedStartup)
                    EnableDelayedStartup(controller);

                return new BooleanReason(true, "");
            }
            catch (Exception e)
            {
                Log.Error(e);
                return new BooleanReason(false, "could not set service startup type: {0}", e.Message);
            }
        }

        public static ServiceStartupValues GetServiceStartupValue(ServiceController controller)
        {
            var configurationPointer = IntPtr.Zero;
            try
            {
                if (controller == null)
                {
                    Log.Warn("Cannot get service startup value: controller is invalid");
                    return ServiceStartupValues.Unknown;
                }

                var requiredBytes = 0;
                var result = NativeMethods.QueryServiceConfig(controller.ServiceHandle, configurationPointer, 0, ref requiredBytes);
                if (result != 0)
                {
                    Log.WarnFormat("Could not retrieve service startup configuration from Windows API.  Issue = {0}", result.ToString(CultureInfo.InvariantCulture));
                    return ServiceStartupValues.Unknown;
                }

                if (requiredBytes == 0)
                {
                    Log.Warn("Could not retrieve service startup configuration from Windows API.  Invalid data size returned");
                    return ServiceStartupValues.Unknown;
                }

                configurationPointer = Marshal.AllocCoTaskMem(requiredBytes);
                result = NativeMethods.QueryServiceConfig(controller.ServiceHandle, configurationPointer, requiredBytes, ref requiredBytes);
                if (result == 0)
                {
                    Log.Warn("Could not retrieve service startup configuration from Windows API");
                    return ServiceStartupValues.Unknown;
                }

                var configuration = (NativeMethods.QueryServiceConfigInfo)Marshal.PtrToStructure(configurationPointer, typeof(NativeMethods.QueryServiceConfigInfo));

                var startupValue = ServiceStartupValues.Unknown;
                switch (configuration.StartType)
                {
                    case 0:
                        startupValue = ServiceStartupValues.Boot;
                        break;
                    case 1:
                        startupValue = ServiceStartupValues.System;
                        break;
                    case 2:
                        startupValue = ServiceStartupValues.Automatic;
                        break;
                    case 3:
                        startupValue = ServiceStartupValues.Manual;
                        break;
                    case 4:
                        startupValue = ServiceStartupValues.Disabled;
                        break;
                }

                // check for delayed start, but only if the startup type is automatic
                if (startupValue == ServiceStartupValues.Automatic)
                {
                    if (GetServiceDelayedAutoStartValue(controller))
                    {
                        startupValue = ServiceStartupValues.Delayed;
                    }
                }

                return startupValue;
            }
            catch (Exception e)
            {
                Log.Warn("An exception occurred while attempting to determine the service startup configuration.", e);
                return ServiceStartupValues.Unknown;
            }
            finally
            {
                if (configurationPointer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(configurationPointer);
                }
            }
        }

        public static bool GetServiceDelayedAutoStartValue(ServiceController controller)
        {
            var configurationPointer = IntPtr.Zero;
            try
            {
                if (SystemUtilities.IsWindowsXp())
                {
                    return false;
                }

                var requiredBytes = 0;
                var result = NativeMethods.QueryServiceConfig2(controller.ServiceHandle, 3, configurationPointer, 0, ref requiredBytes);
                if (result != 0)
                {
                    Log.WarnFormat("Could not retrieve service startup configuration from Windows API.  Issue = {0}", result.ToString(CultureInfo.InvariantCulture));
                    return false;
                }

                if (requiredBytes == 0)
                {
                    Log.Warn("Could not retrieve service startup configuration from Windows API.  Invalid data size returned");
                    return false;
                }

                configurationPointer = Marshal.AllocCoTaskMem(requiredBytes);
                result = NativeMethods.QueryServiceConfig2(controller.ServiceHandle, 3, configurationPointer, requiredBytes, ref requiredBytes);
                if (result == 0)
                {
                    Log.Warn("Could not retrieve service startup configuration from Windows API");
                    return false;
                }

                var delayedAutoStartInfo = (NativeMethods.ServiceDelayedAutoStartInfo)Marshal.PtrToStructure(configurationPointer, typeof(NativeMethods.ServiceDelayedAutoStartInfo));

                return delayedAutoStartInfo.DelayedAutoStart;
            }
            catch (Exception e)
            {
                Log.Warn("An exception occurred while attempting to determine if service is set to delayed-auto-start", e);
                return false;
            }
            finally
            {
                if (configurationPointer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(configurationPointer);
                }
            }
        }

        public static BooleanReason EnableDelayedStartup(ServiceController controller)
        {
            var infoPointer = IntPtr.Zero;
            try
            {
                if (controller == null)
                    return new BooleanReason(false, "controller is invalid");

                var info = new NativeMethods.ServiceDelayedAutoStartInfo {DelayedAutoStart = true};
                infoPointer = Marshal.AllocHGlobal(Marshal.SizeOf(info));
                Marshal.StructureToPtr(info, infoPointer, true);
                if (!NativeMethods.ChangeServiceConfig2(controller.ServiceHandle, 3, infoPointer))
                {
                    var issue = new Win32Exception();
                    return new BooleanReason(false, "An issue occurred while attempting to set service startup type to delayed: {0} ({1})", issue.Message, issue.NativeErrorCode);    
                }

                return new BooleanReason(true,"");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while attempting to enable delayed startup: {0}",
                                         e.Message);
            }
            finally
            {
                if (infoPointer !=IntPtr.Zero)
                    Marshal.FreeHGlobal(infoPointer);
            }
        }

        public static BooleanReason DisableService(ServiceController controller)
        {
            try
            {
                if (controller == null)
                    return new BooleanReason(false, "The service could not be started; invalid controller");

                // now make sure the service is enabled
                if (controller.Status == ServiceControllerStatus.Stopped)
                    return new BooleanReason(true, "The {0} service is already stopped", controller.DisplayName);

                controller.Stop();

                // now wait for the service to be stopped
                controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 45));

                return new BooleanReason(true, "The {0} service was started successfully", controller.DisplayName);
            }
            catch (Exception e)
            {
                Log.Warn("an issue occurred while stopping the service", e);
                return new BooleanReason(false, e.Message); 
            }
        }

        public static BooleanReason EnableService(ServiceController controller)
        {
            try
            {
                if (controller == null)
                    return new BooleanReason(false, "The service could not be started; invalid controller");

                // now make sure the service is enabled
                if (controller.Status == ServiceControllerStatus.Running)
                    return new BooleanReason(true, "The {0} service is already running", controller.DisplayName);

                // now enable the service
                controller.Start();

                // now wait for the service to be started
                controller.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 15));

                return new BooleanReason(true, "The {0} service was started successfully", controller.DisplayName);
            }
            catch (Exception e)
            {
                Log.Warn("an issue occurred while starting the service", e);
                return new BooleanReason(false, e.Message);
            }
        }

        public static IResult EnableService(string serviceName)
        {
            using (var instance = GetServiceInstance(serviceName))
            {
                if (instance == null)
                {
                    if (!IsServiceInfoInRegistry(serviceName))
                        return new ServiceInfoNotInRegistry { ServiceName = serviceName };

                    return new ServiceInstanceNotAvailable { ServiceName = serviceName };
                }

                var result = EnableService(instance);
                if (!result.Result)
                {
                    Log.WarnFormat("Could not enable {0} service: {1}", serviceName, result.Reason);
                    return new ExceptionOccurred(new Exception(result.Reason));
                }

                return new NextResult();
            }

        }

        public static IResult DisableService(string serviceName)
        {
            using (var instance = GetServiceInstance(serviceName))
            {
                if (instance == null)
                {
                    if (!IsServiceInfoInRegistry(serviceName))
                        return new ServiceInfoNotInRegistry { ServiceName = serviceName };

                    return new ServiceInstanceNotAvailable { ServiceName = serviceName };
                }

                var result = DisableService(instance);
                if (!result.Result)
                {
                    Log.WarnFormat("Could not disable {0} service: {1}", serviceName, result.Reason);
                    return new ExceptionOccurred(new Exception(result.Reason));
                }

                return new NextResult();
            }

        }
    }


}
