using System.Windows.Input;
using System.Windows.Media;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Pages
{
    class MockButtonPageModel
    {
        public ICommand Command { get { return null; } }
        public Brush Foreground {get {return new SolidColorBrush(Colors.White);}}
        public Brush Background { get {return new SolidColorBrush(Colors.Teal);} }
        public string ButtonTitle { get { return "Install"; } }
        public string ButtonSubTitle { get { return "InCert"; } }
        public string Instructions { get { return "Click Install to start the InCert installation process."; } }
        public bool ButtonSubTitleVisible { get { return !string.IsNullOrWhiteSpace(ButtonSubTitle); } }
        public Cursor Cursor { get { return Cursors.Hand; } }
    }
}
