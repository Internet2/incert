using System.Collections.Generic;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    interface IHasControlActions
    {
        void ClearActions();
        void AddAction(AbstractControlAction action);
        void AddActions(List<AbstractControlAction> actions);
        void DoActions(bool includeOneTime);
       
    }
}
