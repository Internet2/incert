using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    public abstract class AbstractContentWrapper : AbstractImportable
    {
        private static readonly ILog Log = Logger.Create();

        private readonly List<AbstractLink> _links = new List<AbstractLink>();
        private readonly List<AbstractTextContentWrapper> _content = new List<AbstractTextContentWrapper>();

        public string Style { get; set; }
        public string ControlKey { get; set; }
        public string SettingKey { get; set; }
        public string Color { get; set; }

        public TextAlignment? Alignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public FontWeight? FontWeight { get; set; }
        public FontStyle? FontStyle { get; set; }
        public FontFamily FontFamily { get; set; }
        public double? FontSize { get; set; }
        public Thickness? Margin { get; set; }
        public Thickness? Padding { get; set; }
        public Dock Dock { get; set; }
        public double LineHeight { get; set; }
        public bool Enabled { get; set; }

        public virtual Type GetSupportingModelType()
        {
            return null;
        }

        protected AbstractContentWrapper(
            IEngine engine
            )
            : base(engine)
        {
        }

        public virtual string GetText()
        {
            if (_content == null)
                return "";

            if (!_content.Any())
                return "";

            if (_content.Count == 1)
                return _content[0].GetText();

            var builder = new StringBuilder();
            foreach (var instance in _content)
                builder.AppendLine(instance.GetText());

            return builder.ToString();
        }


        public virtual List<AbstractLink> GetLinks()
        {
            return _links;
        }

        private void ImportLinks(XContainer node)
        {
            var linkNode = node.Element("Links");
            if (linkNode == null)
                return;

            var linkNodes = linkNode.Elements().ToList();

            if (!linkNodes.Any())
                return;

            foreach (var value in linkNodes.Select(GetInstanceFromNode<AbstractLink>))
            {
                if (value == null || !value.Initialized())
                {
                    Log.Warn("could not instatiate link from xml");
                    continue;
                }
                _links.Add(value);
            }
        }

        public virtual string GetDefaultStyle()
        {
            return "Body";
        }

        public void AddContent(AbstractTextContentWrapper content)
        {
            _content.Add(content);
        }

        public override void ConfigureFromNode(XElement node)
        {
            var contentNode = node.Element("Content");
            if (contentNode != null)
            {
                foreach (var content in contentNode.Elements()
                    .Select(GetInstanceFromNode<AbstractTextContentWrapper>)
                    .Where(content => content != null)
                    .Where(content => ((IImportable)content).Initialized()))
                {
                    AddContent(content);
                }
            }

            Style = XmlUtilities.GetTextFromAttribute(node, "style", GetDefaultStyle());
            ControlKey = XmlUtilities.GetTextFromAttribute(node, "controlKey");
            SettingKey = XmlUtilities.GetTextFromAttribute(node, "settingKey");
            Color = XmlUtilities.GetTextFromAttribute(node, "color");
            Dock = XmlUtilities.GetEnumValueFromAttribute(node, "dock", Dock.Top);
            VerticalAlignment = XmlUtilities.GetEnumValueFromAttribute(node, "verticalAlignment", VerticalAlignment.Top);
            Enabled = XmlUtilities.GetBooleanFromAttribute(node, "enabled", true);

            if (XmlUtilities.IsAttributeSet(node, "alignment"))
                Alignment = XmlUtilities.GetEnumValueFromAttribute(node, "alignment", TextAlignment.Left);

            if (XmlUtilities.IsAttributeSet(node, "fontFamily"))
                FontFamily = XmlUtilities.ConvertFromAttributeUsingConverter<FontFamily>(
                    node, "fontFamily", new FontFamilyConverter(), null);

            if (XmlUtilities.IsAttributeSet(node, "fontSize"))
                FontSize = XmlUtilities.GetDoubleFromAttribute(node, "fontSize", 14);

            if (XmlUtilities.IsAttributeSet(node, "fontWeight"))
                FontWeight = XmlUtilities.ConvertFromAttributeUsingConverter(
                    node, "fontWeight", new FontWeightConverter(), FontWeights.Normal);

            if (XmlUtilities.IsAttributeSet(node, "fontStyle"))
                FontStyle = XmlUtilities.ConvertFromAttributeUsingConverter(
                    node, "fontStyle", new FontStyleConverter(), FontStyles.Normal);

            if (XmlUtilities.IsAttributeSet(node, "padding"))
                Padding = XmlUtilities.ConvertFromAttributeUsingConverter(
                    node, "padding", new ThicknessConverter(), new Thickness(0));

            if (XmlUtilities.IsAttributeSet(node, "margin"))
                Margin = XmlUtilities.ConvertFromAttributeUsingConverter(
                    node, "margin", new ThicknessConverter(), new Thickness(0));

            if (XmlUtilities.IsAttributeSet(node, "lineHeight"))
                LineHeight = XmlUtilities.ConvertFromAttributeUsingConverter(
                    node, "lineHeight", new LengthConverter(), 0);

            ImportLinks(node);
        }


        public static Inline GetMainTextFromContents(InlineCollection contents)
        {
            var contentDictionary = contents.ToDictionary(p => p.Name);
            return !contentDictionary.ContainsKey("MainText") ? null : contentDictionary["MainText"];
        }
    }
}
