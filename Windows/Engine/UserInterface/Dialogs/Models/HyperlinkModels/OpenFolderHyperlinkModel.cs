using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Commands;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels
{
    class OpenFolderHyperlinkModel:AbstractHyperlinkModel
    {
        public OpenFolderHyperlinkModel(AbstractModel parentModel) : base(parentModel)
        {
        }

        public override void LoadContent(AbstractLink wrapper)
        {
            base.LoadContent(wrapper);
            Command = new RelayCommand(param =>PathUtilities.OpenFolderPath(wrapper.Target));
        }

        public override Brush TextBrush
        {
            get
            {
                if (!Parent.Enabled)
                    return AppearanceManager.MakeBrushTransparent(LinkBrush as SolidColorBrush, 45);

                return LinkBrush;

            }
        }
    }

  
}
