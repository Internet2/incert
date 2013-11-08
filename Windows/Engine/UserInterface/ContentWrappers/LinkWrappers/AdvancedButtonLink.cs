using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers
{
    class AdvancedButtonLink:AbstractLink
    {
        public AdvancedButtonLink(IEngine engine) : base(engine)
        {
        }

        public override Type GetPreferredModelType()
        {
            return typeof (AdvancedButtonHyperlinkModel);
        }
    }
}
