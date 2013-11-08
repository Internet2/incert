using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class ErrorInfoBulletedContent : AbstractContentWrapper
    {
        
        public ErrorInfoBulletedContent(IEngine engine) :
            base(engine)
        {
        }
        
        public override string GetDefaultStyle()
        {
            return "Body";
        }
    }
}
