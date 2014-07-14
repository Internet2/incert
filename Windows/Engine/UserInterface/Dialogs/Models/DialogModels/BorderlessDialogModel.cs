using System.Windows.Media.Effects;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class BorderlessDialogModel : AbstractDialogModel
    {

        public BorderlessDialogModel(IEngine engine)
            : base(engine, new BorderlessWindow())
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
