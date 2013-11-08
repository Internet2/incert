using System;

namespace Org.InCommon.InCert.DataContracts
{
    public class MacAddressRegistration
    {
        public long Id { get; set; }
        public virtual MachineMacAddress MachineMacAddress { get; set; }
        public virtual User User { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
