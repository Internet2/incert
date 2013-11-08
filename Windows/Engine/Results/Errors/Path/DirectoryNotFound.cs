using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.Path
{
    class DirectoryNotFound:ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Target { get; set; }
    }
}
