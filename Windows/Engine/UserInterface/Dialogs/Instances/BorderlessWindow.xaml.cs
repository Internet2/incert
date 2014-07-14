using System.Windows;
using System.Windows.Input;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances
{
    /// <summary>
    /// Interaction logic for BorderlessWindow.xaml
    /// </summary>
    public partial class BorderlessWindow
    {
        public BorderlessWindow()
        {
            InitializeComponent();
        }

        private void GridMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var window = UserInterfaceUtilities.GetTopLevelControl(this) as Window;
            if (window == null)
                return;

            window.DragMove();
            e.Handled = true;
        }


    }
}
