using System.Windows;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class ContentBlockParagraph : AbstractContentWrapper
    {
        
        public string Banner { get; set; }
        public Thickness BorderSize { get; set; }
        public string BorderColor { get; set; }
        public CornerRadius CornerRadius { get; set; }
    
        public ContentBlockParagraph(IEngine engine)
            : base(engine)
        {
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Banner = XmlUtilities.GetTextFromAttribute(node, "banner");
            BorderSize = XmlUtilities.ConvertFromAttributeUsingConverter(
                node, "borderSize", new ThicknessConverter(), new Thickness(0));
            BorderColor = XmlUtilities.GetTextFromAttribute(node, "borderColor");
            CornerRadius = XmlUtilities.ConvertFromAttributeUsingConverter(
                node, "cornerRadius", new CornerRadiusConverter(), new CornerRadius(0));
        }

        public override System.Type GetSupportingModelType()
        {
            return typeof(BorderedPanelModel);
        }

        public override string GetDefaultStyle()
        {
            return "ContentBorder";
        }
    }
}
