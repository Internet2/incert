using System.Windows;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockButtonImage
    {
        private readonly ImageSource _image;
        private readonly ImageSource _mouseOverImage;

        public MockButtonImage(ImageSource image, ImageSource mouseOverImage)
        {
            _image = image;
            _mouseOverImage = mouseOverImage;
        }

        public ImageSource ImageSource { get { return _image; } }
        public ImageSource MouseOverImageSource { get { return _mouseOverImage; } }
        public VerticalAlignment VerticalAlignment { get { return VerticalAlignment.Center; } }
        public HorizontalAlignment HorizontalAlignment { get { return HorizontalAlignment.Center; } }
        public Thickness Margin { get { return new Thickness(1); } }
        public Visibility Visibility { get { return Visibility.Visible; } }
    }
}