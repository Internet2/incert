using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class BorderlessChildDialogModel:BorderlessDialogModel
    {
        public BorderlessChildDialogModel(IDialogsManager dialogsManager, IBannerManager bannerManager, IAppearanceManager appearanceManager) : base(dialogsManager, bannerManager, appearanceManager)
        {
            ShowInTaskbar = false;
        }

        protected override void RaiseCloseQuestion()
        {
            // child dialogs should not raise close question
        }
    }
}
