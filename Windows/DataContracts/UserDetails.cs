using System.Collections.Generic;

namespace Org.InCommon.InCert.DataContracts
{
    public class UserDetails
    {
        public UserDetails()
        {
            Sessions = new List<SessionText>();
            Machines = new List<Machine>();
            EventEntries = new List<EventEntry>();
        }
        
        public User User { get; set; }
        public List<SessionText> Sessions { get; set; }
        public List<Machine> Machines { get; set; }
        public List<EventEntry> EventEntries { get; set; } 
    }
}
