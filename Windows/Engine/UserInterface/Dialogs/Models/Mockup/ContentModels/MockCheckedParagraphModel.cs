using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    class MockCheckedParagraphModel
    {
        public Visibility Visibility {get {return Visibility.Visible;}}
        public Brush TextBrush {get {return new SolidColorBrush(Colors.Black);}}
        public Brush CheckBrush {get {return new SolidColorBrush(Colors.Red);}}
        public double FontSize { get { return 18; } }
        public string Text { get { return "Checking Windows Security Center"; } }
        public Thickness Margin {get {return new Thickness(0);}}
        public Dock Dock { get { return Dock.Top; } }
        public FontFamily Font {get {return new FontFamily("Verdana");}}
        public MockLowerContentModel LowerContent {get {return new MockLowerContentModel();}}

        internal class MockLowerContentModel
        {
            public double FontSize { get { return 10; } }
            public string Content { get { return "Lower content text"; } }
        }
        
    }
}
