using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.UserInterface
{
    class BannerNotDefined:ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Banner { get; set; }
    }
}
