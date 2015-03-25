using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowHtmlBannerModal : AbstractHtmlBannerTask
    {
        public ShowHtmlBannerModal(IEngine engine)
            : base(engine)
        {
        }

        protected override IResult ShowBanner(IResult previousResults)
        {
            try
            {
                var dialog = GetBannerDialog<BorderedDialogModel>();
                var parent = GetParentDialog();
                
                var banner = GetOrCreateBanner();

                DialogsManager.ActiveDialogKey = Dialog;

                dialog.PreloadHtmlContent(banner, Url);
                
                ConfigurePosition(dialog, parent);
                
                return (parent == null)
                    ? dialog.ShowBannerModal(banner)
                    : parent.ShowChildBannerModal(dialog, banner);
            }
            catch (DialogInstanceNotFound e)
            {
                return new Results.Errors.UserInterface.DialogInstanceNotFound {Dialog = e.Dialog};
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
