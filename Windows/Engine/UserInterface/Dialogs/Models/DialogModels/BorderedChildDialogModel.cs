using System.Windows;
using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    class BorderedChildDialogModel:BorderedDialogModel
    {
        public BorderedChildDialogModel(IEngine engine) : base(engine)
        {
            ShowInTaskbar = false;
            WindowStyle = WindowStyle.ToolWindow;
        }

    
    }
}
