using System.Collections.Generic;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractIdentityQueryContract:AbstractAuthenticationContract
    {
        protected AbstractIdentityQueryContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public override EndPointFunctions SupportedFunction
        {
            get
            {
                return EndPointFunctions.IdentityQuery;
            }
        }

        public List<string> Properties { get; set; }
        public List<string> GroupPaths { get; set; }
    }
}
