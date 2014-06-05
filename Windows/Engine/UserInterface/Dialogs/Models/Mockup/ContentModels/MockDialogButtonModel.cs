using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockDialogButtonModel
    {

        private readonly ImageSource _image;
        private readonly ImageSource _mouseOverImage;

        public MockDialogButtonModel()
        {

            _image = LoadMockImage();
            _mouseOverImage = LoadMockImage();
        }

        public bool IsDefaultButton { get { return false; } }
        public bool IsCancelButton { get { return false; } }
        public Visibility Visibility { get { return Visibility.Visible; } }
        public Boolean Enabled { get { return true; } }
        public Brush TextBrush { get { return new SolidColorBrush(Colors.OrangeRed);} }
        public ICommand Command { get { return null; } }
        public FontFamily Font { get { return new FontFamily("Verdana"); } }
        public double FontSize { get { return 11; } }

        public MockButtonImage ButtonImage { get { return new MockButtonImage(_image, _mouseOverImage); } }

        private static ImageSource LoadMockImage()
        {
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    Engine.Properties.Resources.TestImage32.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(32, 32));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string Text { get { return "Button Text"; } }
    }
}
