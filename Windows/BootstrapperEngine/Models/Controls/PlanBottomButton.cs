using System.Windows;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class PlanBottomButton:AbstractBottomButtonModel
    {
        public PlanBottomButton(PagedViewModel model, InstallActions action) : base(model)
        {
            Command = new RelayCommand(param => Model.BaseModel.Plan(action));
        }

        public override Visibility Visibility
        {
            get { return Visibility.Visible; }
        }

        
    }
}
