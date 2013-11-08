using System;
using System.Windows;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.External;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Controls
{
    class StartExternalBottomButton : AbstractBottomButtonModel
    {
        public StartExternalBottomButton(PagedViewModel model, InstallActions postExecuteAction)
            : base(model)
        {
            Command = new RelayCommand(p => ExecuteExternal(postExecuteAction));
        }

        public override Visibility Visibility
        {
            get { return Visibility.Visible; }
        }

        private void ExecuteExternal(InstallActions postExecuteAction)
        {
            var result = Model.ExternalEngine.Execute();

            if (result == ExecuteResult.Continue)
            {
                Model.BaseModel.Plan(postExecuteAction);
                return;
            }

            if (result == ExecuteResult.Restart)
            {
                // todo: implement
                Model.BaseModel.Plan(postExecuteAction);
                return;
            }

            Model.Dispatcher.Invoke(new Action(() => Model.InstallationState = InstallationState.AskingExternalIssueRetry));
        }
    }
}
