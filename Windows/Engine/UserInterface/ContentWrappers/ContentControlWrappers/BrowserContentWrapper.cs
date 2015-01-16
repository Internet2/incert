using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers
{
    class BrowserContentWrapper:AbstractContentWrapper
    {
        public string Url { get; set; }
        
        public BrowserContentWrapper(IEngine engine) : base(engine)
        {
            
        }
    }
}
