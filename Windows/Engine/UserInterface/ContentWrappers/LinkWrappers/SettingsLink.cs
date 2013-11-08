using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers
{
    class SettingsLink:AbstractLink
    {
        public SettingsLink(IEngine engine) : base(engine)
        {
        }

        public string Value
        {
            get { return GetDynamicValue(); } 
            set {SetDynamicValue(value);}
        }
        
        public override Type GetPreferredModelType()
        {
            return typeof(SettingsValueHyperlinkModel);
        }

        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);
            Value = XmlUtilities.GetTextFromAttribute(node, "value", "True");
        }
    }
}
