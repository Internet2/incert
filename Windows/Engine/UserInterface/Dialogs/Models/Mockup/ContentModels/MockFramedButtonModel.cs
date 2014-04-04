using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockFramedButtonModel
    {

        private ImageSource _image;

        public MockFramedButtonModel()
        {
            var target = new RenderTargetBitmap(72, 72, 72, 72, PixelFormats.Default);
            target.Render(new Rectangle(){Width = 72,Height = 72, Fill = new SolidColorBrush(Colors.White)});
            _image = target;

        }

        public Style Style { get { return null; } }
        public SolidColorBrush Background { get { return new SolidColorBrush(Colors.Aquamarine);} }
        public double Width { get { return 250; } }
        public double Height { get { return 250; } }
        public Thickness BorderSize { get { return new Thickness(3);} }
        public bool IsDefaultButton { get { return false; } }
        public bool IsCancelButton { get { return false; } }
        public SolidColorBrush GlowBrush { get { return new SolidColorBrush(Colors.Wheat);} }
        public ImageSource ImageSource { get { return _image; } }
        public string Text { get { return "Lorem Ipsum Dolor"; } }
    }
}

