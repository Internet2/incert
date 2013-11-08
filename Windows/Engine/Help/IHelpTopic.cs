namespace Org.InCommon.InCert.Engine.Help
{
    public interface IHelpTopic
    {
        string Identifier { get; }
        string Url { get; }
        bool IsValid { get; }
        bool IsInitialized { get; }

    }

}
