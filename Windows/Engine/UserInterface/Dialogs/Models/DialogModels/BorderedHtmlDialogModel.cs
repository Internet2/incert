using System.Windows;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels
{
    public class BorderedHtmlDialogModel:AbstractHtmlDialogModel
    {
        public BorderedHtmlDialogModel(IEngine engine) : base(engine, new StarkBorderedWindow())
        {
            ShowInTaskbar = true;
            WindowStyle = WindowStyle.SingleBorderWindow; 
        }
    }
}
