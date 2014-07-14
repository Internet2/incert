using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    class MockNavigationModel
    {

        

        private readonly ImageSource _image;
        private readonly ImageSource _mouseOverImage;

        public MockNavigationModel()
        {

            _image = LoadMockImage();
            _mouseOverImage = LoadMockImage();
        }

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
        public Thickness Margin { get; set; }
    }
}