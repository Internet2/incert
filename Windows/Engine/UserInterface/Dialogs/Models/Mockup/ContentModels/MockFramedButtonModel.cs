using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockFramedButtonModel
    {

        private readonly ImageSource _image;
        private readonly ImageSource _mouseOverImage;

        public MockFramedButtonModel()
        {
            var canvas1 = new Canvas {Width = 72, Height = 72, Background = new SolidColorBrush(Colors.Wheat)};
            
            var rect1 = new Rectangle
            {
                Width = 72,
                Height = 72,
                Fill = new SolidColorBrush(Colors.White),
                Stroke = new SolidColorBrush(Colors.OliveDrab)
            };
          
            canvas1.Children.Add(rect1);

            var target = new RenderTargetBitmap(72, 72, 96, 96, PixelFormats.Pbgra32);
            target.Render(canvas1);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(target));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var bi =  new BitmapImage {CacheOption = BitmapCacheOption.OnLoad};
                bi.BeginInit();
                bi.StreamSource = stream;
                bi.EndInit();
                _image = bi;
            }


            _image = target;

            var target2 = new RenderTargetBitmap(72, 72, 96, 96, PixelFormats.Pbgra32);
            target.Render(new Rectangle
            {
                Width = 72,
                Height = 72,
                Fill = new SolidColorBrush(Colors.Yellow),
                Stroke = new SolidColorBrush(Colors.YellowGreen)
            });
           
            _mouseOverImage = target2;

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
        public ImageSource MouseOverImageSource { get { return _mouseOverImage; } }
        public string Text { get { return "Lorem Ipsum Dolor"; } }
    }
}

