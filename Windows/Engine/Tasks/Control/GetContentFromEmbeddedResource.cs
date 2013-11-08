using System.Collections.Generic;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.Tasks.Control
{
    class GetContentFromEmbeddedResource:AbstractTask
    {
        private static readonly ILog Log = Logger.Create();
        private readonly List<string> _resourcePaths = new List<string>();
        
        [PropertyAllowedFromXml]
        public string ResourcePath {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Log.Warn("Cannot add null or empty key to list of resources to be loaded");
                    return;
                }
                _resourcePaths.Add(value);
            }
        }

        public GetContentFromEmbeddedResource(IEngine engine) :
            base(engine)
        {
            
        }
        
        public override IResult Execute(IResult previousResults)
        {
            foreach (var path in _resourcePaths)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Log.Warn("Cannot import content from embedded resource; content key is null or empty");
                    continue;
                }

                var content = XmlUtilities.LoadXmlFromAssembly(path);
                if (content == null)
                {
                    Log.WarnFormat("Could not load {0} from resources", path);
                    continue;
                }

                BranchManager.ImportBranchesFromXml(content);
                BannerManager.ImportBannersFromXml(content);
                ErrorManager.ImportEntriesFromXml(content);
                CommandLineManager.ImportProcessorsFromXml(content);
                HelpManager.ImportTopicsFromXml(content);
                AdvancedMenuManager.ImportItemsFromXml(content);
            }
            
            return new NextResult();
        }

        public override string GetFriendlyName()
        {
            return "Get content from embedded resources";
        }
    }
}
