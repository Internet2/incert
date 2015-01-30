using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers
{

    public sealed class BannerManager : IBannerManager
    {
        private static readonly ILog Log = Logger.Create();

        private readonly Dictionary<String, AbstractBanner> _banners = new Dictionary<String, AbstractBanner>();

        public void Initialize()
        {
            try
            {

                _banners.Clear();

                var boostrapXml = XmlUtilities.LoadXmlFromAssembly("Org.InCommon.InCert.Engine.Content.Bootstrap.xml");
                if (boostrapXml == null)
                {
                    Log.Warn("could not load bootstrap.xml from assembly resource");
                    return;
                }

                ImportBannersFromXml(boostrapXml);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }

        public bool ImportBanners(List<AbstractBanner> banners)
        {
            foreach (var banner in banners)
            {
                _banners[banner.Name] = banner;
            }

            return true;
        }


        public bool ImportBannersFromXml(XElement node)
        {
            if (node == null)
            {
                Log.Warn("empty xml document passed to ImportBannersFromXml; cannot import task banners");
                return false;
            }

            var bannerNode = node.Element("Banners");
            if (bannerNode == null)
                return false;

            foreach (var banner in bannerNode.Elements().Select(AbstractImportable.GetInstanceFromNode<AbstractBanner>))
            {
                if (banner == null || !banner.Initialized())
                {
                    Log.Warn("could not instantiate banner from xml");
                    continue;
                }

                _banners[banner.Name] = banner;
            }


            return true;
        }

        public AbstractBanner GetBanner(string value)
        {
            return !_banners.ContainsKey(value) ? null : (_banners[value]);
        }

        public AbstractBanner SetBanner(string key, AbstractBanner value)
        {
            _banners[key] = value;
            return _banners[key];
        }
    }
}
