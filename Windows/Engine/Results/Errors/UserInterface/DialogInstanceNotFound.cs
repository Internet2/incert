using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.UserInterface
{
    class DialogInstanceNotFound: ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string Dialog { get; set; }
    }
}
