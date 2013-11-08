using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class DisabledButton:AbstractButton
    {
        public DisabledButton(IEngine engine) : base(engine)
        {
            Enabled = false;
        }

       
      
    }
}
