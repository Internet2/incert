namespace Org.InCommon.InCert.DataContracts
{
    public class UserMachineSession
    {
        public long Id { get; set; }
        public virtual UserMachine UserMachine { get; set; }
        public virtual Session Session { get; set; }
    }
}
