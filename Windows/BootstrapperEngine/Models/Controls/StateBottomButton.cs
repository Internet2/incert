using System.Windows;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class StateBottomButton:AbstractBottomButtonModel
    {
        public StateBottomButton(PagedViewModel model, InstallationState state) : base(model)
        {
            Command = new RelayCommand(param => Model.InstallationState = state);
        }

        public override Visibility Visibility
        {
            get { return Visibility.Visible; }
        }
    }
}
