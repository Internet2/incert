namespace Org.InCommon.InCert.DataContracts
{
    public class MachineMacAddress
    {
        public long Id { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual MacAddress Address { get; set; }
    }
}
