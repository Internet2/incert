using System.Windows;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class BorderedChildDialogModel:BorderedDialogModel
    {
        public BorderedChildDialogModel(IDialogsManager dialogsManager, IBannerManager bannerManager, IAppearanceManager appearanceManager) : base(dialogsManager, bannerManager, appearanceManager)
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.ToolWindow;
        }

    
    }
}
