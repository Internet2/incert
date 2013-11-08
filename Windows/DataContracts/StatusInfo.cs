namespace Org.InCommon.InCert.DataContracts
{
    public class StatusInfo
    {
        // ReSharper disable InconsistentNaming
        public string ClientIPV4Address { get; set; }
        // ReSharper restore InconsistentNaming
        public string GmtTime { get; set; }
        public string ServerVersion { get; set; }
        public string MinClientVersion { get; set; }
    }
}
