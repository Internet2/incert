using System;
using System.Collections.Generic;
using Org.InCommon.InCert.Engine.ClientIdentifier;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Contracts;
using Org.InCommon.InCert.Engine.WebServices.Contracts.Generic;
using Org.InCommon.InCert.Engine.WebServices.Contracts.InCommon;
using log4net;

namespace Org.InCommon.InCert.Engine.WebServices.Managers
{
    public class EndpointManager : IEndpointManager
    {
        private readonly IClientIdentifier _clientIdentifier;
        
        private readonly Guid _sessionId;
        private string _clientId;

        private static readonly ILog Log = Logger.Create();
        
        private readonly Dictionary<EndPointFunctions, string> _endpointMap = 
            new Dictionary<EndPointFunctions, string>();

        private readonly Dictionary<EndPointFunctions, AbstractContract> _contracts = 
            new Dictionary<EndPointFunctions, AbstractContract>();

        public EndpointManager(IClientIdentifier clientIdentifier)
        {
            _sessionId = Guid.NewGuid();
            _clientIdentifier = clientIdentifier;
            _contracts[EndPointFunctions.Default] = new ContentRequest(this);
            _contracts[EndPointFunctions.LogAsync] = new AsynchronousLogRequest(this);
            _contracts[EndPointFunctions.LogWait] = new SynchronousLogRequest(this);
            _contracts[EndPointFunctions.GetStatusInfo] = new StatusInfoRequest(this);
            _contracts[EndPointFunctions.GetFileInfo] = new FileInfoUrlRequest(this);
            _contracts[EndPointFunctions.GetContent] = new ContentRequest(this);
            _contracts[EndPointFunctions.AuthenticateUser] = new AuthenticateUserRequest(this);
            _contracts[EndPointFunctions.GetCertificate] = new GetCertificateRequest(this);
            _contracts[EndPointFunctions.RegisterComputer] = new RegisterComputerRequest(this);
        }

        public void SetDefaultEndpoint(string value)
        {
            _endpointMap[EndPointFunctions.Default] = value;
        }

        public string GetDefaultEndpoint()
        {
            return !_endpointMap.ContainsKey(EndPointFunctions.Default) ? "" 
                : _endpointMap[EndPointFunctions.Default];
        }

        public string GetEndpointForFunction(EndPointFunctions func)
        {
            return !_endpointMap.ContainsKey(func) 
                ? _endpointMap[EndPointFunctions.Default] 
                : ResolveUrl(_endpointMap[EndPointFunctions.Default], _endpointMap[func]);
        }

        public string GetDefaultHost()
        {
            try
            {
                if (!_endpointMap.ContainsKey(EndPointFunctions.Default))
                    throw new Exception("Do default endpoint defined");

                var uri = new Uri(_endpointMap[EndPointFunctions.Default]);
                return uri.DnsSafeHost;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to resolve the default host: {0}", e.Message);
                return "";
            }
        }

        private static string ResolveUrl(string baseUrl, string functionUrl)
        {
            try
            {
                var uri = new Uri(functionUrl, UriKind.RelativeOrAbsolute);
                if (uri.IsAbsoluteUri)
                    return uri.AbsoluteUri;

                var baseUri = new Uri(baseUrl);
                var fullUri = new Uri(baseUri, uri);
                var result = fullUri.AbsoluteUri;

                return result;
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to resolve the endpoint uri for the function {0}: {1}", functionUrl, e.Message);
                return functionUrl;
            }
        }

        public void SetEndpointForFunction(EndPointFunctions func, string value)
        {
           _endpointMap[func] = value;
        }

        public void SetContractForFunction(EndPointFunctions func, string value)
        {
            SetContractForFunction(func, ReflectionUtilities.LoadFromAssembly<AbstractContract>(value));
        }

        public void SetContractForFunction(EndPointFunctions func, AbstractContract value)
        {
            if (value == null)
                throw  new Exception("Contract cannot be null");

            if (value.SupportedFunction != func)
                throw new Exception(string.Format("Contract {0} is not supported for function {1}", value.GetType().Name, func));

            _contracts[func] = value;
        }

        public T GetContract<T>(EndPointFunctions func) where T : AbstractContract
        {
            if (!_contracts.ContainsKey(func))
                throw new Exception(string.Format("No contract exists for the type {0}", func));

            return _contracts[func] as T;
        }

        public string GetClientIdentifier()
        {
            if (string.IsNullOrWhiteSpace(_clientId))
            {
                _clientId = _clientIdentifier.GetIdentifier();
            }

            return _clientId;
        }

        public Guid GetSessionId()
        {
           return _sessionId;
        }
    }
}
