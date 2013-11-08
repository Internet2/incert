using System;
using System.IO;
using Microsoft.Win32;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.Utilities
{
    public static class RegistryUtilities
    {
        private static readonly ILog Log = Logger.Create();
        
        public enum RegistryRootValues
        {
            CurrentUser,
            LocalMachine,
            Unknown
        }
        
        public static bool KeyExists(this RegistryRootValues root, string keyPath)
        {
            try
            {
                RegistryKey registryRoot = null;

                if (root == RegistryRootValues.LocalMachine)
                    registryRoot = Registry.LocalMachine;

                if (root == RegistryRootValues.CurrentUser)
                    registryRoot = Registry.CurrentUser;

                if (registryRoot == null)
                {
                    Log.WarnFormat("Cannot open registry path. Root {0} is invalid", root);
                    return false;
                }

                using (var result = registryRoot.OpenSubKey(keyPath))
                {
                    return result != null;
                }

            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine whether a registry key exists: {0}", e.Message);
                return false;
            }
        }
        
        public static RegistryKey GetExistingKey(this RegistryRootValues root, string keyPath)
        {
             try
            {
                RegistryKey registryRoot = null;

                if (root == RegistryRootValues.LocalMachine)
                    registryRoot = Registry.LocalMachine;

                if (root == RegistryRootValues.CurrentUser)
                    registryRoot = Registry.CurrentUser;

                if (registryRoot == null)
                {
                    Log.WarnFormat("Cannot open registry path. Root {0} is invalid", root);
                    return null;
                }

                return registryRoot.OpenSubKey(keyPath);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while opening registry key {0} {1}: {2}", root, keyPath, e.Message);
                return null;
            }
        }
        
        public static RegistryKey OpenRegistryKey(this RegistryRootValues root, string keyPath, bool writable)
        {
            try
            {
                RegistryKey registryRoot = null;

                if (root == RegistryRootValues.LocalMachine)
                    registryRoot = Registry.LocalMachine;

                if (root == RegistryRootValues.CurrentUser)
                    registryRoot = Registry.CurrentUser;

                if (registryRoot == null)
                {
                    Log.WarnFormat("Cannot open registry path. Root {0} is invalid", root);
                    return null;
                }

                var permissionCheck = RegistryKeyPermissionCheck.ReadSubTree;
                if (writable)
                    permissionCheck = RegistryKeyPermissionCheck.ReadWriteSubTree;

                var result = registryRoot.CreateSubKey(keyPath, permissionCheck);
                if (result == null)
                    Log.WarnFormat("Cannot open or create registry key {0} {1}", root, keyPath);

                return result;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while opening registry key {0} {1}: {2}", root, keyPath, e.Message);
                return null;
            }
        }


        /// <summary>
        /// Opens an HKLM\SOFTWARE key that may exist in the \Wow6432Node or the normal node, favoring the WOW6432 value.
        /// </summary>
        /// <param name="keyPath">The relative key path, form either HKLM\Software or HKLM\Software\Wow6432Node</param>
        /// <param name="writable">Whether to open the key as writable</param>
        /// <returns>The key or null if it does not exist.</returns>
        /// <remarks>This method first looks for the HKLM\SOFTWARE\Wow6432Node version of the key and then for the HLKM\SOFTWARE version of the key. 
        /// If both exist, it will return the Wow6432Node version.</remarks>
        public static RegistryKey OpenWoW6432NodeKey(string keyPath, bool writable)
        {
            try
            {
                var permissionCheck = RegistryKeyPermissionCheck.ReadSubTree;
                if (writable)
                    permissionCheck = RegistryKeyPermissionCheck.ReadWriteSubTree;

                
                return Registry.LocalMachine.CreateSubKey(Path.Combine(new[] { "SOFTWARE", "WOW6432NODE", keyPath }), permissionCheck) 
                    ??Registry.LocalMachine.CreateSubKey(Path.Combine("SOFTWARE", keyPath), permissionCheck);    
            }
            catch(Exception e)
            {
                Log.WarnFormat("An issue occurred while opening 3264BitKey registry key {0}: {1}", keyPath, e.Message);
                return null;
            }
            
        }
    }
}
