using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.DialogModels
{
    class MockHelpDialogModel
    {
        public Visibility Visibility { get { return Visibility.Visible; } }
        public Brush TextBrush { get { return new SolidColorBrush(Colors.Black); } }
        public Brush Background {get {return new SolidColorBrush(Colors.DarkOrange);}}
        public Brush HighlightBrush {get {return new SolidColorBrush(Colors.Yellow);}}
        public double FontSize { get { return 12; } }
        public Thickness Margin { get { return new Thickness(8); } }
        public Dock Dock { get { return Dock.Top; } }
        public FontFamily FontFamily { get { return new FontFamily("Verdana"); } }
        public Thickness Padding { get { return new Thickness(4, 0, 0, 0); } }
        public string Url { get { return "http://getconnected.iu.edu"; } }
        public bool Enabled { get; set; }
        public string Title { get { return "Help Dialog"; } }
        public ICommand BackCommand { get { return null; } }
        public ICommand ForwardCommand { get { return null; } }
        public ICommand ReloadCommand { get { return null; } }
        public ICommand HomeCommand { get { return null; } }
        public ICommand PreserveCommand { get { return null; } }
        public bool CanGoBack { get { return true; } }
        public bool CanGoForward { get { return false; } }
        
        public double Left { get; set; }
        public double Top { get; set; }

        public bool PreserveChecked { get; set; }
        public string TopBannerText { get { return "More Help Options"; } }
        public string PreserveText { get { return "Preserve this content in a new windows when the utility exits"; } }
    }

  
}
