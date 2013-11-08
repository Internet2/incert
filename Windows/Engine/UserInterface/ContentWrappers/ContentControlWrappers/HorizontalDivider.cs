using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class HorizontalDivider : AbstractContentWrapper
    {
        public HorizontalDivider(IEngine engine)
            : base(engine)
        {
            
        }

        public int Height { get; private set; }
    
        public override System.Type GetSupportingModelType()
        {
            return typeof(HorizontalDividerModel);
        }
        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Height = XmlUtilities.GetIntegerFromAttribute(node, "height", 2);
            Color = XmlUtilities.GetTextFromAttribute(node, "color");
        }
    }
}
