using System.Windows;
using Org.InCommon.InCert.DataContracts;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.WebServices;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.WebApi
{
    class MacAddressReport:AbstractMacAddressReportContract
    {
        public MacAddressReport(IEndpointManager endpointManager) : base(endpointManager)
        {
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.POST) { RequestFormat = RestSharp.DataFormat.Json };
            var wrapper = new MachineMacAddressesReport
                {
                Machine = new Machine { MachineId = Application.Current.GetIdentifier() },
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
