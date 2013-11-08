using System.Windows;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class BorderedDialogModel : AbstractDialogModel
    {

        public BorderedDialogModel(IDialogsManager dialogsManager, IBannerManager bannerManager, IAppearanceManager appearanceManager)
            : base(dialogsManager, bannerManager, appearanceManager, new StarkBorderedWindow())
        {
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.SingleBorderWindow; 
        }
    }
}
