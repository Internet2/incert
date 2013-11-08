using System.Windows.Media;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Pages
{
    public class MockProgressPageModel
    {
        public Brush Background { get { return new SolidColorBrush(Colors.Teal); } }
        public Brush Foreground {get {return new SolidColorBrush(Colors.White);}}
        public string Title { get { return "Installing InCert"; } }
        public string Subtitle { get { return "50% complete"; } }
        public string Note { get { return "It may take a few minutes to finish installing InCert...."; } }
    }
}
