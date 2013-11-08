namespace Org.InCommon.InCert.DataContracts
{
    public class AuthenticatedIdentityQuery
    {
        public BaseAuthenticationQuery AuthenticationQuery { get; set; }
        public IdentityQuery IdentityQuery { get; set; }
    }
}
