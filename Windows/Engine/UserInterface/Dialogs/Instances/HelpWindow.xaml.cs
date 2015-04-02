using System.Windows;
using CefSharp.Wpf;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public ChromiumWebBrowser Browser { get; private set; }
        
        public HelpWindow()
        {
            InitializeComponent();
            Browser = new ChromiumWebBrowser();
            BrowserGrid.Children.Add(Browser);
        }

        private void OnClosingHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
}
