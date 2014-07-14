using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class HiddenButton:AbstractButtonWrapper
    {

        public HiddenButton(IEngine engine):base(engine)
        {
            Enabled = false;
            Visible = false;
        }

    }
}
