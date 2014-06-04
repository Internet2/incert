using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockFramedButtonModel
    {

        private readonly ImageSource _image;
        private readonly ImageSource _mouseOverImage;

        public MockFramedButtonModel()
        {
            var canvas1 = new Canvas { Width = 72, Height = 72, Background = new SolidColorBrush(Colors.Wheat) };

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
                var bi = new BitmapImage { CacheOption = BitmapCacheOption.OnLoad };
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
        public SolidColorBrush Background { get { return new SolidColorBrush(Colors.Aquamarine); } }
        public double Width { get { return 250; } }
        public double Height { get { return 250; } }
        public Thickness BorderSize { get { return new Thickness(3); } }
        public bool IsDefaultButton { get { return false; } }
        public bool IsCancelButton { get { return false; } }
        public SolidColorBrush GlowBrush { get { return new SolidColorBrush(Colors.Wheat); } }
        public string Text { get { return "Lorem Ipsum Dolor"; } }

        public MockButtonImage ButtonImage { get { return new MockButtonImage(_image, _mouseOverImage); } }

        public MockButtonText Caption { get { return new MockButtonText("Lorem Ipsum Dolor"){FontSize = 18}; } }
        public MockButtonText SubCaption { get { return new MockButtonText("Lorem Ipsum Dolor 2"){FontSize = 11, Margin = new Thickness(0,8,0,0)}; } }

        public class MockButtonText
        {
            private readonly string _value;

            public MockButtonText(string value)
            {
                _value = value;
            }

            public string Value { get { return _value; } }
            public VerticalAlignment VerticalAlignment { get { return VerticalAlignment.Center; } }
            public HorizontalAlignment HorizontalAlignment { get { return HorizontalAlignment.Center; } }
            public Thickness Margin { get; set; }
            public Thickness Padding { get; set; }
            public double FontSize { get; set; }
            public FontFamily FontFamily { get { return new FontFamily("Verdana");} }
            public FontWeight FontWeight { get { return FontWeights.Bold;} }
            public Style Style { get { return null; } }
            public Visibility Visibility { get {  return Visibility.Visible;} }
        }

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
        }
    }


}

