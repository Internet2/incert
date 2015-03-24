using System.Windows;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions
{
    class SetFocusAction:AbstractControlAction
    {
        public SetFocusAction(IEngine engine) : base(engine)
        {
        }

        public override void DoAction(AbstractModel model, bool includeOneTime)
        {
            if (!IsOneTimeOk(includeOneTime))
                return;
            
            if (model == null)
                return;

            var result = EvaluateConditions();
            if (!result.Result)
                return;

            if (model.Content == null)
                return;

            if (model.RootDialogModel == null)
                return;

            if (model.RootDialogModel.DialogInstance == null)
                return;

            var element = model.Content as IInputElement;
            if (element == null)
                return;

            FocusManager.SetFocusedElement(model.RootDialogModel.DialogInstance, element);
            Keyboard.Focus(element);
            Application.Current.DoEvents(250);
        }
    }
}
