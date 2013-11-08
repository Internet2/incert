using System.Windows;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Settings;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    internal class FramedButton : AbstractContentWrapper
    {
        public FramedButton(IEngine engine)
            : base(engine)
        {
        }

        public double Width { get; set; }
        public double Height { get; set; }
        public bool GlowEffect { get; set; }
        public string GlowColor { get; set; }
        public bool IsDefaultButton { get; set; }
        public bool IsCancelButton { get; set; }
        public Thickness BorderSize { get; set; }
        public string Background { get; set; }
        public string Value { get; set; }

        public override System.Type GetSupportingModelType()
        {
            return typeof (FramedButtonModel);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Width = XmlUtilities.GetDoubleFromAttribute(node, "width", 150);
            Height = XmlUtilities.GetDoubleFromAttribute(node, "height", 150);
            GlowEffect = XmlUtilities.GetBooleanFromAttribute(node, "glowEffect", true);
            GlowColor = XmlUtilities.GetTextFromAttribute(node, "glowColor");
            IsDefaultButton = XmlUtilities.GetBooleanFromAttribute(node, "defaultButton", false);
            IsCancelButton = XmlUtilities.GetBooleanFromAttribute(node, "cancelButton", false);
            BorderSize = XmlUtilities.ConvertFromAttributeUsingConverter(node, "borderSize", new ThicknessConverter(),
                                                                         new Thickness(3));
            Background = XmlUtilities.GetTextFromAttribute(node, "background");
            Value = XmlUtilities.GetTextFromAttribute(node, "value", "True");

            var caption = XmlUtilities.GetTextFromAttribute(node, "caption");
            if (!string.IsNullOrWhiteSpace(caption))
                AddContent(new DirectTextContent(Engine){BaseText = caption});
        }
    }
}
