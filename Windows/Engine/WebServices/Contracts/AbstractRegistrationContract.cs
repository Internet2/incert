using System.Collections.Generic;
using System.Net;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts
{
    abstract class AbstractRegistrationContract:AbstractAuthenticationContract
    {
        protected AbstractRegistrationContract(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        public List<MacAddress> WirelessAddresses { get; set; }
        public List<MacAddress> WiredAddresses { get; set; }

        protected override T ProcessResults<T>(T result)
        {
            return result ?? new T();
        }

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
                return true;

            if (result.StatusCode != HttpStatusCode.NoContent)
                return true;

            return false;
        }


        public override EndPointFunctions SupportedFunction
        {
            get { return EndPointFunctions.RegisterComputer; }
        }
    }
}
