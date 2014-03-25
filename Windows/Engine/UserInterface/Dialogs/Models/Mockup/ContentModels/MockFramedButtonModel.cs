using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockFramedButtonModel
    {
        public Style Style { get { return null; } }
        public SolidColorBrush Background { get { return new SolidColorBrush(Colors.Aquamarine);} }
        public double Width { get { return 250; } }
        public double Height { get { return 250; } }
        public Thickness BorderSize { get { return new Thickness(3);} }
        public bool IsDefaultButton { get { return false; } }
        public bool IsCancelButton { get { return false; } }
        public SolidColorBrush GlowBrush { get { return new SolidColorBrush(Colors.Wheat);} }
        public ImageSource ImageSource { get { return new BitmapImage();} }
        public string Text { get { return "Lorem Ipsum Dolor"; } }
    }
}

