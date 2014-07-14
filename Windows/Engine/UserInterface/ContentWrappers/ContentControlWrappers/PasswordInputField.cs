using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class PasswordInputField : AbstractContentWrapper
    {
        public PasswordInputField(IEngine engine)
            : base(engine)
        {
        }

        public string Watermark { get; set; }

        public override System.Type GetSupportingModelType()
        {
            return typeof (PassphraseContentModel);
        }

        public override string GetDefaultStyle()
        {
            return "PasswordField";
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Watermark = XmlUtilities.GetTextFromAttribute(node, "watermark");
         
        }

       
    }
}
