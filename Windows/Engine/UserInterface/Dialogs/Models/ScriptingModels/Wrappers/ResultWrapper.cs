using System.Collections.Generic;
using System.Runtime.Serialization;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Results;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.Wrappers
{
    [DataContract]
    public class ResultWrapper
    {
        [DataMember(Name="type", Order = 1)]
        public string ResultType { get; set; }

        [DataMember(Name = "isOk", Order = 2)]
        public bool IsOk { get; set; }

        [DataMember(Name = "details", Order = 3)]
        public Dictionary<string, string> Details { get; set; } 

        public ResultWrapper(IResult result)
        {
            if (result == null)
            {
                return;
            }
            
            Details = result.GetDetailsDictionary();
            ResultType = result.ToString();
            IsOk = result.IsOk();
        }
    }
}
