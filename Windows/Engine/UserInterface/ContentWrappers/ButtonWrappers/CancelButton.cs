using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class CancelButton:AbstractButton
    {
        public CancelButton(IEngine engine):base(engine)
        {
            IsCancelButton = true;
        }
        
        public override bool Initialized()
        {
            return true;
        }
    }
}
