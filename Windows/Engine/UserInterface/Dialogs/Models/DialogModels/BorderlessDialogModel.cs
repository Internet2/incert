using System.Windows.Media.Effects;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class BorderlessDialogModel : AbstractDialogModel
    {

        public BorderlessDialogModel(IDialogsManager dialogsManager, IBannerManager bannerManager, IAppearanceManager appearanceManager)
            : base(dialogsManager, bannerManager, appearanceManager, new BorderlessWindow())
        {
            ShowInTaskbar = true;
            SuppressCloseQuestion = true;
        }

        private DropShadowEffect _dropShadow;
        

        public DropShadowEffect DropShadow
        {
            get
            {
                return _dropShadow;
            }
            set
            {
                _dropShadow = value;
                OnPropertyChanged();
            }

        }

    }
}
