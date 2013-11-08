using System;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers
{
    public abstract class AbstractLink : AbstractDynamicPropertyContainer
    {
        private AbstractTextContentWrapper _content;


        protected AbstractLink(IEngine engine) : base(engine)
        {
            
        }

        public virtual string GetText()
        {
            return _content == null ? "" : _content.GetText();
        }

        public string Target
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string Color
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public abstract Type GetPreferredModelType();

        public override bool Initialized()
        {
            if (_content == null)
                return false;

            return IsPropertySet("Target");
        }

        public override void ConfigureFromNode(XElement node)
        {
            _content = GetInstanceFromNode<AbstractTextContentWrapper>(node.Elements().FirstOrDefault());
            Target = XmlUtilities.GetTextFromAttribute(node, "target");
            Color = XmlUtilities.GetTextFromAttribute(node, "color");
        }
    }
}
