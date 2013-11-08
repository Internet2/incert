using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers
{
    public abstract class AbstractTextContentWrapper : AbstractDynamicPropertyContainer
    {
        protected AbstractTextContentWrapper(IEngine engine) : base(engine)
        {
        }

        public abstract string GetText();

        [PropertyAllowedFromXml]
        public string BaseText
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override void ConfigureFromNode(XElement node)
        {
            BaseText = XmlUtilities.GetTextFromNode(node);
        }
    }
}
