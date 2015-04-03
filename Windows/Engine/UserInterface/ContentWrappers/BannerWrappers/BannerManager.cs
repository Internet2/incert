using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using CefSharp;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels.SchemeHandlers;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers
{

    public sealed class BannerManager : IBannerManager
    {
        private static readonly ILog Log = Logger.Create();

        private readonly Dictionary<string, AbstractBanner> _banners = new Dictionary<String, AbstractBanner>();
        private readonly Dictionary<string, string> _htmlRedirects = new Dictionary<string, string>(); 

        public void Initialize()
        {
            try
            {
                InitializeChromium();

                _banners.Clear();

                var boostrapXml = XmlUtilities.LoadXmlFromAssembly("Org.InCommon.InCert.Engine.Content.Bootstrap.xml");
                if (boostrapXml == null)
                {
                    Log.Warn("could not load bootstrap.xml from assembly resource");
                    return;
                }

                ImportBannersFromXml(boostrapXml);
                InitializeRedirectDictionary(_htmlRedirects);
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

        public void AddHtmlRedirect(string key, string value)
        {
            _htmlRedirects[key] = value;
        }

        public void RemoveHtmlRedirect(string key)
        {
            if (!_htmlRedirects.ContainsKey(key))
            {
                Log.WarnFormat("Cannot remove redirect for key {0}: redirect not found", key);
                return;
            }

            _htmlRedirects.Remove(key);
        }

        public Dictionary<string, string> GetHtmlRedirects()
        {
            return _htmlRedirects;
        }

        private static void InitializeChromium()
        {
            if (Cef.IsInitialized)
            {
                return;
            }
            
            var settings = new CefSettings
            {
                PackLoadingDisabled =false,
                LogSeverity = LogSeverity.Disable
                    
            };

            settings.CefCommandLineArgs.Add(new KeyValuePair<string, string>("no-proxy-server", ""));
            
            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = ArchiveSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new ArchiveSchemeHandlerFactory(),

            });

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = EmbeddedResourceSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new EmbeddedResourceSchemeHandlerFactory(),

            });
            
            if (!Cef.Initialize(settings))
            {
                throw new Exception("Could not initialize Chromium browser");
            }
        }

        private static void InitializeRedirectDictionary(IDictionary<string, string> dictionary)
        {
            AddResourcePathsToDictonary(dictionary, "Org.InCommon.InCert.Engine.Content.Html.Fonts", "resource://html/fonts/");
            AddResourcePathsToDictonary(dictionary, "Org.InCommon.InCert.Engine.Content.Html.Css", "resource://html/css/");
            AddResourcePathsToDictonary(dictionary, "Org.InCommon.InCert.Engine.Content.Html.Scripts", "resource://html/scripts/");
        }

        private static void AddResourcePathsToDictonary(IDictionary<string, string> dictionary, string manifestPrefix, string urlPrefix)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resources = assembly.GetManifestResourceNames()
                .Where(p => p.StartsWith(manifestPrefix));

            foreach (var resource in resources.Select(r => RemoveManifestPrefix(r, manifestPrefix)))
            {
                dictionary[resource.ToLowerInvariant()] = urlPrefix + resource;
            }
        }
        
        private static string RemoveManifestPrefix(string value, string prefix)
        {
            value = value.Replace(prefix, "");
            return value.TrimStart('.');
        }
    }
}
