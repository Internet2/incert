﻿using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers
{
    class DisabledButton:AbstractButtonWrapper
    {
        public DisabledButton(IEngine engine) : base(engine)
        {
            Enabled = false;
        }

       
      
    }
}
