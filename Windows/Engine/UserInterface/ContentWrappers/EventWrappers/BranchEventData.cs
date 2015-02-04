using System.Runtime.Serialization;
using Org.InCommon.InCert.Engine.TaskBranches;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers
{
    [DataContract]
    public class BranchEventData:AbstractEventData
    {
        public BranchEventData()
        {
            
        }

        public BranchEventData(ITaskBranch branch)
        {
            Name = branch.Name;
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "message", Order = 1)]
        public string Message { get; set; }
    }
}
