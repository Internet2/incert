using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class OpenAdvancedMenuButton:AbstractButton
    {
        public OpenAdvancedMenuButton(IEngine engine):base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Group { get; set; }
    }
}
