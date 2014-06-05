using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class BorderlessChildDialogModel:BorderlessDialogModel
    {
        public BorderlessChildDialogModel(IEngine engine) : base(engine)
        {
            ShowInTaskbar = false;
        }

        protected override void RaiseCloseQuestion()
        {
            // child dialogs should not raise close question
        }
    }
}
