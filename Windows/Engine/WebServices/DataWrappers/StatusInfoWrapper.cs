namespace Engine.WebServices.DataWrappers
{
    public class StatusInfoWrapper
    {
// ReSharper disable InconsistentNaming
        public string ClientIPV4Address { get; set; }
// ReSharper restore InconsistentNaming
        public string GmtTime { get; set; }
        public double ServerVersion { get; set; }
        public double MinClientVersion { get; set; }
    }
}
