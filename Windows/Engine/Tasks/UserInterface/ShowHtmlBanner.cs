using log4net;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowHtmlBanner:AbstractHtmlBannerTask
    {
        private static readonly ILog Log = Logger.Create();

        public ShowHtmlBanner(IEngine engine) : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            var dialog = DialogsManager.GetDialog<BorderedDialogModel>(Dialog);
            if (dialog == null)
                return new DialogInstanceNotFound { Dialog = Dialog };

            var banner = GetOrCreateBanner();

            DialogsManager.ActiveDialogKey = Dialog;

            dialog.PreloadContent(banner);
            SetAddress(dialog, Url);
           
            var result = dialog.ShowBanner(banner);
            if (!result.IsOk())
            {
                return result;
            }

            WaitForLoad(dialog);

            return result;
        }

        public override string GetFriendlyName()
        {
            return string.Format("Show html banner ({0}) in dialog {1}", Url, Dialog);
        }
    }
}
