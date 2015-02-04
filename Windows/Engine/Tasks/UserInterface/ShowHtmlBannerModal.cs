using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowHtmlBannerModal : AbstractHtmlBannerTask
    {
        public ShowHtmlBannerModal(IEngine engine)
            : base(engine)
        {
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetDialog<BorderedDialogModel>(Dialog);
                if (dialog == null)
                    return new DialogInstanceNotFound { Dialog = Dialog };

                var banner = GetOrCreateBanner();

                DialogsManager.ActiveDialogKey = Dialog;

                dialog.PreloadContent(banner);
                SetAddress(dialog, Url);
                return dialog.ShowBannerModal(banner);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }
        
        public override string GetFriendlyName()
        {
            return string.Format("Show html banner {0} in model dialog {1}", Url, Dialog);
        }
    }
}
