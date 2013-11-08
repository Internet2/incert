using System.Windows.Input;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    class MockAdvancedMenuItem
    {
        public string KeyText { get { return "A"; } }
        public string Title { get { return "Mock Menu Item Title"; } }
        public Brush Background { get { return new SolidColorBrush(Colors.Blue); } }
        public Brush GraphicForeground {get {return new SolidColorBrush(Colors.Yellow);}}
        public Brush GraphicBackground {get {return new SolidColorBrush(Colors.LightBlue);}}
        public Brush Highlight { get { return new SolidColorBrush(Colors.LightCyan); } }
        public Brush TextBrush { get { return new SolidColorBrush(Colors.Yellow); } }
        public FontFamily FontFamily { get { return new FontFamily("Verdana"); } }
        public ICommand SingleClickCommand { get { return null; } }
        public ICommand DoubleClickCommand { get { return null; } }
        public object Tag { get { return this; } }
        public bool IsEnabled { get { return true; } }


    }
}

