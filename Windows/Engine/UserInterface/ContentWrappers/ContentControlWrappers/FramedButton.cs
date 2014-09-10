using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
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

        public string BorderColor { get;set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public bool GlowEffect { get; set; }
        public string GlowColor { get; set; }
        public bool IsDefaultButton { get; set; }
        public bool IsCancelButton { get; set; }
        public Thickness BorderSize { get; set; }
        public string Background { get; set; }
        public string Value { get; set; }
        public ButtonImageContent Image { get; set; }
        public ButtonTextContent Caption { get; set; }
        public ButtonTextContent SubCaption { get; set; }
        public string EffectName { get; set; }
        public string EffectArgument { get; set; }
        public CornerRadius CornerRadius { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
       
        public override System.Type GetSupportingModelType()
        {
            return typeof(FramedButtonModel);
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            HorizontalAlignment = XmlUtilities.GetEnumValueFromAttribute(node, "alignment", HorizontalAlignment.Left);
            Width = XmlUtilities.GetDoubleFromAttribute(node, "width", 150);
            Height = XmlUtilities.GetDoubleFromAttribute(node, "height", 150);
            GlowEffect = XmlUtilities.GetBooleanFromAttribute(node, "glowEffect", true);
            GlowColor = XmlUtilities.GetTextFromAttribute(node, "glowColor");
            IsDefaultButton = XmlUtilities.GetBooleanFromAttribute(node, "defaultButton", false);
            IsCancelButton = XmlUtilities.GetBooleanFromAttribute(node, "cancelButton", false);
            BorderSize = XmlUtilities.ConvertFromAttributeUsingConverter(node, "borderSize", new ThicknessConverter(),
                                                                         new Thickness(3));

            BorderColor = XmlUtilities.GetTextFromAttribute(node, "borderColor");
            Background = XmlUtilities.GetTextFromAttribute(node, "background");
            Value = XmlUtilities.GetTextFromAttribute(node, "value", "True");
            Image = GetInstanceFromNode<ButtonImageContent>(node.Element("Image"));
            Caption = GetInstanceFromNode<ButtonTextContent>(node.Element("Caption"));
            SubCaption = GetInstanceFromNode<ButtonTextContent>(node.Element("SubCaption"));
            EffectName = XmlUtilities.GetTextFromChildNodeAttribute(node, "Effect", "type");
            EffectArgument = XmlUtilities.GetTextFromChildNode(node, "Effect","");
            CornerRadius = XmlUtilities.ConvertFromAttributeUsingConverter(node, "cornerRadius",
                new CornerRadiusConverter(), new CornerRadius(0));
        }


        public class ButtonImageContent : AbstractDynamicPropertyContainer
        {
            public ButtonImageContent(IEngine engine)
                : base(engine)
            {

            }

            public VerticalAlignment VerticalAlignment { get; set; }
            public HorizontalAlignment Alignment { get; set; }
            public Thickness Margin { get; set; }


            public string Key
            {
                get { return GetDynamicValue(); }
                set { SetDynamicValue(value); }
            }

            public string MouseOverKey
            {
                get { return GetDynamicValue(); }
                set { SetDynamicValue(value); }
            }

            public override void ConfigureFromNode(XElement element)
            {
                base.ConfigureFromNode(element);

                VerticalAlignment = XmlUtilities.GetEnumValueFromAttribute(element, "verticalAlignment", VerticalAlignment.Center);
                Alignment = XmlUtilities.GetEnumValueFromAttribute(element, "alignment", HorizontalAlignment.Center);

                if (XmlUtilities.IsAttributeSet(element, "margin"))
                    Margin = XmlUtilities.ConvertFromAttributeUsingConverter(
                        element, "margin", new ThicknessConverter(), new Thickness(0));

                Key = XmlUtilities.GetTextFromAttribute(element, "key");
                MouseOverKey = XmlUtilities.GetTextFromAttribute(element, "mouseOverKey");
            }
        }

        
        public class ButtonTextContent : AbstractDynamicPropertyContainer
        {
            public ButtonTextContent(IEngine engine)
                : base(engine)
            {
            }

            public FontWeight FontWeight { get; set; }
            public FontStyle FontStyle { get; set; }
            public FontFamily FontFamily { get; set; }
            public double FontSize { get; set; }
            public VerticalAlignment VerticalAlignment { get; set; }
            public HorizontalAlignment HorizontalAlignment { get; set; }
            public Thickness Margin { get; set; }
            public Thickness Padding { get; set; }


            public string Value
            {
                get { return GetDynamicValue(); }
                set { SetDynamicValue(value); }
            }

            public override void ConfigureFromNode(XElement element)
            {
                base.ConfigureFromNode(element);

                VerticalAlignment = XmlUtilities.GetEnumValueFromAttribute(element, "verticalAlignment", VerticalAlignment.Center);
                HorizontalAlignment = XmlUtilities.GetEnumValueFromAttribute(element, "alignment", HorizontalAlignment.Center);

                FontFamily = XmlUtilities.ConvertFromAttributeUsingConverter<FontFamily>
                    (element, "fontFamily", new FontFamilyConverter(), null);

                FontSize = XmlUtilities.GetDoubleFromAttribute(element, "fontSize", 14);

                FontWeight = XmlUtilities.ConvertFromAttributeUsingConverter(
                    element, "fontWeight", new FontWeightConverter(), FontWeights.Normal);

                FontStyle = XmlUtilities.ConvertFromAttributeUsingConverter(
                        element, "fontStyle", new FontStyleConverter(), FontStyles.Normal);

                Margin = XmlUtilities.ConvertFromAttributeUsingConverter(
                        element, "margin", new ThicknessConverter(), new Thickness(0));

                Padding = XmlUtilities.ConvertFromAttributeUsingConverter(
                        element, "padding", new ThicknessConverter(), new Thickness(0));

                Value = XmlUtilities.GetTextFromNode(element);
            }
        }
    }
}

