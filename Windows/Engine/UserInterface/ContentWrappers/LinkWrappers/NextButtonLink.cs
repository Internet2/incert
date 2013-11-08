using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers
{
    class NextButtonLink:AbstractLink
    {
        public NextButtonLink(IEngine engine) : base(engine)
        {
        }

        public override Type GetPreferredModelType()
        {
            return typeof(NextButtonHyperlinkModel);
        }
    }
}
