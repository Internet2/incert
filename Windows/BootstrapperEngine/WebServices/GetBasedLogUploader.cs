using System;
using RestSharp;

namespace Org.InCommon.InCert.BootstrapperEngine.WebServices
{
    class GetBasedLogUploader:AbstractLogUploader
    {
        internal override IRestRequest GetRequestObject(string eventId, string message)
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("Function", "LogMessage");
            request.AddParameter("Message", RenderMessage(eventId, message));
            return request;
        }

        private string RenderMessage(string eventId, string message)
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", DateTime.UtcNow, UserId, Session, MachineId, eventId,message);
        }
    }
}
