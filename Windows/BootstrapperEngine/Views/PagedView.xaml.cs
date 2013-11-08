using System;
using Org.InCommon.InCert.BootstrapperEngine.Logging;

namespace Org.InCommon.InCert.BootstrapperEngine.Views
{
    /// <summary>
    /// Interaction logic for PagedView.xaml
    /// </summary>
    public partial class PagedView
    {
        public PagedView()
        {
            InitializeComponent();
        }

        private void ClosedHandler(object sender, EventArgs e)
        {
            Logger.Standard("Window close event detected");
            Dispatcher.InvokeShutdown();
        }

       
    }
}
