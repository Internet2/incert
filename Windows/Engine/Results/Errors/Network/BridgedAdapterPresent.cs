using Org.InCommon.InCert.Engine.Extensions;

namespace Org.InCommon.InCert.Engine.Results.Errors.Network
{
    class BridgedAdapterPresent:ErrorResult
    {
        [ResultExtensions.IncludeInErrorDetails]
        public string InterfaceName { get; set; }
        
    }
}
