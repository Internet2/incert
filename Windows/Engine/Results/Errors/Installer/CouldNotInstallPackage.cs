using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.Installer
{
    class CouldNotInstallPackage:ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Package { get; set; }
    }
}
