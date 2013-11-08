using System;
using System.Windows;
using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    public static class NetConfigExtensions
    {
        public static INetCfg GetNetConfigInstance()
        {
            var interfaceType = Type.GetTypeFromCLSID(NetCfgGuids.ClsidCNetCfg);
            return (INetCfg)Activator.CreateInstance(interfaceType);
        }
        
        public static IEnumNetCfgBindingPath ToComponentBindings(this INetCfg instance, string componentName)
        {
            object rawComponent;
            if (0 != instance.FindComponent(componentName, out rawComponent))
                throw new Exception(string.Format("Could not retrieve {0} component", componentName));

            var component = rawComponent as INetCfgComponent;
            if (component == null)
                throw new Exception(string.Format("Could not retrieve {0} component", componentName));

            var bindings = component.ToComponentBindings();

            object rawBindingPath;
            if (0 != bindings.EnumBindingPaths((int)BindingPathsFlags.Below, out rawBindingPath))
                throw new Exception("Could not retrieve binding paths");

            return (IEnumNetCfgBindingPath)rawBindingPath;
        }
        
        public static INetCfgLock ToInitializedLock(this INetCfg instance)
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            // this is legit - c# equivalent of queryinterface com operation
            var result = (INetCfgLock)instance;
            // ReSharper restore SuspiciousTypeConversion.Global

            string lockHeldBy;
            if (0 != result.AcquireWriteLock(5000, Application.Current.GetProductName(), out lockHeldBy))
                throw new Exception("Could not acquire lock");

            if (0 != instance.Initialize(IntPtr.Zero))
                throw new Exception("Could not initialize net config");

            return result;
        }

        public static INetCfgComponentBindings ToComponentBindings(this INetCfgComponent instance)
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            // this is legit - c# equivalent of queryinterface com operation
            return (INetCfgComponentBindings)instance;
            // ReSharper restore SuspiciousTypeConversion.Global
        }

        public static INetCfgBindingPath GetNextBindingPath(this IEnumNetCfgBindingPath instance)
        {
            object rawBinding;
            int fetched;
            if (0 != instance.Next(1, out rawBinding, out fetched))
                return null;

            if (fetched < 1)
                return null;

            return rawBinding as INetCfgBindingPath;
        }
    }
}
