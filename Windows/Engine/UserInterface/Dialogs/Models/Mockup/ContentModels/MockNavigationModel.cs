using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    class MockNavigationModel
    {

        

        private readonly ImageSource _image;
        private readonly ImageSource _mouseOverImage;

        public MockNavigationModel()
        {

            var canvas1 = new Canvas { Width = 28, Height = 29, Background = new SolidColorBrush(Colors.Wheat) };

            var rect1 = new Rectangle
            {
                Width = 28,
                Height = 29,
                Fill = new SolidColorBrush(Colors.White),
                Stroke = new SolidColorBrush(Colors.OliveDrab)
            };

            canvas1.Children.Add(rect1);
            var target = new RenderTargetBitmap(28, 29, 72, 72, PixelFormats.Pbgra32);
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

            var target2 = new RenderTargetBitmap(28, 29, 72, 72, PixelFormats.Pbgra32);
            target.Render(new Rectangle
            {
                Width = 72,
                Height = 72,
                Fill = new SolidColorBrush(Colors.Yellow),
                Stroke = new SolidColorBrush(Colors.YellowGreen)
            });

            _mouseOverImage = target2;
        }

        public virtual Visibility Visibility { get { return Visibility.Visible; } }
        public string Text { get; set; }

        public SolidColorBrush TextBrush
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public bool Enabled { get { return true; } }

        public ICommand Command
        {
            get { return null; }
        }

        public bool IsDefaultButton {get { return false; }}
        public bool IsCancelButton { get { return false; }}

        public MockButtonImage ButtonImage { get { return new MockButtonImage(_image, _mouseOverImage); } }
    }
}