using System;
using System.Windows;
using CefSharp.Wpf;
using log4net;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    public abstract class AbstractHtmlDialogModel : AbstractDialogModel
    {
        private static readonly ILog Log = Logger.Create();
        
        protected AbstractHtmlDialogModel(IEngine engine, Window dialogInstance) : base(engine, dialogInstance)
        {
        }

        public void SetBrowserAddress(string url)
        {
            var model = ContentModel as BrowserContentModel;
            if (model == null)
            {
                throw new Exception("Could not retrieve browser content model");
            }

            if (ShowingUrl(model, url))
            {
                model.Reload();
            }
            else
            {
                model.SetAddress(url);
            }
        }
        
        public override void LoadContent(AbstractBanner banner)
        {
            try
            {
                SetNavigationModels(banner.GetButtons());

                var url = GetUrlFromBanner(banner as HtmlBanner);

                if (!ShowingUrl(url))
                {
                    var content = new BrowserContentModel(this);
                    content.LoadContent<DependencyObject>(banner);
                    content.Padding = banner.Margin;
                    content.Background = AppearanceManager.GetBrushForColor(banner.Background, AppearanceManager.BackgroundBrush);
                    ContentModel = content;
                }

                CanClose = banner.CanClose;
                SuppressCloseQuestion = banner.SuppressCloseQuestion;

                Height = banner.Height;
                Width = banner.Width;
                Background = AppearanceManager.GetBrushForColor(banner.Background, AppearanceManager.BackgroundBrush);
                Cursor = banner.Cursor;

                SetBrowserAddress(url);

            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to load banner content: {0}", e.Message);
            }
        }

        private string GetUrlFromBanner(HtmlBanner banner)
        {
            return banner == null
                ? string.Empty
                : banner.Url;
        }

        private bool ShowingUrl(string url)
        {
            if (ContentModel == null)
            {
                return false;
            }

            var model = ContentModel.FindChildModel<BrowserContentModel>("browser");
            return ShowingUrl(model, url);
        }

        private static bool ShowingUrl(BrowserContentModel browser, string url)
        {
            if (browser == null)
            {
                return false;
            }

            var address = browser.GetAddress();
            if (String.IsNullOrWhiteSpace(address))
            {
                return false;

            }

            return address.Equals(url, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
