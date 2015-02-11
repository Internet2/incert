using System;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public abstract class AbstractHtmlBannerTask:AbstractTask
    {
        private readonly string _identifier = Guid.NewGuid().ToString();

        protected AbstractHtmlBannerTask(IEngine engine) : base(engine)
        {
        }
        
        [PropertyAllowedFromXml]
        public string Url
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public double Width { get; set; }

        [PropertyAllowedFromXml]
        public double Height { get; set; }

        internal static void SetAddress(AbstractDialogModel dialog, string url)
        {
            var model = dialog.ContentModel.FindChildModel<BrowserContentModel>("browser");
            if (model == null)
            {
                throw new Exception("Could not retrieve browser content model");
            }

            model.Address = url;
        }

        protected static void WaitForLoad(AbstractDialogModel dialog)
        {
            var model = dialog.ContentModel.FindChildModel<BrowserContentModel>("browser");
            if (model == null)
            {
                return;
            }
            while (!model.IsLoaded)
            {
                Application.Current.DoEvents();
                Thread.Sleep(5);
            }
        }

        protected AbstractBanner GetOrCreateBanner()
        {
            var banner = BannerManager.GetBanner(_identifier);
            if (banner != null)
            {
                return banner;
            }

            var wrapper = new BrowserContentWrapper(Engine)
            {
                Uri = new Uri(Url),
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                SilentMode = true,
                ControlKey = "browser",
                Width = Width,
                Height = Height
            };

            banner = new SimpleBanner(Engine)
            {
                Width = Width,
                Height = Height,
                CanClose = true,
                Margin = new Thickness(0)
            };

            banner.AddMember(wrapper);
            return BannerManager.SetBanner(_identifier, banner);
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);
            
            if (Width <= 0) Width = 600;
            if (Height <= 0) Height = 600;
        }
    }
}
