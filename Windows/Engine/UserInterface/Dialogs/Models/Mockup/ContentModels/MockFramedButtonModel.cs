using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockFramedButtonModel
    {

        private readonly ImageSource _image;
        private readonly ImageSource _mouseOverImage;

        public MockFramedButtonModel()
        {

            _image = LoadMockImage();
            _mouseOverImage = LoadMockImage();
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
        public bool Enabled { get { return true; } }
        public HorizontalAlignment HorizontalAlignment { get { return HorizontalAlignment.Left;} }
        public MockButtonImage ButtonImage { get { return new MockButtonImage(_image, _mouseOverImage); } }
        public Dock Dock { get { return Dock.Left; } }
        public Brush TextBrush { get { return new SolidColorBrush(Colors.LemonChiffon);} }

        public SolidColorBrush BorderBrush
        {
            get { return new SolidColorBrush(Colors.LightBlue); }
        }

        private static ImageSource LoadMockImage()
        {
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    Engine.Properties.Resources.TestImage72.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(72, 72));
            }
            catch (Exception e)
            {
                return null;
            }
        }

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

       
    }


}

