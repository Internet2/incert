using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class SimpleInputField : AbstractContentWrapper
    {
        public int MaxLines { get; set; }
        public int MinLines { get; set; }
        public bool ReadOnly { get; set; }
        public bool CanScroll { get; set; }
        public bool AlwaysScrollToEnd { get; set; }
        
        public SimpleInputField(IEngine engine)
            : base(engine)
        {
        }

        public override Type GetSupportingModelType()
        {
            return typeof(InputContentModel);
        }

        public override string GetDefaultStyle()
        {
            return "InputField";
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            MaxLines = XmlUtilities.GetIntegerFromAttribute(node, "maxLines", 1);
            MinLines = XmlUtilities.GetIntegerFromAttribute(node, "minLines", 1);
            ReadOnly = XmlUtilities.GetBooleanFromAttribute(node, "readOnly", false);
            CanScroll = XmlUtilities.GetBooleanFromAttribute(node, "canScroll", false);
            AlwaysScrollToEnd = XmlUtilities.GetBooleanFromAttribute(node, "scrollToEnd", false);

            if (MinLines > MaxLines)
                MaxLines = 0;
        }
    }
}
