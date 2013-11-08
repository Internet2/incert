using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.Pages;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.PageModels
{
    public class BannerPageModel : AbstractModel
    {
        private ContentPage _instance;
        private Brush _background;
        private Cursor _cursor;
        private Thickness _padding;
        private ContentPanelModel _contentPanel;

        private readonly Dictionary<string, ContentPanelModel> _contentCache = new Dictionary<string, ContentPanelModel>();

        public BannerPageModel(AbstractDialogModel dialogModel): base(dialogModel)
        {
        }

        public BannerPageModel(AbstractModel parentModel): this(parentModel.RootDialogModel)
        {
        }

        public ContentPage Page
        {
            get { return _instance ?? (_instance = new ContentPage {DataContext = this}); }
        }

        public Thickness Padding
        {
            get { return _padding; }
            set { _padding = value; OnPropertyChanged();
            }
        }

        public ContentPanelModel ContentModel
        {
            get { return _contentPanel; }
            private set
            {
                _contentPanel = value; OnPropertyChanged();
            }
        }

       
        
        public Brush Background
        {
            get { return _background; }
            private set {_background = value; OnPropertyChanged(); }
        }

        public Cursor Cursor
        {
            get { return _cursor; }
            set { _cursor = value; OnPropertyChanged(); }
        }

        public void LoadBanner(AbstractBanner banner)
        {
            if (banner == null)
                return;

            var result = GetBannerFromCache(banner);
            
            ClearChildModels();
            AddChildModel("ContentPanel", result);
            
            

            ContentModel = result;
            Content = ContentModel.Content;
            Background = AppearanceManager.GetBrushForColor(banner.Background, AppearanceManager.BackgroundBrush);

            Padding = banner.Margin;
        }

        private ContentPanelModel GetBannerFromCache(AbstractBanner banner)
        {
            if (_contentCache.ContainsKey(banner.Name))
            {
                var cachedContent = _contentCache[banner.Name];
                cachedContent.RestoreOriginalValues();
                return cachedContent;
            }

            var content = new ContentPanelModel(this);
            content.LoadContent<DependencyObject>(banner);

            if (banner.Cache)
                _contentCache[banner.Name] = content;

            return content;
        }




    }
}
