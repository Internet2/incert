using System.Net;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.Certificates;
using Org.InCommon.InCert.Engine.WebServices.Managers;
using RestSharp;

namespace Org.InCommon.InCert.Engine.WebServices.Contracts.InCommon
{
    class GetCertificateRequest : AbstractGetCertificateContract
    {
    
        public GetCertificateRequest(IEndpointManager endpointManager)
            : base(endpointManager)
        {
        }

        protected override bool ErrorOccurred(IRestResponse result)
        {
            if (result.ResponseStatus != ResponseStatus.Completed)
                return true;

            if (result.StatusCode != HttpStatusCode.OK)
                return true;

            return false;
        }

        protected override IRestRequest GetRequestObject()
        {
            var request = new RestRequest(Method.GET)
                { RootElement = "UserCertificate" };

            request.AddParameter("Function", "GetCertificate");
            request.AddParameter("LoginId", LoginId);
            request.AddParameter("LoginPwd", Credential1);
            request.AddParameter("LoginCred2", Credential2);
            request.AddParameter("LoginCred3", Credential3);
            request.AddParameter("LoginCred4", Credential4);
            request.AddParameter("CaName", Provider);
            request.AddParameter("EncryptPkcs12", EncryptCertificate);
            return request;
        }

        public override IResult GetErrorResult()
        {
            return new CouldNotRetrieveCertificate { Issue = GetError().Message };
        }

        
    }
}
