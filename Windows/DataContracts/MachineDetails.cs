using System.Collections.Generic;

namespace Org.InCommon.InCert.DataContracts
{
    public class MachineDetails
    {
        public MachineDetails()
        {
            Sessions = new List<SessionText>();
            Users = new List<User>();
            EventEntries = new List<EventEntry>();
            Reports = new List<ReportingEntry>();
            MacAddresses = new List<MacAddress>();
            Registrations = new List<MacAddressRegistration>();
            IpAddresses = new List<string>();
        }
        
        public Machine Machine { get; set; }
        public List<SessionText> Sessions { get; set; }
        public List<User> Users { get; set; }
        public List<EventEntry> EventEntries { get; set; }
        public List<MacAddress> MacAddresses { get; set; }
        public List<MacAddressRegistration> Registrations { get; set; } 
        public List<ReportingEntry> Reports { get; set; }
        public List<string> IpAddresses { get; set; }
    }
}
