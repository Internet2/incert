using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers
{
    class SimpleBanner:AbstractBanner
    {
        public SimpleBanner(IEngine engine):base(engine)
        {
            Width = 400;
            Height = 400;
        }
    }
}
