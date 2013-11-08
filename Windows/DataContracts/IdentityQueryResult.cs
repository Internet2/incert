using System.Collections.Generic;

namespace Org.InCommon.InCert.DataContracts
{
    public class IdentityQueryResult
    {
        public Dictionary<string, string> Properties { get; set; }
        public List<string> Groups { get; set; }
        public Dictionary<string, string> Preferences { get; set; }

        public static IdentityQueryResult Empty()
        {
            return new IdentityQueryResult
                {
                    Properties = new Dictionary<string, string>(),
                    Groups = new List<string>(),
                    Preferences = new Dictionary<string, string>()
                };
        }
    }
}
