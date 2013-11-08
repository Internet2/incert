using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class SystemIconParagraph:AbstractContentWrapper
    {
        public UserInterfaceUtilities.StockIconIdentifier Icon { get; set; }
        public UserInterfaceUtilities.StockIconSize Size { get; set; }

        public SystemIconParagraph(IEngine engine)
            : base(engine)
        {
            
        }

        

        public override System.Type GetSupportingModelType()
        {
            return typeof (ImageContentModel);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Icon = XmlUtilities.GetEnumValueFromAttribute(node, "icon", UserInterfaceUtilities.StockIconIdentifier.Error);
            Size = XmlUtilities.GetEnumValueFromAttribute(node, "size", UserInterfaceUtilities.StockIconSize.ShellSize);

            
        }

        
    }
}
