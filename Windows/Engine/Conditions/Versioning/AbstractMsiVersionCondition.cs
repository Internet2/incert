using System;
using System.Runtime.InteropServices;
using System.Text;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Versioning
{
    public abstract class AbstractMsiVersionCondition : AbstractCondition
    {
        protected static class NativeMethods
        {
            [DllImport("msi.dll", CharSet = CharSet.Unicode)]
            internal static extern uint MsiEnumRelatedProducts(
                string upgradeCode,
                uint reserved,
                uint index,
                StringBuilder buffer);

            [DllImport("msi.dll", CharSet = CharSet.Unicode)]
            internal static extern uint MsiGetProductInfo(
                string product,
                string property,
                StringBuilder buffer,
                ref int len);
        }

        protected AbstractMsiVersionCondition(IEngine engine)
            : base (engine)
        {
        }

        /// <summary>
        /// Given a package's upgrade code, get its installed version
        /// </summary>
        /// <param name="upgradeCode"></param>
        /// <returns>Version. Note that a version of 0.0.0 indicates that the product might not be installed or no version information could be retrieved.</returns>
        protected static Version GetInstalledVersionForUpgradeCode(string upgradeCode)
        {
            if (string.IsNullOrWhiteSpace(upgradeCode))
                return new Version(0,0,0);
            
            uint index = 0;
            uint result;
            var version = new Version(0, 0, 0);
            do
            {
                var buffer = new StringBuilder(512);
                result = NativeMethods.MsiEnumRelatedProducts(upgradeCode, 0, index, buffer);
                if (result == 0)
                {
                    var instanceVersion = GetInstalledVersion(buffer.ToString());
                    if (instanceVersion != null)
                    {
                        if (instanceVersion >= version)
                            version = instanceVersion;
                    }
                }

                index++;
            } while (result == 0);

            return version;
        }
        
        /// <summary>
        /// Given a valid upgrade code, returns true if product or version related by upgrade code is installed
        /// </summary>
        /// <param name="upgradeCode"></param>
        /// <returns></returns>
        protected static bool IsProductInstalledByUpgradeCode(string upgradeCode)
        {
            var result = GetInstalledVersionForUpgradeCode(upgradeCode);
            return result > new Version(0, 0, 0);
        }


        /// <summary>
        /// Given a valid product code, returns true if product is installed (and have valid install date set)
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        protected static bool IsProductInstalled(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
                return false;
            
            var length = 512;
            var buffer = new StringBuilder(length);
            return 0 == NativeMethods.MsiGetProductInfo(productCode, "InstallDate", buffer, ref length);
        }

        /// <summary>
        /// Given a packages product code, get its installed version
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        protected static Version GetInstalledVersion(string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
                return new Version(0, 0, 0);
            
            var length = 512;
            var buffer = new StringBuilder(length);
            if (0 != NativeMethods.MsiGetProductInfo(productCode, "VersionString", buffer, ref length))
                return new Version(0, 0, 0, 0);

            Version result;
            return !Version.TryParse(buffer.ToString(), out result) ? new Version(0, 0, 0, 0) : result;
        }
    }
}
