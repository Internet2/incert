using Org.InCommon.InCert.DataContracts;
using RestSharp;

namespace Org.InCommon.InCert.BootstrapperEngine.WebServices
{
    class PostBasedLogUploader:AbstractLogUploader
    {
        internal override IRestRequest GetRequestObject(string eventId, string message)
        {
            var entry = new EventEntry
            {
                EventType = new EventType { Name = eventId },
                Machine = new Machine { MachineId = MachineId },
                User = new User { Username = UserId },
                Session = new Session { SessionGuid = Session },
                Text = message

            };

            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(entry);

            return request;
        }

        

       

        
    }
}
