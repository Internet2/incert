using System.Collections.Generic;

namespace Org.InCommon.InCert.DataContracts
{
    public class IdentityQuery
    {
        public List<string> Properties { get; set; }
        public List<string> GroupPaths { get; set; }
        public List<string> Preferences { get; set; } 

        public static IdentityQuery Empty()
        {
            return new IdentityQuery
                {
                    Properties = new List<string>(),
                    GroupPaths = new List<string>(),
                    Preferences = new List<string>()
                };
        }
    }
}
