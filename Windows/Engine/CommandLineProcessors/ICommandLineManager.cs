using System.Xml.Linq;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors
{
    public interface ICommandLineManager
    {
        void Initialize();
        bool ImportProcessorsFromXml(XElement node);
        void ProcessCommandLines();
        void ProcessCommandLine(string value);
        void AddProcessorEntry(ICommandLineProcessor value);
        ICommandLineProcessor GetProcessor(string key);
    }
}