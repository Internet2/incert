using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.UserInterface
{
    internal class CouldNotLoadHtmlContent : ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Url { get; set; }

        public bool IsExternalUrl { get; set; }
    }
}