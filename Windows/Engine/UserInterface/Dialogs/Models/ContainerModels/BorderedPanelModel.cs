using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels
{
    class BorderedPanelModel : AbstractContainerModel
    {
        private readonly IBannerManager _manager;

        private Thickness _borderSize;
        private Brush _borderBrush;
        private CornerRadius _cornerRadius;
        private bool _canScroll;
        private ScrollBarVisibility _verticalScrollBarVisibility;
        private ScrollBarVisibility _horizontalScrollBarVisibility;

        public bool CanScroll
        {
            get { return _canScroll; }
            set { _canScroll = value; OnPropertyChanged(); }
        }

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return _verticalScrollBarVisibility; }
            set { _verticalScrollBarVisibility = value; OnPropertyChanged(); }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return _horizontalScrollBarVisibility; }
            set { _horizontalScrollBarVisibility = value; OnPropertyChanged(); }
        }

        public Thickness BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                OnPropertyChanged();
            }
        }

        public Brush BorderBrush
        {
            get { return _borderBrush ?? (_borderBrush = AppearanceManager.BodyTextBrush); }
            set { _borderBrush = value; OnPropertyChanged(); }
        }

        public CornerRadius CornerRadius
        {
            get { return _cornerRadius; }
            set { _cornerRadius = value; OnPropertyChanged(); }
        }

        public BorderedPanelModel(AbstractModel parentModel, IBannerManager manager)
            : base(parentModel)
        {
            _manager = manager;
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var contentWrapper = wrapper as ContentBlockParagraph;
            if (contentWrapper == null)
                return default(T);

            BorderSize = contentWrapper.BorderSize;
            BorderBrush = AppearanceManager.GetBrushForColor(
                contentWrapper.BorderColor,
                AppearanceManager.BodyTextBrush);
            CornerRadius = contentWrapper.CornerRadius;
            VerticalAlignment = contentWrapper.VerticalAlignment;
            Margin = wrapper.Margin.GetValueOrDefault(new Thickness(0));
            Padding = wrapper.Padding.GetValueOrDefault(new Thickness(0));
            Dock = wrapper.Dock;
            ControlKey = wrapper.ControlKey;
            Enabled = wrapper.Enabled;
            
            var banner = _manager.GetBanner(contentWrapper.Banner);
            return LoadContent<T>(banner);
        }


        public override T LoadContent<T>(AbstractBanner banner)
        {
            var result = base.LoadContent<T>(banner);
            if (result == null)
                return default(T);

            CanScroll = banner.Scrollable;
            VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;

            return result;
        }

        protected override DependencyObject CreateInstance(AbstractBanner banner)
        {
            if (banner == null)
                return null;

            if (banner.Scrollable)
                return new ScrollingBorderedContentPanel { DataContext = this };

            return new BorderedContentPanelInstance { DataContext = this };
        }
    }
}
