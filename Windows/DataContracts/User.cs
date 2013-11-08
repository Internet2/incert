namespace Org.InCommon.InCert.DataContracts
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public virtual Campus Campus { get; set; }
        
    }
}