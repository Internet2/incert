using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.Models.Controls;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Panels
{
    public class ButtonPanelModel : AbstractPanelModel
    {
        private AbstractBottomButtonModel _button1Model;
        private AbstractBottomButtonModel _button2Model;
        private AbstractBottomButtonModel _button5Model;
        private AbstractBottomButtonModel _button4Model;
        private AbstractBottomButtonModel _button3Model;

        private readonly UrlBottomButtonModel _defaultHelpButton;

        public ButtonPanelModel(PagedViewModel model)
            : base(model)
        {
            _defaultHelpButton = new UrlBottomButtonModel(Model, model.BaseModel.HelpUrl) { Text = "Help" };
        }

        public AbstractBottomButtonModel Button1Model
        {
            get { return _button1Model; }
            set { _button1Model = value; OnPropertyChanged("Button1Model"); }
        }

        public AbstractBottomButtonModel Button5Model
        {
            get { return _button5Model; }
            set { _button5Model = value; OnPropertyChanged("Button5Model"); }
        }

        public AbstractBottomButtonModel Button4Model
        {
            get { return _button4Model; }
            set { _button4Model = value; OnPropertyChanged("Button4Model"); }
        }

        public AbstractBottomButtonModel Button2Model
        {
            get { return _button2Model; }
            set { _button2Model = value; OnPropertyChanged("Button2Model"); }
        }

        public AbstractBottomButtonModel Button3Model
        {
            get { return _button3Model; }
            set { _button3Model = value; OnPropertyChanged("Button3Model"); }
        }

        protected override void ConfigureInstall()
        {

            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model, 1602) { Text = "Close" };
        }

        protected override void ConfigureStart()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new PlanBottomButton(Model, InstallActions.Repair) { Text = "Repair" };
            Button3Model = new DynamicShowExternalOptionsButtonModel(Model){Text = "Uninstall"};
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model, 0) { Text = "Close" };
        }

        protected override void ConfigureApplying()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CancelBottomButtonModel(Model) { Text = "Cancel" };
        }

        protected override void ConfigureInstallApplied()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model, 0) { Text = "Close" };
        }

        protected override void ConfigureRepairApplied()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new PlanBottomButton(Model, InstallActions.Repair) { Text = "Repair" };
            Button3Model = new DynamicShowExternalOptionsButtonModel(Model) { Text = "Uninstall" };
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model,0) { Text = "Close" };
        }

        protected override void ConfigureRemoveApplied()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model,0) { Text = "Close" };

        }

        protected override void ConfigureInitializing()
        {
        }

        protected override void ConfigureFailed()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model,Model.ErrorCode) { Text = "Close" };
        }

        protected override void ConfigureNewer()
        {

            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model, 1638) { Text = "Close" };
        }

        protected override void ConfigureOlder()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model, 1602) { Text = "Close" };
        }

        protected override void ConfigureLaunchingNew()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CancelBottomButtonModel(Model) { Text = "Cancel" };
        }

        protected override void ConfigureLaunchingCurrent()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new CloseBottomButtonModel(Model, 0) { Text = "Close" };
        }

        protected override void Cancelling()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new InvisibleButtonModel(Model); 
        }

        protected override void ConfigureShowRemoveOptions()
        {
            Button1Model = _defaultHelpButton;
            if (Model.ExternalEngine.AllowBack)
                Button2Model = new StateBottomButton(Model, InstallationState.DetectedPresent){Text = "Menu"};
            else
                Button2Model = new InvisibleButtonModel(Model);

            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            
            Button5Model = new StartExternalBottomButton(Model, InstallActions.Remove){Text="Uninstall"}; 
        }

        protected override void ConfigureRunningExternal()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new InvisibleButtonModel(Model);
            Button5Model = new InvisibleButtonModel(Model); 
        }

        protected override void ConfigureAskExternalRetry()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new DynamicShowExternalOptionsButtonModel(Model) { Text = "Menu" };
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new StartExternalBottomButton(Model,InstallActions.Remove){Text = "Retry"};
            Button5Model = new CloseBottomButtonModel(Model, 1602) { Text = "Cancel" };
        }

        protected override void ConfigureAskCancel()
        {
            Button1Model = _defaultHelpButton;
            Button2Model = new InvisibleButtonModel(Model);
            Button3Model = new InvisibleButtonModel(Model);
            Button4Model = new StateBottomButton(Model, InstallationState.Cancelling){ Text = "Yes" };
            Button5Model = new StateBottomButton(Model, InstallationState.Applying) { Text = "No" };
        }
    }
}
