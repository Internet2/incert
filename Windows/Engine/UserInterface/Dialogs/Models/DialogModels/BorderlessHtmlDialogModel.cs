using System.Windows.Media.Effects;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;
using System.Windows;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    public class BorderlessHtmlDialogModel : AbstractHtmlDialogModel
    {
        public BorderlessHtmlDialogModel(IEngine engine)
            : base(engine, new BorderlessWindow())
        {
            ShowInTaskbar = true;
            SuppressCloseQuestion = true;
            WindowStyle = WindowStyle.None;
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
