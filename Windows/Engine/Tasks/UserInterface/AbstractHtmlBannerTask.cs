using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public abstract class AbstractHtmlBannerTask : AbstractTask
    {
        private readonly string _identifier = Guid.NewGuid().ToString();

        protected AbstractHtmlBannerTask(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Url
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ParentDialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public double Width { get; set; }

        [PropertyAllowedFromXml]
        public double Height { get; set; }

        [PropertyAllowedFromXml]
        public bool SuppressRetry { get; set; }

        [PropertyAllowedFromXml]
        public int XOffset { get; set; }

        [PropertyAllowedFromXml]
        public int YOffset { get; set; }

        [PropertyAllowedFromXml]
        public Cursor Cursor { get; set; }

        internal static void SetAddress(AbstractHtmlDialogModel dialog, string url)
        {
            dialog.SetBrowserAddress(url);
        }

        protected abstract IResult ShowBanner(IResult previousResults);

        

        public override IResult Execute(IResult previousResults)
        {
            var result = ShowBanner(previousResults);
            if (!PostProcessResult(result))
            {
                return result;
            }

            return ShowRetryDialog(result);
        }

        protected virtual IResult ShowRetryDialog(IResult issue)
        {
            SettingsManager.SetTemporarySettingString("generic issue retry banner title", "Could not display page");
            SettingsManager.SetTemporarySettingString("generic issue retry banner description", "!ApplicationTitle! was unable to load this page. Would you like to try again?");
            SettingsManager.SetTemporaryObject("generic issue retry banner stored result", issue);

            var task = new ShowHtmlBannerModal(Engine)
            {
                Url = "resource://html/GenericRetry.html",
                Dialog = Dialog,
                Width = Width,
                Height = Height,
                SuppressRetry = true
            };

            return task.Execute(issue);
        }

        protected void ConfigurePosition(AbstractDialogModel dialog, AbstractDialogModel parent)
        {
            if (parent == null)
            {
                dialog.StartupLocation = WindowStartupLocation.CenterScreen;
                return;
            }

            if (XOffset == 0 && YOffset == 0)
            {
                dialog.StartupLocation = WindowStartupLocation.CenterOwner;
                return;
            }

            dialog.StartupLocation = WindowStartupLocation.Manual;
            dialog.Left = parent.Left + XOffset;
            dialog.Top = parent.Top + YOffset;
        }

        private bool PostProcessResult(IResult result)
        {
            if (SuppressRetry)
            {
                return false;
            }

            if (result.IsOk())
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(ErrorBranch))
            {
                return false;
            }

            var instance = result as CouldNotLoadHtmlContent;
            return instance != null && instance.IsExternalUrl;
        }

        protected static IResult WaitForLoad(AbstractHtmlDialogModel dialog)
        {
            var model = dialog.ContentModel as BrowserContentModel;
            if (model == null)
            {
                return new NextResult();
            }
            while (!model.IsLoaded)
            {
                Application.Current.DoEvents(250);
            }

            return dialog.Result ?? new NextResult();
        }

        protected HtmlBanner GetOrCreateBanner()
        {
            var banner = BannerManager.GetBanner(_identifier) as HtmlBanner;
            if (banner != null)
            {
                return banner;
            }
            
            banner = new HtmlBanner(Engine)
            {
                Width = Width,
                Height = Height,
                CanClose = true,
                Margin = new Thickness(0),
                Url = Url,
                Cursor = Cursor
            };

            
            return BannerManager.SetBanner(_identifier, banner) as HtmlBanner;
        }

        public override void ConfigureFromNode(XElement node)
        {
            base.ConfigureFromNode(node);

            if (Width <= 0) Width = 600;
            if (Height <= 0) Height = 600;
        }

        protected class DialogInstanceNotFound : Exception
        {
            public string Dialog { get; private set; }

            public DialogInstanceNotFound(string dialog)
            {
                Dialog = dialog;
            }
        }

        protected T GetBannerDialog<T>() where T : AbstractHtmlDialogModel
        {
            var result = DialogsManager.GetDialog<T>(Dialog);
            if (result == null)
            {
                throw new DialogInstanceNotFound(Dialog);
            }
            return result;
        }

        protected AbstractDialogModel GetParentDialog()
        {
            if (string.IsNullOrWhiteSpace(ParentDialog))
            {
                return null;
            }

            var result = DialogsManager.GetExistingDialog(ParentDialog);
            if (result == null)
            {
                throw new DialogInstanceNotFound(ParentDialog);
            }

            return result;
        }
    }
}
