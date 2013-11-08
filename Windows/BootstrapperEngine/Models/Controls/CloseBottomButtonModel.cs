using System.Windows;
using Org.InCommon.InCert.BootstrapperEngine.Commands;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class CloseBottomButtonModel:AbstractBottomButtonModel
    {
        public CloseBottomButtonModel(PagedViewModel model, int exitCode) : base(model)
        {
            Command = new RelayCommand(param=>model.Close(exitCode));
        }

        public override Visibility Visibility
        {
            get { return Visibility.Visible; }
        }
    }
}
