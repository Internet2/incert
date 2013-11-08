using System;
using System.Windows;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class BulletParagraph : AbstractContentWrapper
    {

        public double BulletSize { get; set; }

        public int Indent { get; set; }

        public BulletParagraph(
           IEngine engine)
            : base(engine)
        {
            Margin = new Thickness(0, 0, 8, 0);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Indent = XmlUtilities.GetIntegerFromAttribute(node, "indent", 12);
            BulletSize = XmlUtilities.GetDoubleFromAttribute(node, "bulletSize", 6);
        }

        public override Type GetSupportingModelType()
        {
            return typeof(BulletedTextModel);
        }
    }
}
