using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Results.Errors.Mapping
{
    class ErrorManager : IErrorManager
    {
        private static readonly ILog Log = Logger.Create();
        
        private readonly Dictionary<String, AbstractErrorEntry> _errorMap =
            new Dictionary<string, AbstractErrorEntry>();

        public bool ImportEntriesFromXml(XElement node)
        {
            if (node == null)
            {
                Log.Warn("Cannot import entries to error map from xml; source node is null.");
                return false;
            }

            var entryNodes = node.Elements("Errors").Elements();
            foreach (var entry in from entryNode in entryNodes select AbstractImportable.GetInstanceFromNode<AbstractErrorEntry>(entryNode) into entry where entry != null where entry.Initialized() select entry)
                _errorMap[entry.Key] = entry;
            
            return true;
        }

        public bool ImportErrorEntries(List<AbstractErrorEntry> entries)
        {
            if (entries == null)
            {
                Log.Warn("Cannot import entries to error map from xml; source collection is null.");
                return false;
            }
            
            foreach (var entry in entries)
            {
                _errorMap[entry.Key] = entry;
            }
            return true;
        }


        public AbstractErrorEntry GetErrorEntry(ErrorResult issue)
        {
            try
            {
                if (issue == null)
                    return null;

                var key = issue.GetType().ToString().Replace("Org.InCommon.InCert.Engine.Results.", "");
                return !_errorMap.ContainsKey(key) ? new DefaultErrorEntry(issue) : _errorMap[key];
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new DefaultErrorEntry(issue);
            }
        }

        public void Initialize()
        {
            try
            {
                _errorMap.Clear();

                var errorXml = XmlUtilities.LoadXmlFromAssembly("Org.InCommon.InCert.Engine.Content.Errors.xml");
                if (errorXml == null)
                {
                    Log.Warn("could not load Errors.xml from assembly resource");
                    return;
                }

                ImportEntriesFromXml(errorXml);

            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

    }
}
