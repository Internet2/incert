namespace Org.InCommon.InCert.DataContracts
{
    public class BaseAuthenticationQuery
    {
        public string UserId { get; set; }
        public string Credential1 { get; set; }
        public string Credential2 { get; set; }
        public string Credential3 { get; set; }
        public string Credential4 { get; set; }
        public string Provider { get; set; }
    }
}
