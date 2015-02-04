using System.Runtime.Serialization;
using Org.InCommon.InCert.Engine.Tasks;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers
{
    [DataContract]
    public class TaskEventData:AbstractEventData
    {
        public TaskEventData()
        {
            
        }

        public TaskEventData(ITask task)
        {
            Id = task.Id;
            Message = task.UiMessage;
        }
        
        [DataMember(Name = "id", Order = 1)]
        public string Id { get; set; }

        [DataMember(Name="message", Order=2)]
        public string Message { get; set; }
    }
}
