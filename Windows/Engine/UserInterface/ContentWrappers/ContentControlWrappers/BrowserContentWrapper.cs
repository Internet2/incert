using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class BrowserContentWrapper:AbstractContentWrapper
    {
        public string Url { get; set; }
        
        public BrowserContentWrapper(IEngine engine) : base(engine)
        {
            
        }

        public override Type GetSupportingModelType()
        {
            return typeof (BrowserContentModel);
        }
    }
}
