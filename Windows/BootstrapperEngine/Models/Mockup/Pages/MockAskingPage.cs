using System.Windows.Media;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Pages
{
    public class MockAskingPage
    {
        public string Title { get { return "An issue has occurred while attempting to do something important."; } }
        public string Details { get { return "These are the details for the issue"; } }
        public string Question { get { return "Would you like to try the operation again?"; } }
        public Brush Foreground { get { return new SolidColorBrush(Colors.White); } }
        public Brush Background { get { return new SolidColorBrush(Colors.Teal); } }
        public string Glyph { get { return "!"; } }
    }
}
