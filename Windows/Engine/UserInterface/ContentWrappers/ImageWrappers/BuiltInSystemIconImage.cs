using System.Windows.Controls;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ImageWrappers
{
    internal class BuiltInSystemIconImage : AbstractImageContent
    {
        public BuiltInSystemIconImage(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Icon { get; set; }

        public override Image GetImage()
        {
            return UserInterfaceUtilities.GetSystemIcon(Icon);
        }
    }
}
