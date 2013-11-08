using System;

namespace Org.InCommon.InCert.DataContracts
{
    public class ReportingEntry
    {
        public long Id { get; set; }
        public virtual Machine Machine { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual Session Session { get; set; }
    }
}
