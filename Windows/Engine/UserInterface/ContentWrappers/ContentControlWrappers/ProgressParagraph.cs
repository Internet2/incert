using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class ProgressParagraph : AbstractContentWrapper
    {
        public ProgressParagraph(IEngine engine)
            : base(engine)
        {
        }

        public override bool Initialized()
        {
            return true;
        }

        public override System.Type GetSupportingModelType()
        {
            return typeof (ProgressTextContentModel);
        }
   }
}
