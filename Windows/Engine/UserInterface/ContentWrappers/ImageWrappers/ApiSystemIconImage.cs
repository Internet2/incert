using System.Windows.Controls;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ImageWrappers
{
    class ApiSystemIconImage:AbstractImageContent
    {
        public ApiSystemIconImage(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public UserInterfaceUtilities.StockIconIdentifier Icon { get; set; }

        [PropertyAllowedFromXml]
        public UserInterfaceUtilities.StockIconSize Size { get; set;}
        
        public override Image GetImage()
        {
            return UserInterfaceUtilities.GetSystemIconFromApi(Icon, Size, false, false);
        }
    }
}
