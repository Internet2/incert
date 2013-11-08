namespace Org.InCommon.InCert.DataContracts
{
    public class UserMachine
    {
        public long Id { get; set; }
        public virtual User User { get; set; }
        public virtual Machine Machine { get; set; }
    }

    
}