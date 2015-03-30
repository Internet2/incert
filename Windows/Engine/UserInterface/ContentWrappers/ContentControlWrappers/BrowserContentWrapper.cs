using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContainerModels;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class BrowserContentWrapper:AbstractContentWrapper
    {
        public Uri Uri { get; set; }
        public bool SilentMode { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public BrowserContentWrapper(IEngine engine) : base(engine)
        {
            
        }

        public override Type GetSupportingModelType()
        {
            return typeof (BrowserContentModel);
        }
    }
}
