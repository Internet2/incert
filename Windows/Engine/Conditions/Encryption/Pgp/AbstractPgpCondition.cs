using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.Pgp
{
    abstract class AbstractPgpCondition : AbstractEncryptionCondition
    {
        const string Wow6432RegistryPath = @"SOFTWARE\WOW6432Node\PGP CORPORATION\PGP";
        const string RegistryPath = @"SOFTWARE\PGP CORPORATION\PGP";

        protected AbstractPgpCondition(IEngine engine)
            : base (engine)
        {
        }

        protected static string GetInstallPath()
        {
            var result = GetWow6432InstallPath();
            if (!string.IsNullOrWhiteSpace(result))
                return result;

            return GetNormalInstallPath();
            
        }

        private static string GetNormalInstallPath()
        {
            using (var key=RegistryUtilities.RegistryRootValues.LocalMachine.OpenRegistryKey(RegistryPath, false)){
                if (key==null)
                    return "";

                var value = key.GetValue("INSTALLPATH");
                return value.ToStringOrDefault("");
            }
        }

        private static string GetWow6432InstallPath()
        {
            using (var key = RegistryUtilities.RegistryRootValues.LocalMachine.OpenRegistryKey(Wow6432RegistryPath, false)){
                if (key==null)
                    return "";
                
                var value = key.GetValue("INSTALLPATH");
                return value.ToStringOrDefault("");
            }
        }
    }
}
