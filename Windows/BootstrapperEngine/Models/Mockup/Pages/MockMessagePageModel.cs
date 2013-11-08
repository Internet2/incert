using System.Windows.Media;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Pages
{
    public class MockMessagePageModel
    {
        public Brush Foreground { get { return new SolidColorBrush(Colors.White); } }
        public Brush Background { get { return new SolidColorBrush(Colors.Teal); } }
        public string Text
        {
            get
            {
                return
                    "Lorem ipsum dolor sit amet, consectetur adipisicing elit, " +
                    "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
            }
        }
    }
}
