using System;
using System.Windows;
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

        public MockButtonImage ButtonImage { get { return new MockButtonImage(_image, _mouseOverImage); } }

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
    }
}
