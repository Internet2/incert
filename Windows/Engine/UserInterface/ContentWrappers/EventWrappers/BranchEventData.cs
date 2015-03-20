using System.Runtime.Serialization;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.Wrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers
{
    [DataContract]
    public class BranchEventData:AbstractEventData
    {
        public BranchEventData()
        {
            
        }

        public BranchEventData(ITaskBranch branch, IResult result=null)
        {
            Name = branch.Name;
            Result = new ResultWrapper(result);
        }

        [DataMember(Name = "name", Order = 1)]
        public string Name { get; set; }

        [DataMember(Name = "message", Order = 2)]
        public string Message { get; set; }

        [DataMember(Name = "result", Order = 3)] 
        public ResultWrapper Result { get; private set; }
    }
}
