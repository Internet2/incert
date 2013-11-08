using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Logging;
using log4net;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors.Composite
{
    class MultiProcessor:AbstractCommandLineProcessor
    {
        private static readonly ILog Log = Logger.Create();

        private readonly List<ICommandLineProcessor> _processors;

        public MultiProcessor(IEngine engine):base(engine)
        {
            _processors = new List<ICommandLineProcessor>();
        }
        
        public override void ProcessCommandLine(string value)
        {
            try
            {
                if (!_processors.Any())
                    return;

                foreach (var processor in _processors.Where(processor => processor.IsInitialized()))
                    processor.ProcessCommandLine(value);
                
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to process the command-line value {0}: {1}", value, e.Message);        
            }
        }

        public override void ConfigureFromNode(XElement element)
        {
            base.ConfigureFromNode(element);

            var processorsNode = element.Element("Processors");
            if (processorsNode == null)
                return;

            if (!processorsNode.HasElements)
                return;
            
            foreach (var processor in processorsNode.Elements()
                .Select(GetInstanceFromNode<AbstractCommandLineProcessor>)
                .Where(processor => processor != null)
                .Where(processor => processor.IsInitialized()))
            {
                _processors.Add(processor);
            }
        }
    }
}
