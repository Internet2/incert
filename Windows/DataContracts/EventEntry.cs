using System;

namespace Org.InCommon.InCert.DataContracts
{
    public class EventEntry
    {
        public long Id { get; set; }
        
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
      
        public virtual Session Session { get; set; }
        public virtual EventType EventType { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual User User { get; set; }
    }


}