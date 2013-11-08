using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class FileBrowseButton:FramedButton
    {
        public string Filter { get; set; }
        public string Title { get; set; }
        
        public FileBrowseButton(IEngine engine) : base(engine)
        {
        }

        public override Type GetSupportingModelType()
        {
            return typeof (FileBrowseButtonModel);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Filter = XmlUtilities.GetTextFromAttribute(node, "filter");
            Title = XmlUtilities.GetTextFromAttribute(node, "title");
        }
    }
}
