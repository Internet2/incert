using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Org.InCommon.InCert.Engine.Results;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.EventWrappers
{
    [DataContract]
    public class IssueEventData:AbstractEventData
    {
        public IssueEventData()
        {
            
        }

        public IssueEventData(IResult result)
        {
            
        }
    }
}
