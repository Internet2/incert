using System.Collections.Generic;
using System.Xml.Linq;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers
{
    public interface IBannerManager
    {
        void Initialize();
        bool ImportBanners(List<AbstractBanner> banners);
        bool ImportBannersFromXml(XElement node);
        AbstractBanner GetBanner(string value);
        AbstractBanner SetBanner(string key, AbstractBanner value);
        void AddHtmlRedirect(string key, string value);
        void RemoveHtmlRedirect(string key);
        Dictionary<string, string> GetHtmlRedirects();
    }
}
