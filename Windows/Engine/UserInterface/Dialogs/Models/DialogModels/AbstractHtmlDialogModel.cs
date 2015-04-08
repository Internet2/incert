using System;
using System.Collections.Generic;
using System.Windows;
using log4net;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    public abstract class AbstractHtmlDialogModel : AbstractDialogModel
    {
        private static readonly ILog Log = Logger.Create();

        protected AbstractHtmlDialogModel(IEngine engine, Window dialogInstance)
            : base(engine, dialogInstance)
        {
        }

        public LinkPolicy LinkPolicy { private get; set; }
        public string Url { private get; set; }
        public Dictionary<string, string> Redirects { private get; set; }
        
        public override void LoadContent(AbstractBanner banner)
        {
            try
            {
                SetNavigationModels(banner.GetButtons());

                if (!ShowingUrl(Url))
                {
                    var content = new BrowserContentModel(this, Url, Redirects, LinkPolicy);

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

                LoadUrl();
            }
            catch (Exception e)
            {
                Log.WarnFormat("An exception occurred while attempting to load banner content: {0}", e.Message);
            }
        }

        private void LoadUrl()
        {
            var model = ContentModel as BrowserContentModel;
            if (model == null)
            {
                throw new Exception("Could not retrieve browser content model");
            }

            if (ShowingUrl(model, Url))
            {
                model.Reload();
            }
            else
            {
                model.SetAddress(Url);
            }
        }
        
        private bool ShowingUrl(string url)
        {
            return ShowingUrl(ContentModel as BrowserContentModel, url);
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