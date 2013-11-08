using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class HiddenButton:AbstractButton
    {

        public HiddenButton(IEngine engine):base(engine)
        {
            Enabled = false;
            Visible = false;
        }

    }
}
