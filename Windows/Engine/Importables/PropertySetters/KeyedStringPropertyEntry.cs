using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Importables.PropertySetters
{
    public class KeyedDynamicStringPropertyEntry : AbstractDynamicPropertyContainer
    {
        public KeyedDynamicStringPropertyEntry(IEngine engine)
            : base(engine)
        {
        }

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string Value
        {
            get
            {
                return !Resolve ? GetRawValue() : GetDynamicValue();
            }
            set { SetDynamicValue(value); }
        }

        public bool Resolve { get; set; }

        public override void ConfigureFromNode(System.Xml.Linq.XElement element)
        {
            Key = XmlUtilities.GetTextFromAttribute(element, "key");
            Resolve = XmlUtilities.GetBooleanFromAttribute(element, "resolve", true);
            Value = element.Value;
        }


    }
}
