using System;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class OptionInputField:AbstractContentWrapper
    {
        public OptionInputField(IEngine engine) : base(engine)
        {
        }

        public string Group { get; set; }
       
        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            Group = XmlUtilities.GetTextFromAttribute(node, "group", "default group");
        }

        public override Type GetSupportingModelType()
        {
            return typeof(OptionParagraphContentModel);
        }
    }
}
