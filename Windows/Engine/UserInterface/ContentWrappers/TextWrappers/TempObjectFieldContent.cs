using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers
{
    public class TempObjectFieldContent : AbstractTextContentWrapper
    {
        [PropertyAllowedFromXml]
        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public TempObjectFieldContent(IEngine engine)
            : base(engine)
        {
        }

        public override string GetText()
        {
            return ReflectionUtilities.GetObjectPropertyText(
                SettingsManager.GetTemporaryObject(Key),
                BaseText);
        }

        public override bool Initialized()
        {
            if (string.IsNullOrWhiteSpace(BaseText))
                return false;

            if (!IsPropertySet("Key"))
                return false;

            return true;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
            BaseText = node.Value;
        }
    }
}
