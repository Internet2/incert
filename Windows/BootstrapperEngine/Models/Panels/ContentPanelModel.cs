using System.Windows.Controls;
using System.Windows.Input;
using Org.InCommon.InCert.BootstrapperEngine.Commands;
using Org.InCommon.InCert.BootstrapperEngine.Enumerations;
using Org.InCommon.InCert.BootstrapperEngine.Extensions;
using Org.InCommon.InCert.BootstrapperEngine.Models.Pages;
using Org.InCommon.InCert.BootstrapperEngine.Properties;

namespace Org.InCommon.InCert.BootstrapperEngine.Models.Panels
{
    public class ContentPanelModel : AbstractPanelModel
    {
        private AbstractPageModel _pageModel;

        public ContentPanelModel(PagedViewModel model)
            : base(model)
        {

        }

        public AbstractPageModel PageModel
        {
            get { return _pageModel; }
            set
            {
                var oldModel = _pageModel;
                _pageModel = value;
                OnPropertyChanged("Page");
                if (oldModel != null) oldModel.Dispose();
            }
        }

        public Page Page
        {
            get
            {
                return _pageModel == null ? null : _pageModel.Page;
            }
        }

        protected override void ConfigureInstall()
        {
            PageModel = new ButtonPageModel(Model)
            {
                ButtonTitle = "Install",
                Foreground = Model.Foreground,
                Background = Model.Background,
                ButtonSubTitle = Model.ProductName,
                Instructions = Resources.AbsentText.ToProductMessage(Model.ProductName),
                Command = new RelayCommand(
                    param => Model.BaseModel.Plan(InstallActions.Install),
                    param => Model.InstallationState == InstallationState.DetectedAbsent)
            };
            Model.Activate();
        }

        protected override void ConfigureStart()
        {
            PageModel = new ButtonPageModel(Model)
            {
                ButtonTitle = "Start",
                Foreground = Model.Foreground,
                Background = Model.Background,
                ButtonSubTitle = Model.BaseModel.ProductName,
                Instructions = Resources.PresentText.ToProductMessage(Model.ProductName),
                Command = new LaunchEngineCommand(Model, false)
            };
            Model.Activate();
        }

        protected override void ConfigureApplying()
        {
            PageModel = new ProgressPageModel(Model)
                {
                    Background = Model.Background,
                    Foreground = Model.Foreground,
                    Note = Model.BaseModel.PlannedAction.GetProgressNote(Model.ProductName),
                    Title = Model.BaseModel.PlannedAction.GetProgressTitle(Model.ProductName),
                    Subtitle = "initializing"
                };
            Model.Activate();
        }

        protected override void ConfigureInstallApplied()
        {
            PageModel = new ButtonPageModel(Model)
            {
                ButtonTitle = "Start",
                Foreground = Model.Foreground,
                Background = Model.Background,
                ButtonSubTitle = Model.BaseModel.ProductName,
                Instructions = Resources.PresentText.ToProductMessage(Model.ProductName),
                Command = new LaunchEngineCommand(Model, true)
            };
            Model.Activate();
        }

        protected override void ConfigureRepairApplied()
        {
            PageModel = new ButtonPageModel(Model)
            {
                ButtonTitle = "Start",
                Foreground = Model.Foreground,
                Background = Model.Background,
                ButtonSubTitle = Model.BaseModel.ProductName,
                Instructions = Resources.RepairedText.ToProductMessage(Model.ProductName),
                Command = new LaunchEngineCommand(Model, false)
            };
            Model.Activate();
        }

        protected override void ConfigureRemoveApplied()
        {
            PageModel = new MessagePageModel(Model)
                {
                    Foreground = Model.Foreground,
                    Background = Model.Background,
                    Text = Resources.UinstalledText.ToProductMessage(Model.ProductName)
                };
            Model.Activate();
        }

        protected override void ConfigureInitializing()
        {
        }

        protected override void ConfigureFailed()
        {
            PageModel = new MessagePageModel(Model)
                {
                    Foreground = Model.Foreground,
                    Background = Model.Background,
                    Text = Model.BaseModel.PlannedAction.GetIssueText(Model.ProductName, Model.ErrorCode, Model.ErrorMessage)
                };
            Model.Activate();
        }

        protected override void ConfigureNewer()
        {
            PageModel = new MessagePageModel(Model)
            {
                Foreground = Model.Foreground,
                Background = Model.Background,
                Text = Resources.NewerText.ToProductMessage(Model.ProductName)
            };
            Model.Activate();

        }

        protected override void ConfigureOlder()
        {
            PageModel = new ButtonPageModel(Model)
            {
                ButtonTitle = "Update",
                Foreground = Model.Foreground,
                Background = Model.Background,
                ButtonSubTitle = Model.ProductName,
                Instructions = Resources.AbsentText.ToProductMessage(Model.ProductName),
                Command = new RelayCommand(
                    param => Model.BaseModel.Plan(InstallActions.Upgrade),
                    param => Model.InstallationState == InstallationState.DetectedOlder)
            };
            Model.Activate();
        }

        protected override void ConfigureLaunchingNew()
        {
            PageModel = new ProgressPageModel(Model)
                {
                    Foreground = Model.Foreground,
                    Background = Model.Background,
                    Note =
                        string.Format("It sometimes takes a few seconds to start {0} the first time", Model.ProductName),
                    Title = string.Format("Starting {0}", Model.ProductName),
                    Subtitle = "please wait"
                };
        }

        protected override void ConfigureLaunchingCurrent()
        {
            PageModel = new ProgressPageModel(Model)
            {
                Foreground = Model.Foreground,
                Background = Model.Background,
                Note =
                    string.Format("It sometimes takes a few seconds to start\n{0}", Model.ProductName),
                Title = string.Format("Starting {0}", Model.ProductName),
                Subtitle = "please wait"
            };
        }

        protected override void Cancelling()
        {
            PageModel = new ProgressPageModel(Model)
            {
                Background = Model.Background,
                Foreground = Model.Foreground,
                Note = string.Format("It may take a second or two to cancel the {0}", Model.BaseModel.PlannedAction.Verb.Gerundive),
                Title = string.Format("Cancelling {0}", Model.BaseModel.PlannedAction.Verb.Gerundive.UppercaseFirst()),
                Subtitle = "please wait"
            };
        }

        protected override void ConfigureShowRemoveOptions()
        {
            PageModel = Model.ExternalEngine.UninstallOptions;
        }

        protected override void ConfigureRunningExternal()
        {
            Model.Cursor = Cursors.Wait;
            PageModel = new ExternalProgressPageModel(Model, Model.ExternalEngine.PipeName)
                {
                    Background = Model.Background,
                    Foreground = Model.Foreground,
                    Note = string.Format("It may take a few minutes to finish configuring your computer"),
                    Title = "Configuring Computer",
                    Subtitle = "please wait"
                };
            Model.Activate();
        }

        protected override void ConfigureAskExternalRetry()
        {
            Model.Cursor = Cursors.Arrow;
            PageModel = new AskingPageModel(Model)
                {
                    Background = Model.Background,
                    Foreground = Model.Foreground,
                    Title = "An issue occurred while attempting to configure your computer:",
                    Question = "Would you like to try to configure your computer again?",
                    Details = Model.ExternalEngine.IssueText,
                    Glyph = "!"
                };
            Model.Activate();
        }

        protected override void ConfigureAskCancel()
        {
            Model.Cursor = Cursors.Arrow;
            PageModel = new AskingPageModel(Model)
            {
                Background = Model.Background,
                Foreground = Model.Foreground,
                Title = string.Format("Are you sure that you want to cancel the {0}?",
                                        Model.BaseModel.PlannedAction.Verb.Gerundive),
                Question = "",
                Details = "",
                Glyph = "?"
            };
        }
    }
}
