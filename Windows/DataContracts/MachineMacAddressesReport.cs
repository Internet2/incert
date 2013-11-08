using System.Collections.Generic;

namespace Org.InCommon.InCert.DataContracts
{
    public class MachineMacAddressesReport
    {
        public Machine Machine { get; set; }
        public List<MacAddress> Addresses { get; set; }
    }
}
