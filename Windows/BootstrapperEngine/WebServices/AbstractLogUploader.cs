using System;
using RestSharp;

namespace Org.InCommon.InCert.BootstrapperEngine.WebServices
{
    abstract class AbstractLogUploader
    {
        public string EndpointUrl { get; set; }
        public Guid Session { get; set; }
        public string MachineId { get; set; }
        public string UserId { get; set; }
        public int Timeout { get; set; }

        internal abstract IRestRequest GetRequestObject(string eventId, string message);
        
        public void UploadLog(string eventId, string message)
        {
            var client = GetClient();
            var request = GetRequestObject(eventId, message);

            client.ExecuteAsync(request, ResponseHandler);
        }

        protected AbstractLogUploader()
        {
            Timeout = 30000;
        }


        private static void ResponseHandler(IRestResponse restResponse, RestRequestAsyncHandle restRequestAsyncHandle)
        {
            // ignore for now;
        }

        protected virtual RestClient GetClient()
        {
            var client = new RestClient(EndpointUrl) { Timeout = Timeout, };
            return client;
        }
    }
}
