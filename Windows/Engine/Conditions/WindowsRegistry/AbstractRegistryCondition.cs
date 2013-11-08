using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.WindowsRegistry
{
    abstract class AbstractRegistryCondition:AbstractCondition
    {
        protected AbstractRegistryCondition(IEngine engine):base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public RegistryUtilities.RegistryRootValues RegistryRoot { get; set; }

        [PropertyAllowedFromXml]
        public string RegistryKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ValueName
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override bool IsInitialized()
        {
            if (!IsPropertySet("RegistryKey"))
                return false;

            if (!IsPropertySet("ValueName"))
                return false;

            if (RegistryRoot == RegistryUtilities.RegistryRootValues.Unknown)
                return false;

            return true;
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            RegistryRoot = XmlUtilities.GetEnumValueFromAttribute(node, "root",
                                                                  RegistryUtilities.RegistryRootValues.LocalMachine);
            RegistryKey = XmlUtilities.GetTextFromAttribute(node, "keyPath");
            ValueName = XmlUtilities.GetTextFromAttribute(node, "valueName");
        }
    }
}
