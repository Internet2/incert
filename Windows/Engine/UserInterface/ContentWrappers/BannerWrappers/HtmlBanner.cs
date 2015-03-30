using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers
{
    public class HtmlBanner:AbstractBanner
    {
        public HtmlBanner(IEngine engine) : base(engine)
        {
        }

        public string Url { get; set; }
    }
}
