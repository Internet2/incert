using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.ClientIdentifier;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class MacAddressReport:AbstractMacAddressReportContract
    {
        private readonly IClientIdentifier _clientIdentifier;

        public MacAddressReport(IEndpointManager endpointManager, IClientIdentifier clientIdentifier) : base(endpointManager)
        {
            _clientIdentifier = clientIdentifier;
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) { RequestFormat = RestSharp.DataFormat.Json };
            var wrapper = new MachineMacAddressesReport
                {
                Machine = new Machine { MachineId = _clientIdentifier.GetIdentifier() },
                Addresses = Addresses
            };

            request.AddBody(wrapper);
            return request;
        }

        public override IResult GetErrorResult()
        {
            return new CouldNotUploadReport();
        }
    }
}
