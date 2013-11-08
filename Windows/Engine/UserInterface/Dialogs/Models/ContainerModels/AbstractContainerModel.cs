using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Ninject;
using Ninject.Parameters;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels
{
    public abstract class AbstractContainerModel : AbstractModel
    {
        private Thickness _padding;
        private Brush _background;
        private VerticalAlignment _verticalAlignment;
        private HorizontalAlignment _horizontalAlignment;
        private Thickness _margin;
        private double _width;
        private double _height;

        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged(); }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged(); }
        }

        public Thickness Margin
        {
            get { return _margin; }
            set { _margin = value; OnPropertyChanged(); }
        }

        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set { _verticalAlignment = value; OnPropertyChanged(); }
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set { _horizontalAlignment = value; OnPropertyChanged(); }
        }

        private AbstractContainerModel _contentModel;

        protected AbstractContainerModel(AbstractModel parentModel)
            : base(parentModel)
        {

        }

        protected AbstractContainerModel(AbstractDialogModel parentModel)
            : base(parentModel)
        {

        }

        public AbstractContainerModel ContentModel
        {
            get { return _contentModel; }
            set
            {
                _contentModel = value;
                OnPropertyChanged();
            }
        }

        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged();
            }
        }

        public Thickness Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                OnPropertyChanged();
            }
        }

        public List<DependencyObject> ChildInstances
        {
            get
            {
                var result = (from model in ChildModels.Values where model.Content != null select model.Content).ToList();
                return result;
            }
        }

        public Cursor Cursor { get { return RootDialogModel.Cursor; } }

        public AbstractBanner CurrentBanner { get; private set; }
        
        public virtual T LoadContent<T>(AbstractBanner banner) where T : DependencyObject
        {
            if (banner == null)
                return default(T);

            CurrentBanner = banner;
            VerticalAlignment = banner.VerticalAlignment;
            HorizontalAlignment = banner.HorizontalAlignment;
            Background = AppearanceManager.GetBrushForColor(banner.Background, AppearanceManager.BackgroundBrush);
            Enabled = true;
            Width = banner.Width;
            Height = banner.Height;

            AddChildModels(banner);

            Content = CreateInstance(banner);
            return Content as T;
        }

        protected void AddChildModels(AbstractBanner banner)
        {
            foreach (var paragraph in banner.GetParagraphs())
            {
                var modelType = paragraph.GetSupportingModelType();
                if (modelType == null)
                    continue;

                var contentModel = Application.Current.CurrentKernel().Get(
                    modelType, new ConstructorArgument("parentModel", this)) as AbstractModel;
                if (contentModel == null)
                    continue;

                var element = contentModel.LoadContent<UIElement>(paragraph);
                if (element == null)
                    continue;

                AddChildModel(paragraph.ControlKey, contentModel);
            }
        }

        protected abstract DependencyObject CreateInstance(AbstractBanner banner);
    }
}
