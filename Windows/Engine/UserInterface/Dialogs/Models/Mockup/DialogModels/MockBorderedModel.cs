using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.DialogModels
{
    class MockBorderedModel
    {
        public string WindowTitle { get { return "Mock Bordered Window"; } }
        public bool IsEnabled { get { return true; } }
        public Brush Background { get { return new SolidColorBrush(Colors.CornflowerBlue); } }
        public FontFamily Font { get {return new FontFamily("Verdana");} }
        public double Width { get { return 500; } }
        public double Height { get { return 400; } }
        public WindowStyle WindowStyle { get {return WindowStyle.SingleBorderWindow;} }
        public ImageSource Icon { get { return null; } }
        public Cursor Cursor { get { return Cursors.Arrow; } }
        public MockBorderedContent ContentModel { get; private set; }
        public MockNavigationModel NextModel { get; private set; }
        public MockNavigationModel BackModel { get; private set; }
        public MockNavigationModel HelpModel { get; private set; }
        public MockNavigationModel AdvancedModel { get; private set; }
        public bool ShowInTaskbar { get { return true; } }
        public double Left { get; set; }
        public double Top { get; set; }

        public MockBorderedModel()
        {
            ContentModel = new MockBorderedContent();
            NextModel = new MockNavigationModel{Text = "Next", Margin = new Thickness(0,6,14,8)};
            BackModel = new MockNavigationModel { Text = "Back", Margin=new Thickness(0,6,4,8) };
            AdvancedModel = new MockNavigationModel { Text = "Advanced", Margin = new Thickness(4,6,0,8)};
            HelpModel = new MockHiddenNavigtationModel { Text = "Help", Margin = new Thickness(14,6,0,8)};
        }
    }

    class MockHiddenNavigtationModel:MockNavigationModel
    {
        public override Visibility Visibility {get {return Visibility.Collapsed;}}
    }

    class MockBorderedContent
    {
        public Cursor Cursor { get; private set; }
        public DockPanel Content { get; private set; }
        public Brush Background { get { return new SolidColorBrush(Colors.CadetBlue); } }
        public MockBorderedContent()
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
