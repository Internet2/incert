using System.Windows;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class DynamicShowExternalOptionsButtonModel:AbstractBottomButtonModel
    {
        public DynamicShowExternalOptionsButtonModel(PagedViewModel model) : base(model)
        {
            if (model.ExternalEngine == null  || !model.ExternalEngine.IsValid)
                Command = new RelayCommand(param => Model.BaseModel.Plan(InstallActions.Remove));

            Command = new RelayCommand(param => Model.InstallationState = InstallationState.ShowingRemoveOptions);
        }

        public override Visibility Visibility
        {
            get { return Visibility.Visible; }
        }
    }
}
