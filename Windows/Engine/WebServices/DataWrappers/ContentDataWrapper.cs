using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.AdvancedMenu;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Help;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Errors.Mapping;
using Org.InCommon.InCert.Engine.TaskBranches;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using log4net;

namespace Org.InCommon.InCert.Engine.WebServices.DataWrappers
{
    public class ContentDataWrapper:AbstractImportable
    {
        private static readonly ILog Log = Logger.Create();

        public ContentDataWrapper():base(null){}

        public ContentDataWrapper(IEngine engine) : base(engine)
        {
        }

        public List<AbstractBranch> Branches { get; private set; }
        public List<AbstractBanner> Banners { get; private set; }
        public List<AbstractErrorEntry> Errors { get; private set; }
        public List<HelpTopic> Topics { get; set; }
        public List<AdvancedMenuItem> AdvancedMenuItems { get; private set; }

        public override void ConfigureFromNode(XElement node)
        {
            if (node == null)
            {
                Log.Warn("empty xml document passed to ConfigureFromNode; cannot import content");
                return;
            }

            Branches = ImportContent<AbstractBranch>(node.Element("Branches"));
            Banners = ImportContent<AbstractBanner>(node.Element("Banners"));
            Errors = ImportContent<AbstractErrorEntry>(node.Element("Errors"));
            Topics = ImportContent<HelpTopic>(node.Element("HelpTopics"));
            AdvancedMenuItems = ImportContent<AdvancedMenuItem>(node.Element("AdvancedMenuItems"));

        }

        private static List<T> ImportContent<T>(XContainer node ) where T : class, IImportable
        {
            if (node == null)
                return new List<T>();

            var nodes = node.Elements().ToList();
            if (!nodes.Any())
                return new List<T>();

            return nodes.Select(GetInstanceFromNode<T>).Where(instance => instance != null && instance.Initialized()).ToList();
        }
    }
}
