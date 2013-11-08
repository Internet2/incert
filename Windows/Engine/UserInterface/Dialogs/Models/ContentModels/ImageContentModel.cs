using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class ImageContentModel : AbstractContentModel
    {
        private static readonly ILog Log = Logger.Create();

        private ImageSource _source;
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

        public ImageSource Source
        {
            get { return _source; }
            set { _source = value; OnPropertyChanged(); }
        }

        public ImageContentModel(AbstractModel parentModel): base(parentModel)
        {
        }

        
        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var imageSource = GetSourceFromWrapper(wrapper as SystemIconParagraph);
            if (imageSource == null)
            {
                Log.Warn("Cannot initialize image source from wrapper");
                return default (T);
            }

            Source = imageSource;

            var content = new Image();
              
            InitializeBindings(content);
            InitializeValues(wrapper);

            Content = content;
            return content as T;
        }

        protected override void InitializeBindings(DependencyObject target)
        {
            base.InitializeBindings(target);
            BindingOperations.SetBinding(target, Image.SourceProperty, GetOneWayBinding(this, "Source"));
            BindingOperations.SetBinding(target, FrameworkElement.WidthProperty, GetOneWayBinding(this, "Width"));
            BindingOperations.SetBinding(target, FrameworkElement.HeightProperty, GetOneWayBinding(this, "Height"));
        }

        protected override void InitializeValues(AbstractContentWrapper wrapper)
        {
            base.InitializeValues(wrapper);
            var imageSource = GetSourceFromWrapper(wrapper as SystemIconParagraph);
            if (imageSource == null)
            {
                Log.Warn("Cannot initialize image source from wrapper");
                return;
            }

            Source = imageSource;
            Width = imageSource.Width;
            Height = imageSource.Height;
        }
        
        private static ImageSource GetSourceFromWrapper(SystemIconParagraph wrapper)
        {
            if (wrapper == null)
                return null;

            return UserInterfaceUtilities.GetSystemIconImageSourceFromApi(
                wrapper.Icon, wrapper.Size, false, false);
        }
    }
}
