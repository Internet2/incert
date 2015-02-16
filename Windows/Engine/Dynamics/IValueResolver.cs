namespace Org.InCommon.InCert.Engine.Dynamics
{
    public interface IValueResolver
    {
        string Resolve(string value, bool resolveSystemTokens);
    }
}
