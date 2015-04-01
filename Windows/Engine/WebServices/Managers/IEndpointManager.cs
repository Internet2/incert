using System;
using Org.InCommon.InCert.Engine.WebServices.Contracts;

namespace Org.InCommon.InCert.Engine.WebServices.Managers
{
    public interface IEndpointManager
    {
        void SetDefaultEndpoint(string value);
        string GetDefaultEndpoint();
        string GetEndpointForFunction(EndPointFunctions func);
        string GetDefaultHost();
        string ResolveContentUrl(string url);
        void SetEndpointForFunction(EndPointFunctions func, string value);
        void SetContractForFunction(EndPointFunctions func, string value);
        void SetContractForFunction(EndPointFunctions func, AbstractContract value);
        T GetContract<T>(EndPointFunctions func) where T:AbstractContract;

        string GetClientIdentifier();
        Guid GetSessionId();
    }
}