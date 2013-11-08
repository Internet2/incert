using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class UpdatableParagraph:AbstractContentWrapper
    {
        public UpdatableParagraph(IEngine engine)
            : base(engine)
        {
        }
        
        public override System.Type GetSupportingModelType()
        {
            return typeof (UpdatableTextContentModel);
        }
    }
}
