using System;
using System.Diagnostics;
using System.Windows;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Logging;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class UrlBottomButtonModel:AbstractBottomButtonModel
    {
        private readonly string _url;
        
        public UrlBottomButtonModel(PagedViewModel model, string url) : base(model)
        {
            _url = url;
            Command = new RelayCommand(param =>LaunchUrl(), param => ! string.IsNullOrWhiteSpace(_url));
        }
        
        public override Visibility Visibility
        {
            get { return Visibility.Visible; }
        }

        private void LaunchUrl()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_url))
                    return;

                var info = new ProcessStartInfo(_url) { UseShellExecute = true };
                Process.Start(info);
            }
            catch (Exception e)
            {
                Logger.Error("An issue occurred while attempting to launch the help topic", e);
            }
        }
    }
}
