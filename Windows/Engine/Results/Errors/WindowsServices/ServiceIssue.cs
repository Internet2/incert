using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.WindowsServices
{
    class ServiceIssue : ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Name { get; set; }
    }
}
