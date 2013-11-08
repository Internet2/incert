using System.Collections.Generic;

namespace Org.InCommon.InCert.DataContracts
{
    public class RegistrationRequest
    {
        public Machine Machine { get; set; }
        public BaseAuthenticationQuery AuthenticationQuery { get; set; }
        public List<MacAddress> WiredAddresses { get; set; }
        public List<MacAddress> WirelessAddresses { get; set; }
        public string Session { get; set; }
    }
}
