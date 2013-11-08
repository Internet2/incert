using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Controls;
using Org.InCommon.InCert.BootstrapperEngine.Models.Mockup.Pages;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Mockup
{
    public class MockPagedView
    {
        public MockPageModel PageModel { get { return new MockPageModel(); } }

        public Brush Background { get { return new SolidColorBrush(Colors.Teal); } }
        public Brush Foreground { get { return new SolidColorBrush(Colors.White); } }
        public FontFamily FontFamily { get { return new FontFamily("Verdana"); } }
        public double FontSize { get { return 12; } }

        public MockButtonPanelModel BottomButtonsModel { get { return new MockButtonPanelModel(); } }

        public object WindowTitle
        {
            get {  return "InCert Installer"; } 
        }

        public object Cursor
        {
            get { return Cursors.Arrow; }
        }
    }
}
