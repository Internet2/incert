using System;
using System.IO;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Conditions.Encryption.Pgp
{
    class PgpInstalled:AbstractPgpCondition
    {
        public PgpInstalled(IEngine engine):base(engine)
        {
        }

        public override BooleanReason Evaluate()
        {
            try
            {
                var installPath = GetInstallPath();
                if (string.IsNullOrWhiteSpace(installPath))
                    return new BooleanReason(false, "Pgp install path not present in registry");

                var utilityPath = Path.Combine(installPath, "pgpwde.exe");
                if (!File.Exists(utilityPath))
                    return new BooleanReason(false, "Could not locate pgpwde.exe");

                return new BooleanReason(true, "Pgp registry key exists, and pgpwde.exe exists in installpath");
            }
            catch (Exception e)
            {
                return new BooleanReason(e);
            }
        }

        public override bool IsInitialized()
        {
            return true;
        }
    }
}
