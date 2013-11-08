using System.Windows.Controls;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ImageWrappers
{
    public abstract  class AbstractImageContent:AbstractImportable
    {
        protected AbstractImageContent(IEngine engine) : base(engine)
        {
        }

        public abstract Image GetImage();
        public object Target { get; set; }
        public override void ConfigureFromNode(XElement node)
        {
            MapChildrenToProperties(node, this);
        }
    }
}
