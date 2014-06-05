using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    public abstract class AbstractButtonWrapper : AbstractDynamicPropertyContainer
    {
        [PropertyAllowedFromXml]
        public string Text
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        [PropertyAllowedFromXml]
        public bool Enabled { get; set; }

        [PropertyAllowedFromXml]
        public bool Visible { get; set; }

        [PropertyAllowedFromXml]
        public ButtonTargets Target { get; set; }

        [PropertyAllowedFromXml]
        public bool IsDefaultButton { get; set; }

        [PropertyAllowedFromXml]
        public bool IsCancelButton { get; set; }

        [PropertyAllowedFromXml]
        public string ImageKey { get; set; }

        [PropertyAllowedFromXml]
        public string MouseOverImageKey { get; set; }

        protected AbstractButtonWrapper(IEngine engine):base(engine)
        {
            Enabled = true;
            Visible = true;
        }

     
        public override void ConfigureFromNode(XElement node)
        {
            MapChildrenToProperties(node, this);

           
        }

        public override bool Initialized()
        {
            return (Target != ButtonTargets.None);
        }

      
      
    }
}
