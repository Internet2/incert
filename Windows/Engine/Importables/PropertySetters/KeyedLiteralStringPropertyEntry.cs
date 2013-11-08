using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.Importables.PropertySetters
{
    public class KeyedLiteralStringPropertyEntry : AbstractDynamicPropertyContainer
    {
        public KeyedLiteralStringPropertyEntry(IEngine engine) : base(engine)
        {
        }

        public string Key { get; set; }
        public string Value { get; set; }

        public override void ConfigureFromNode(System.Xml.Linq.XElement element)
        {
            Key = element.Attribute("key").Value;
            Value = element.Value;
        }
    }
}
