using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.WebServices
{
    class CouldNotRetrieveContent: ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Target { get; set; }
    }
}
