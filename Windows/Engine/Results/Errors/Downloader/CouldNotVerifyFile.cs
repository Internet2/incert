using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.Downloader
{
    class CouldNotVerifyFile:ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Target { get; set; }
        
    }
}
