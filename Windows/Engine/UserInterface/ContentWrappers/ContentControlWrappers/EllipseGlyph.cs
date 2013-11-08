using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class EllipseGlyph:AbstractContentWrapper
    {
        public EllipseGlyph(IEngine engine) : base(engine)
        {
        }

        public int BorderSize { get; set; }
        public string Glyph { get; set; }

        public override Type GetSupportingModelType()
        {
            return typeof(EllipseGlyphWrapper);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            BorderSize = XmlUtilities.GetIntegerFromAttribute(node, "borderSize", 4);
            Glyph = XmlUtilities.GetTextFromAttribute(node, "glyph");
        }
    }
}
