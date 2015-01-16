using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    public class BrowserContentModel:AbstractContentModel
    {
        public BrowserContentModel(AbstractModel parentModel) : base(parentModel)
        {
            
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            var content = new BrowserControl {DataContext = this};
            InitializeBindings(content);

            return content as T;

        }
    }
}
