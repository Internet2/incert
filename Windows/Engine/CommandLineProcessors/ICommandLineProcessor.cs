namespace Org.InCommon.InCert.Engine.CommandLineProcessors
{
    public interface ICommandLineProcessor
    {
        string GetProcessorKey();
        void ProcessCommandLine(string value);
        bool IsInitialized();
    }
}
