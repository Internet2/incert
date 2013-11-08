using System.Windows;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class CancelBottomButtonModel : AbstractBottomButtonModel
    {
        public CancelBottomButtonModel(PagedViewModel model)
            : base(model)
        {
            Command = new RelayCommand(param => Model.Cancelled = true, param => Model.Cancelled != true & Model.InstallationState == InstallationState.Applying);
        }

        public override Visibility Visibility
        {
            get { return Visibility.Visible; }
        }
    }
}
