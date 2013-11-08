using System.Linq;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ImageWrappers;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{

    class LeftImageContentParagraph : AbstractContentWrapper
    {
        public LeftImageContentParagraph(IEngine engine)
            : base(engine)
        {
        }

        private static readonly ILog Log = Logger.Create();

        public string Banner { get; set; }

        public AbstractImageContent ImageContent { get; private set; }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {

            var contentNode = node.Element("Content");
            if (contentNode != null)
            {
                var instance = GetInstanceFromNode<AbstractImageContent>(contentNode.Elements().FirstOrDefault());
                if (instance != null && instance.Initialized())
                    ImageContent = instance;
            }

            Style = XmlUtilities.GetTextFromAttribute(node, "style");
            ControlKey = XmlUtilities.GetTextFromAttribute(node, "controlKey");
            Banner = XmlUtilities.GetTextFromAttribute(node, "banner");
        }

    
    }
}

