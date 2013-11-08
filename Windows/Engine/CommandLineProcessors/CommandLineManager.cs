using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Ninject;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.CommandLineProcessors
{
    public class CommandLineManager : ICommandLineManager
    {
        private static readonly ILog Log = Logger.Create();

        private readonly Dictionary<string, ICommandLineProcessor> _processors = new Dictionary<string, ICommandLineProcessor>();
        
        public void Initialize()
        {
            try
            {

                _processors.Clear();

                var processorsXml = XmlUtilities.LoadXmlFromAssembly("Org.InCommon.InCert.Engine.Content.CommandLineProcessors.xml");
                if (processorsXml == null)
                {
                    Log.Warn("Could not load CommandLineProcessors.xml from assembly resource");
                    return;
                }

                ImportProcessorsFromXml(processorsXml);

            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        public bool ImportProcessorsFromXml(XElement node)
        {
            if (node == null)
            {
                Log.Warn("empty xml document passed to ImportBranchesFromXml; cannot import command-line processors");
                return false;
            }

            var processorsNode = node.Element("CommandLineProcessors");
            if (processorsNode == null)
                return false;

            foreach (var processorNode in processorsNode.Elements())
            {
                var processor = AbstractImportable.GetInstanceFromNode<AbstractCommandLineProcessor>(processorNode);
                if (processor == null || !processor.Initialized())
                    continue;

                _processors[processor.GetProcessorKey()] = processor;
            }

            return true;
        }

        public void ProcessCommandLines()
        {
            var commands = Environment.GetCommandLineArgs();

            // the first element will always be the execuatable file, so ignore
            if (commands.LongLength <= 1)
                return;

            for (var index = 1; index < commands.LongLength; index++)
                ProcessCommandLine(commands[index]);
            
        }

        public void ProcessCommandLine(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                if (!value.StartsWith("-", StringComparison.InvariantCulture))
                {
                    Log.WarnFormat("Cannot process command-line {0}: invalid command string", value);
                    return;
                }

                var key = value.Remove(0, 1);
                var match = Regex.Match(value, "-(.*?)=(.*)");
                key = GetKeyFromMatch(match, key);
                if (string.IsNullOrWhiteSpace(key))
                {
                    Log.WarnFormat("Cannot process command-line {0}: invalid key {1}", value, key);
                    return;
                }

                if (!_processors.ContainsKey(key))
                {
                    Log.WarnFormat("Cannot process command-line {0}: invalid key {1}", value, key);
                    return;
                }

                var parameter = GetParameterFromMatchGroup(match);
                _processors[key].ProcessCommandLine(parameter);

            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to parse command-line argument {0}: {1}", value, e);
            }

        }

        private static string GetKeyFromMatch(Match match, string defaultValue)
        {
            if (!match.Success)
                return defaultValue;

            if (match.Groups.Count < 2)
                return defaultValue;

            return string.IsNullOrWhiteSpace(match.Groups[1].Value) ? defaultValue : match.Groups[1].Value;
        }

        private static string GetParameterFromMatchGroup(Match match)
        {
            if (!match.Success)
                return "";

            if (match.Groups.Count < 3)
                return "";

            var result = match.Groups[2].Value;
            if (string.IsNullOrWhiteSpace(result))
                return result;

            result = result.Replace("\"", "");
            result = result.Replace("'", "");

            return result;
        }

        public void AddProcessorEntry(ICommandLineProcessor value)
        {
            if (!value.IsInitialized())
                return;

            _processors[value.GetProcessorKey()] = value;
        }

        public ICommandLineProcessor GetProcessor(string key)
        {
            return !_processors.ContainsKey(key) ? null : _processors[key];
        }
    }
}
