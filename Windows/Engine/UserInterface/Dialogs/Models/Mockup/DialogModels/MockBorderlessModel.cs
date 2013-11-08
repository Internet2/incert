using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.DialogModels
{
    class MockBorderlessModel
    {
        public string WindowTitle { get; private set; }
        public bool IsEnabled { get; private set; }
        public Brush Background { get; private set; }
        public FontFamily Font { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public Cursor Cursor { get { return Cursors.Arrow; } }
        public MockBorderlessContent ContentModel { get; private set; }
        public ImageSource Icon { get; private set; }
        public bool ShowInTaskbar { get { return true; } }

        public DropShadowEffect DropShadow
        {
            get
            {
                return new DropShadowEffect() {BlurRadius = 10, Color = Colors.Black, Opacity = .8, ShadowDepth = 8};
            }
        }

        public MockBorderlessModel()
        {
            WindowTitle = "Mock Borderless Window";
            IsEnabled = true;
            Background = new SolidColorBrush(Colors.CornflowerBlue);
            Font = new FontFamily("Verdana");
            Width = 300;
            Height = 300;
            Icon = null;
            ContentModel = new MockBorderlessContent{Width = Width, Height = Height};
        }
    }

    class MockBorderlessContent
    {
        public Cursor Cursor { get; private set; }
        public DockPanel Content { get; private set; }
        public Brush Background { get { return new SolidColorBrush(Colors.CadetBlue); } }
        public double Width { get; set; }
        public double Height { get; set; }
        public MockBorderlessContent()
        {
            Cursor = Cursors.Hand;
            Content = new DockPanel
                {
                    Background = new SolidColorBrush(Colors.CadetBlue),
                };
            Content.Children.Add(new Label
                {
                    Content = "Mock Content",
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.White),
                    Margin = new Thickness(20, 20, 20, 20)

                });
        }

    }
}
