using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class SimpleParagraph:AbstractContentWrapper
    {
        public SimpleParagraph(IEngine engine):
            base(engine)
        {
        }

        public override Type GetSupportingModelType()
        {
            return typeof (TextContentModel);
        }

        public override bool Initialized()
        {
            return true;
        }
    }
}
