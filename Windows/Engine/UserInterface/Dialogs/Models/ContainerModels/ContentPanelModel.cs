using System.Windows;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels
{
    public class ContentPanelModel : AbstractContainerModel
    {
        public ContentPanelModel(AbstractModel parentModel)
            : base(parentModel)
        {
        }

        public ContentPanelModel(AbstractDialogModel parentModel) : base(parentModel)
        {
            
        }

        protected override DependencyObject CreateInstance(AbstractBanner banner)
        {
            return new ContentPanelInstance {DataContext = this};
        }

        public override T LoadContent<T>(AbstractBanner banner)
        {
            var result = base.LoadContent<T>(banner);
            if (result == null)
                return default(T);
            
            Margin = banner.Margin;
            return result;
        }

       
    }
}
