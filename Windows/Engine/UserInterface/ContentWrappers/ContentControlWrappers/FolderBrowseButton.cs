using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class FolderBrowseButton:FramedButton
    {
        public FolderBrowseButton(IEngine engine) : base(engine)
        {
        }

        public string Description { get; private set; }

        public override System.Type GetSupportingModelType()
        {
            return typeof (FolderBrowseButtonModel);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Description = XmlUtilities.GetTextFromAttribute(node, "description");
        }
    }
}
