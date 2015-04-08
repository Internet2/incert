using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers
{
    public class HtmlBanner:AbstractBanner
    {
        public HtmlBanner(IEngine engine) : base(engine)
        {
        }

        public string Url { get; set; }
        public LinkPolicy LinkPolicy { get; set; }
    }
}
