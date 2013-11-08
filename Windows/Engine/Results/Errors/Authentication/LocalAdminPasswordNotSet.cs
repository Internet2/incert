using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.Authentication
{
    class LocalAdminPasswordNotSet:ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Username { get; set; }

        [ResultExtensions.IncludeInErrorDetails]
        public string DisplayName { get; set; }
    }
}
