using System.Collections.Generic;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    public interface IControlAction
    {
        List<string> ControlKeys { get; }
        bool OneTime { get; set; }
        void DoAction(AbstractModel model, bool includeOneTime);
    }

}
