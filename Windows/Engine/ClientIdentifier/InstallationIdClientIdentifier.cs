using System;
using System.Linq;
using log4net;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.NativeCode.Wmi;

namespace Org.InCommon.InCert.Engine.ClientIdentifier
{
    public class InstallationIdClientIdentifier : AbstractClientIdentifier
    {
        private static readonly ILog Log = Logger.Create();
        private readonly ISoftwareLicensingProductProxy _productProxy;

        public InstallationIdClientIdentifier(ISoftwareLicensingProductProxy productProxy,
            IClientIdentifier backupIdentifier)
            : base(backupIdentifier)
        {
            _productProxy = productProxy;
        }

        public override string GetIdentifier()
        {
            try
            {
                const string windowsApplicationId = "55c92734-d682-4d71-983e-d6ec3f16059f";

                var product = _productProxy.GetInstances()
                    .OfType<SoftwareLicensingProduct>()
                    .FirstOrDefault(
                        e =>
                            !string.IsNullOrWhiteSpace(e.PartialProductKey) &&
                            e.ApplicationID.Equals(windowsApplicationId) &&
                            !string.IsNullOrWhiteSpace(e.OfflineInstallationId));

                return product == null 
                    ? GetBackupIdentifier() 
                    : product.OfflineInstallationId.ToSha1Hash();
            }


            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to determine this computer's installation-id-based identifier value: {0}", e.Message);
                return GetBackupIdentifier();
            }

        }

        
    }
}
