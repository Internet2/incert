using System;
using System.Linq;
using System.Reflection;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.Resources
{
    class EmbeddedResourceExists:AbstractCondition
    {
        public EmbeddedResourceExists(IEngine engine):base(engine)
        {
        }

        public string Key
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Key = XmlUtilities.GetTextFromAttribute(node, "key");
        }
        public override BooleanReason Evaluate()
        {
            var resourcePath = Assembly.GetExecutingAssembly().GetName().Name + "." + Key;
            return Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Any(resource => resource.Equals(resourcePath, StringComparison.InvariantCulture)) 
                ? new BooleanReason(true , string.Format("{0} exists as an embedded resource", Key)) 
                : new BooleanReason(false, string.Format("{0} does not exist as an embedded resource", Key));
        }

        public override bool IsInitialized()
        {
            return !string.IsNullOrWhiteSpace(Key);
        }
    }
}
